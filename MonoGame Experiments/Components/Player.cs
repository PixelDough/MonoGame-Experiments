using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Player : Actor
    {
        private bool _initialized = false;

        public Player() : base()
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
                moveAmount -= Vector2.UnitY * 1;
            if (Input.IsKeyDown(Keys.Down))
                moveAmount += Vector2.UnitY * 1;
            if (Input.IsKeyDown(Keys.Left))
                moveAmount -= Vector2.UnitX * 1;
            if (Input.IsKeyDown(Keys.Right))
                moveAmount += Vector2.UnitX * 1;

            Transform.Rotation += MathHelper.ToRadians(1);

            MoveX(moveAmount.X, null);
            MoveY(moveAmount.Y, null);

            Collider.Update(gameTime);

            // TODO: Fix case where there is no x or y axis collision, but movement goes into a solid.
            //_baseObject.transform.Position += moveAmount;

            Game.Camera.Position = _baseObject.transform.Position - new Vector2(Game.Camera.Viewport.Width / 2, Game.Camera.Viewport.Height / 2);
        }

    }
}
