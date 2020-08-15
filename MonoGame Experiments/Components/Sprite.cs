using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Components
{
    class Sprite : Component
    {
        private Texture2D _texture;
        private int _width;
        private int _height;
        private Vector2 _position;

        public Sprite(Texture2D texture, int width, int height, Vector2 position)
        {
            _texture = texture;
            _width = width;
            _height = height;
            _position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            //_position.X += 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Input.IsKeyDown(Keys.Up))
                _position.Y -= 1;
            if (Input.IsKeyDown(Keys.Down))
                _position.Y += 1;
            if (Input.IsKeyDown(Keys.Left))
                _position.X -= 1;
            if (Input.IsKeyDown(Keys.Right))
                _position.X += 1;

        }
    }
}
