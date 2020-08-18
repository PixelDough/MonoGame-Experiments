using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Tile : Solid
    {
        public Tilemap Tilemap;
        public TilemapLayer TilemapLayer;
        public int IdOnTilemap = -1;

        private Texture2D _texture;
        private Vector2 _position;
        public Vector2 Position
        {
            get { return Vector2.Round(_position); }
        }
        private Rectangle _spriteRectangle;
        public Rectangle SpriteRectangle { get { return _spriteRectangle; } }

        public Tile(Texture2D texture, Vector2 position, Rectangle spriteRectangle) : base()
        {
            _texture = texture;
            _position = position;
            _spriteRectangle = spriteRectangle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _spriteRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
