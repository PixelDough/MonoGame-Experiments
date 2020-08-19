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
        private Vector2 _localPosition;
        private Vector2 _position;

        public Sprite(Texture2D texture, int width, int height, Vector2 localPosition)
        {
            _texture = texture;
            _width = width;
            _height = height;
            _localPosition = localPosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height), null, Color.White, Transform.Rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 0f);
            
        }

        public override void Update(GameTime gameTime)
        {
            _position = _baseObject.transform.Position + _localPosition;
        }
    }
}
