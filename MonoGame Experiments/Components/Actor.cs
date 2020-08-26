using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Components
{
    // The actor class itself has no concept of it's own speed, velocity, or gravity.
    // Classes that extend the Actor class keep track of those things.
    public class Actor : Component, IRigidbody
    {
        private Collider _collider;
        public Collider Collider {
            get
            {
                if (_collider != null) return _collider;
                else
                {
                    _collider = _entity.GetComponent<Collider>();
                    return _collider;
                }
            }
        }

        private float _velocityCap = 10f;

        public Actor()
        {

        }

        public override void Initialize(Entity baseObject)
        {
            base.Initialize(baseObject);

            Game._currentScene.Actors.Add(this);
            Collider.DebugColor = Color.LawnGreen;
        }

        public override void Update(GameTime gameTime)
        {
            Collider.CollisionResults.ClearSides();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public bool MoveX(float amount, Action onCollide)
        {
            amount = Math.Clamp(amount, -_velocityCap, _velocityCap);
            float xRemainder = amount;
            int move = (int)Math.Round(xRemainder);

            Collider.RefreshPositionToTransform();
            if (move != 0)
            {
                xRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    bool hitSolid = Collider.CollideAt(Game._currentScene.Solids, Collider.Position + (Vector2.UnitX * sign), true);

                    if (!hitSolid)
                    {
                        // No solid immediately beside us
                        Transform.Position += Vector2.UnitX * sign;
                        Collider.RefreshPositionToTransform();
                        move -= sign;
                    }
                    else
                    {
                        // Hit a solid!
                        onCollide?.Invoke();
                        if (sign <= 0) Collider.CollisionResults.SetSide(Collider.CollisionResult.Side.Left);
                        else Collider.CollisionResults.SetSide(Collider.CollisionResult.Side.Right);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool MoveY(float amount, Action onCollide, int jumpCornerCorrectionAmount = 0)
        {
            amount = Math.Clamp(amount, -_velocityCap, _velocityCap);
            float yRemainder = amount;
            int move = (int)Math.Round(yRemainder);

            Collider.RefreshPositionToTransform();
            if (move != 0)
            {
                yRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    Collider.RefreshPositionToTransform();
                    bool hitSolid = Collider.CollideAt(Game._currentScene.Solids, Collider.Position + (Vector2.UnitY * sign), true);

                    if (!hitSolid)
                    {
                        // No solid immediately beside us
                        Transform.Position += Vector2.UnitY * sign;
                        Collider.RefreshPositionToTransform();
                        move -= sign;
                    }
                    else
                    {
                        // This could potentially be refactored. Right now it seems like an odd way of doing it. But it works :)
                        bool hitCornerJump = false;
                        if (jumpCornerCorrectionAmount != 0 && sign < 0)
                        {
                            hitCornerJump = JumpCornerCorrection(jumpCornerCorrectionAmount);
                        }
                        
                        if (!hitCornerJump)
                        {
                            // Hit a solid!
                            onCollide?.Invoke();
                            if (sign <= 0) Collider.CollisionResults.SetSide(Collider.CollisionResult.Side.Up);
                            else Collider.CollisionResults.SetSide(Collider.CollisionResult.Side.Down);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool JumpCornerCorrection(int jumpCornerCorrectionAmount)
        {
            for (int i = 1; i <= jumpCornerCorrectionAmount; i++)
            {
                for (int j = 1; j >= -1; j -= 2)
                {
                    bool hitSolid = Collider.CollideAt(Game._currentScene.Solids, Collider.Position + new Vector2(i * j, -1), true);

                    if (!hitSolid)
                    {
                        Transform.Position += Vector2.UnitX * (i * j);
                        Collider.RefreshPositionToTransform();
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool IsRiding(Solid solid)
        {
            if (Collider.CollisionCheck(solid.Collider, Vector2.UnitY))
            {
                return true;
            }
            return false;
        }

        public virtual void Squish()
        {

        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Game._currentScene.Actors.Remove(this);
        }

    }
}
