using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Tile : Component
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _spriteRectangle;
        public Tile(Texture2D texture, Vector2 position, Rectangle spriteRectangle)
        {
            _texture = texture;
            _position = position;
            _spriteRectangle = spriteRectangle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _spriteRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            //if (Input.IsKeyDown(Keys.Up))
            //    _position.Y -= 1;
            //if (Input.IsKeyDown(Keys.Down))
            //    _position.Y += 1;
            //if (Input.IsKeyDown(Keys.Left))
            //    _position.X -= 1;
            //if (Input.IsKeyDown(Keys.Right))
            //    _position.X += 1;
        }
    }
}
