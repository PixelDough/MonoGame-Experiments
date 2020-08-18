using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MonoGame_Experiments
{
    public class Collider : Component
    {
        public Vector2 Position
        {
            get { return _baseObject.transform.Position + _localPosition; }
            set { _position = Vector2.Round(value); }
        }
        private Vector2 _position = Vector2.Zero;
        private Vector2 _localPosition = Vector2.Zero;

        public static List<Collider> Colliders = new List<Collider>();

        public int Width { get; set; }
        public int Height { get; set; }

        public Collider(Vector2 localPosition, int width, int height)
        {
            _localPosition = localPosition;
            Width = width;
            Height = height;

            Colliders.Add(this);
        }

        public int Top
        {
            get { return (int)_position.Y; }
            set { _position.Y = value; }
        }

        public int Bottom
        {
            get { return (int)_position.Y + Height; }
            set { _position.Y = value - Height; }
        }

        public int Left
        {
            get { return (int)_position.X; }
            set { _position.X = value; }
        }

        public int Right
        {
            get { return (int)_position.X + Width; }
            set { _position.X = value - Width; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(x:(int)Position.X, y:(int)Position.Y, width:(int)Width, height:(int)Height);
            }
        }

        private Texture2D _rectangleTexture;
        public Texture2D RectangleTexture
        {
            get
            {
                if (_rectangleTexture == null)
                {
                    List<Color> colors = new List<Color>();
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                            {
                                colors.Add(Color.White);
                            }
                            else
                            {
                                colors.Add(Color.Transparent);
                            }
                        }
                    }
                    _rectangleTexture = new Texture2D(Game.Graphics.GraphicsDevice, Width, Height);
                    _rectangleTexture.SetData(colors.ToArray());
                }
                return _rectangleTexture;
            }
        }

        public bool CollisionCheck(Collider other, Vector2? localOffset = null)
        {
            Vector2 _localOffset = Vector2.Zero;
            if (localOffset != null) _localOffset = (Vector2)localOffset;

            if (this.Left + _localOffset.X < other.Right
                && this.Right + _localOffset.X > other.Left
                && this.Top + _localOffset.Y < other.Bottom
                && this.Bottom + _localOffset.Y > other.Top)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Position = _baseObject.transform.Position + _localPosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Game.DebugMode)
                spriteBatch.Draw(RectangleTexture, Position, Color.Multiply(Color.Red, 0.75f));
        }
    }
}
