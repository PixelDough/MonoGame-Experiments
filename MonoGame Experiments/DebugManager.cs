using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments
{
    public static class DebugManager
    {

        private static Queue<IDrawType> _drawCalls = new Queue<IDrawType>();
        private static List<string> _previousCommands = new List<string>();

        public static bool ShowCollisionRectangles = false;
        
        public static void DrawSprite(Vector2 position, Texture2D texture, Color color)
        {
            DrawTypeSprite drawType = new DrawTypeSprite(position, texture, color);
            _drawCalls.Enqueue(drawType);
        }

        public static void DrawText(Vector2 position, SpriteFont spriteFont, string text, Color color)
        {
            DrawTypeText drawType = new DrawTypeText(position, spriteFont, text, color);
            _drawCalls.Enqueue(drawType);
        }
        
        public static void Draw(SpriteBatch spriteBatch)
        {
            while (_drawCalls.Count > 0)
            {
                IDrawType drawType = _drawCalls.Dequeue();
                drawType.Draw(spriteBatch);
            }
        }

        private interface IDrawType
        {
            public void Draw(SpriteBatch spriteBatch);
        }

        private class DrawTypeSprite : IDrawType
        {
            private Vector2 _position;
            private Texture2D _texture;
            private Color _color;
            public DrawTypeSprite(Vector2 position, Texture2D texture, Color color)
            {
                _position = position;
                _texture = texture;
                _color = color;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(_texture, _position, null, _color, 0f, Vector2.Zero, 1, SpriteEffects.None, 1f);
            }
        }

        private class DrawTypeText : IDrawType
        {
            private Vector2 _position;
            private SpriteFont _font;
            private string _text;
            private Color _color;

            public DrawTypeText(Vector2 position, SpriteFont font, string text, Color color)
            {
                _position = position;
                _font = font;
                _text = text;
                _color = color;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.DrawString(_font, _text, _position, _color, 0, new Vector2(0, 12), 1, SpriteEffects.None, .5f);
            }
        }
    }
}
