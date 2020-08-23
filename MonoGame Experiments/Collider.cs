using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Components;
using MonoGame_Experiments.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MonoGame_Experiments
{
    public class Collider : Component
    {
        public bool Collidable { get; set; } = true;
        public Vector2 Position
        {
            get { return Vector2.Round(Transform.Position + _localPosition); }
            set { _position = Vector2.Round(value); }
        }
        private Vector2 _position = Vector2.Zero;
        private Vector2 _localPosition = Vector2.Zero;

        public static List<Collider> Colliders = new List<Collider>();

        public Color DebugColor = Color.Red;

        public int Width { get; set; }
        public int Height { get; set; }

        public Collider(Vector2 localPosition, int width, int height)
        {
            Width = width;
            Height = height;
            _localPosition = localPosition;

            Colliders.Add(this);
        }

        public int Top
        {
            get { return (int)Position.Y; }
            set { _position.Y = value; }
        }

        public int Bottom
        {
            get { return (int)Position.Y + Height; }
            set { _position.Y = value - Height; }
        }

        public int Left
        {
            get { return (int)Position.X; }
            set { _position.X = value; }
        }

        public int Right
        {
            get { return (int)Position.X + Width; }
            set { _position.X = value - Width; }
        }

        public Vector2 BottomCenter
        {
            get { return new Vector2(Width / 2, Bottom); }
            set
            {
                _localPosition.X = MathF.Round(value.X - Width / 2);
                _localPosition.Y = MathF.Round(value.Y - Height);
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(x:(int)Position.X, y:(int)Position.Y, width:(int)Width, height:(int)Height);
            }
        }

        private static Dictionary<string, Texture2D> _rectangleTextures = new Dictionary<string, Texture2D>();
        private Texture2D _rectangleTexture;
        public Texture2D RectangleTexture
        {
            get
            {
                string sizeKey = string.Concat(Width, "x", Height);
                if (_rectangleTextures.ContainsKey(sizeKey))
                {
                    _rectangleTexture = _rectangleTextures[sizeKey];
                }
                else
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
                    _rectangleTextures.Add(sizeKey, _rectangleTexture);
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

        public bool CollideAt(List<IRigidbody> targets, Vector2 position)
        {
            //float dist = Vector2.Distance(Position, position);
            Vector2 relativePos = position - Position;
            float myDiagLengthSquared = (Width * Width) + (Height * Height);
            foreach(IRigidbody rigidbody in targets)
            {
                // I am stupid and forgot to put this in. No wonder my moving solids were preventing the actors from moving when being pushed!
                if (!rigidbody.Collider.Collidable) { continue; }

                float diagLengthSquared = (float)Math.Pow(rigidbody.Collider.Width, 2) + (float)Math.Pow(rigidbody.Collider.Height, 2);
                float dist = Vector2.DistanceSquared(position, rigidbody.Collider.Position);
                if (dist > myDiagLengthSquared + diagLengthSquared)
                    continue;

                if (this.Left + relativePos.X < rigidbody.Collider.Right
                    && this.Right + relativePos.X > rigidbody.Collider.Left
                    && this.Top + relativePos.Y < rigidbody.Collider.Bottom
                    && this.Bottom + relativePos.Y > rigidbody.Collider.Top)
                {
                    return true;
                }
            }

            return false;
        }

        public void RefreshPositionToTransform()
        {
            Position = _entity.transform.Position + _localPosition;
        }

        public override void Update(GameTime gameTime)
        {
            RefreshPositionToTransform();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Game.DebugMode)
                spriteBatch.Draw(RectangleTexture, Position, null, Color.Multiply(DebugColor, 1f), 0f, Vector2.Zero, 1, SpriteEffects.None, 1f);
        }
    }
}
