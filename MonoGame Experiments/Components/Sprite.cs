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
        private float _depth;
        private Rectangle? _spriteRectangle;

        public bool FlipX = false;

        public Sprite(Entity entity, Texture2D texture, int width, int height, Vector2 localPosition, float depth = 1f, Rectangle? spriteRectangle = null) : base(entity)
        {
            _texture = texture;
            _width = width;
            _height = height;
            _localPosition = localPosition;
            _depth = depth;
            _spriteRectangle = spriteRectangle ?? null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture, 
                new Rectangle(
                    (int)Transform.Position.X + (int)_localPosition.X, 
                    (int)Transform.Position.Y + (int)_localPosition.Y, 
                    _width, 
                    _height), 
                _spriteRectangle, 
                Color.White, 
                0, 
                new Vector2(0, 0), 
                FlipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
                _depth);
            
        }
        
        public override void Update(GameTime gameTime)
        {
            //_position = _baseObject.transform.Position + _localPosition;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            //_texture.Dispose();
        }
    }
}
