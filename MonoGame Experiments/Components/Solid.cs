using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Solid : Component, IRigidbody
    {
        public enum SolidTypes
        {
            Block,
            Tilemap
        }
        public SolidTypes SolidType = SolidTypes.Block;
        public Collider Collider { get { return _entity.GetComponent<Collider>(); } }
        private bool _active = false;

        public Solid(SolidTypes solidType)
        {
            SolidType = solidType;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Game._currentScene.Solids.Contains(this))
            {
                Game._currentScene.Solids.Add(this);
            }
            //float myDiagLengthSquared = (Collider.Width * Collider.Width) + (Collider.Height * Collider.Height);

            //_active = false;
            //foreach (Actor actor in Game._currentScene.Actors)
            //{
            //    float diagLengthSquared = (float)Math.Pow(actor.Collider.Width, 2) + (float)Math.Pow(actor.Collider.Height, 2);
            //    float dist = Vector2.DistanceSquared(Collider.Center, actor.Collider.Center);
            //    if (dist <= diagLengthSquared + myDiagLengthSquared + 10)
            //    {
            //        _active = true;
            //    }
            //}

            //if (_active)
            //{
            //    Collider.DebugColor = Color.Red;
            //    if (!Game._currentScene.Solids.Contains(this)) Game._currentScene.Solids.Add(this);
            //}
            //else
            //{
            //    Collider.DebugColor = Color.DarkRed;
            //    if (Game._currentScene.Solids.Contains(this)) Game._currentScene.Solids.Remove(this);
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public bool MoveX(float amount, Action onCollide)
        {
            return false;
        }
        public bool MoveY(float amount, Action onCollide, int jumpCornerCorrectionAmount = 0)
        {
            return false;
        }

        private List<Actor> GetAllRidingActors()
        {
            List<Actor> returnValue = new List<Actor>();

            foreach (Actor actor in Game._currentScene.Actors)
            {
                if (actor.IsRiding(this))
                {
                    returnValue.Add(actor);
                }
            }

            return returnValue;
        }

        public void Move(float x, float y)
        {
            float xRemainder = x;
            float yRemainder = y;
            int moveX = (int)Math.Round(xRemainder);
            int moveY = (int)Math.Round(yRemainder);

            Vector2 pos = Transform.Position;

            if (moveX != 0 || moveY != 0)
            {
                // Loop through every Actor in the Level
                // Add it to a list if actor.IsRiding(this) is true
                List<Actor> riding = GetAllRidingActors();

                // Make this Solid non-collidable for Actors,
                // so that Actors moved by it do not get stuck on it
                Collider.Collidable = false;


                if (moveX != 0)
                {
                    xRemainder -= moveX;
                    pos.X += moveX;
                    Transform.Position = pos;
                    //Collider.RefreshPositionToTransform();

                    if (moveX > 0)
                    {
                        foreach (Actor actor in Game._currentScene.Actors)
                        {
                            if (Collider.CollisionCheck(actor.Collider, Vector2.Zero))
                            {
                                // Push right
                                actor.MoveX(Collider.Right - actor.Collider.Left, new Action(actor.Squish));
                            }
                            else if (riding.Contains(actor))
                            {
                                // Carry right
                                actor.MoveX(moveX, null);
                            }
                        }
                    }
                    else
                    {
                        foreach (Actor actor in Game._currentScene.Actors)
                        {
                            if (Collider.CollisionCheck(actor.Collider, Vector2.Zero))
                            {
                                // Push left
                                actor.MoveX(Collider.Left - actor.Collider.Right, new Action(actor.Squish));
                            }
                            else if (riding.Contains(actor))
                            {
                                // Carry left
                                actor.MoveX(moveX, null);
                            }
                        }
                    }
                }

                if (moveY != 0)
                {
                    // Do y-axis movement
                    yRemainder -= moveY;
                    pos.Y += moveY;
                    Transform.Position = pos;
                    //Collider.RefreshPositionToTransform();

                    if (moveY > 0)
                    {
                        foreach (Actor actor in Game._currentScene.Actors)
                        {
                            if (Collider.CollisionCheck(actor.Collider, Vector2.Zero))
                            {
                                // Push down
                                actor.MoveY(Collider.Bottom - actor.Collider.Top + 1, new Action(actor.Squish));
                                //actor.MoveY(moveY, null);
                            }
                            else if (riding.Contains(actor))
                            {
                                // Carry down
                                actor.MoveY(moveY, null);
                            }
                        }
                    }
                    else
                    {
                        foreach (Actor actor in Game._currentScene.Actors)
                        {
                            if (Collider.CollisionCheck(actor.Collider, -Vector2.Zero))
                            {
                                // Push up
                                actor.MoveY(Collider.Top - actor.Collider.Bottom - 1, new Action(actor.Squish));
                                //actor.MoveY(moveY, null);

                            }
                            else if (riding.Contains(actor))
                            {
                                // Carry up
                                actor.MoveY(moveY, null);
                            }
                        }
                    }
                }

                // Re-enable collisions for this Solid
                Collider.Collidable = true;

                Transform.Position = pos;
                Collider.RefreshPositionToTransform();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Game._currentScene.Solids.Remove(this);
            Dispose();
        }
    }
}
