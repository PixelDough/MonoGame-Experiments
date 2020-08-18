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
        public Collider Collider { get; set; }
        public static List<IRigidbody> Actors = new List<IRigidbody>();

        public Actor()
        {
            Actors.Add(this);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public void MoveX(float amount, Action onCollide)
        {
            float xRemainder = amount;
            int move = (int)Math.Round(xRemainder);

            if (move != 0)
            {
                xRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    if (!Collider.CollideAt(Solid.Solids, Collider.Position + (Vector2.UnitX * sign)))
                    {
                        // No solid immediately beside us
                        Transform.Position += Vector2.UnitX * sign;
                        move -= sign;
                    }
                    else
                    {
                        // Hit a solid!
                        onCollide?.Invoke();
                        break;
                    }
                }
            }
        }

        public void MoveY(float amount, Action onCollide)
        {
            float yRemainder = amount;
            int move = (int)Math.Round(yRemainder);

            if (move != 0)
            {
                yRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    if (!Collider.CollideAt(Solid.Solids, Collider.Position + (Vector2.UnitY * sign)))
                    {
                        // No solid immediately beside us
                        Transform.Position += Vector2.UnitY * sign;
                        move -= sign;
                    }
                    else
                    {
                        // Hit a solid!
                        onCollide?.Invoke();
                        break;
                    }
                }
            }
        }
    }
}
