using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Player : Component
    {
        public Collider Collider;
        private bool _initialized = false;

        public Player()
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (!_initialized)
            {
                if (Collider == null)
                    Collider = _baseObject.GetComponent<Collider>();
            }

            Vector2 moveAmount = Vector2.Zero;
            if (Input.IsKeyDown(Keys.Up))
                moveAmount -= Vector2.UnitY * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Input.IsKeyDown(Keys.Down))
                moveAmount += Vector2.UnitY * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Input.IsKeyDown(Keys.Left))
                moveAmount -= Vector2.UnitX * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Input.IsKeyDown(Keys.Right))
                moveAmount += Vector2.UnitX * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Collider != null)
            {
                foreach (Collider otherCollider in Collider.Colliders)
                {
                    if (otherCollider == Collider) continue;
                    if (Collider.CollisionCheck(otherCollider, moveAmount * Vector2.UnitY))
                    {
                        moveAmount.Y = 0;
                    }
                    
                }
            }
            _baseObject.transform.Position += moveAmount * Vector2.UnitY;
            Collider.Update(gameTime);

            if (Collider != null)
            {
                foreach (Collider otherCollider in Collider.Colliders)
                {
                    if (otherCollider == Collider) continue;
                    if (Collider.CollisionCheck(otherCollider, moveAmount * Vector2.UnitX))
                    {
                        moveAmount.X = 0;
                    }

                }
            }
            _baseObject.transform.Position += moveAmount * Vector2.UnitX;
            Collider.Update(gameTime);

            // TODO: Fix case where there is no x or y axis collision, but movement goes into a solid.
            //_baseObject.transform.Position += moveAmount;

            Game.Camera.Position = _baseObject.transform.Position - new Vector2(Game.Camera.Viewport.Width / 2, Game.Camera.Viewport.Height / 2);
        }

    }
}
