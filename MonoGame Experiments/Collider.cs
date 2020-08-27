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

        public Color DebugColor = Color.Red;
        public int Width { get; set; }
        public int Height { get; set; }
        public CollisionResult CollisionResults;

        public Collider(Vector2 localPosition, int width, int height)
        {
            Width = width;
            Height = height;
            _localPosition = localPosition;

        }

        public int Top
        {
            get { return (int)Position.Y; }
            set { _position.Y = value; }
        }

        public int Bottom
        {
            get { return (int)Position.Y + Height - 1; }
            set { _position.Y = value - Height - 1; }
        }

        public int Left
        {
            get { return (int)Position.X; }
            set { _position.X = value; }
        }

        public int Right
        {
            get { return (int)Position.X + Width - 1; }
            set { _position.X = value - Width - 1; }
        }

        public Vector2 Center
        {
            get { return new Vector2(Position.X + Width / 2, Position.Y + Height / 2); }
            set { _position = new Vector2(value.X - Width / 2, value.Y - Height / 2); }
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

            if (this.Left + _localOffset.X <= other.Right
                && this.Right + _localOffset.X >= other.Left
                && this.Top + _localOffset.Y <= other.Bottom
                && this.Bottom + _localOffset.Y >= other.Top)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CollideAt(List<IRigidbody> targets, Vector2 position, bool includeTilemap = false, OgmoTilemap tilemap = null)
        {
            //float dist = Vector2.Distance(Position, position);
            Vector2 relativePos = position - Position;
            foreach(IRigidbody rigidbody in targets)
            {
                if (rigidbody is Tilemap)
                {
                    foreach (TilemapLayer layer in ((Tilemap)rigidbody).TilemapData.layers)
                    {
                        if (layer.dataCoords == null) continue;
                        int left_tile = (int)MathF.Floor((Left + relativePos.X) / layer.gridCellWidth);
                        int right_tile = (int)MathF.Floor((Right + relativePos.X) / layer.gridCellWidth);
                        int top_tile = (int)MathF.Floor((Top + relativePos.Y) / layer.gridCellHeight);
                        int bottom_tile = (int)MathF.Floor((Bottom + relativePos.Y) / layer.gridCellHeight);

                        if (left_tile < 0) left_tile = 0;
                        if (right_tile > layer.gridCellsX) right_tile = layer.gridCellsX;
                        if (top_tile < 0) top_tile = 0;
                        if (bottom_tile > layer.gridCellsY) bottom_tile = layer.gridCellsY;

                        for (int xx = left_tile; xx <= right_tile; xx++)
                        {
                            for (int yy = top_tile; yy <= bottom_tile; yy++)
                            {
                                int[] tileData = layer.dataCoords[xx + (yy * layer.gridCellsX)];
                                if (tileData[0] != -1)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    continue;
                }

                // I am stupid and forgot to put this in. No wonder my moving solids were preventing the actors from moving when being pushed!
                if (!rigidbody.Collider.Collidable) { continue; }

                if (this.Left + relativePos.X <= rigidbody.Collider.Right
                    && this.Right + relativePos.X >= rigidbody.Collider.Left
                    && this.Top + relativePos.Y <= rigidbody.Collider.Bottom
                    && this.Bottom + relativePos.Y >= rigidbody.Collider.Top)
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
                spriteBatch.Draw(RectangleTexture, Position, null, DebugColor, 0f, Vector2.Zero, 1, SpriteEffects.None, 1f);
        }

        public struct CollisionResult
        {
            public Side Sides;
            [Flags]
            public enum Side
            {
                None = 0,
                Left = 1,
                Right = 2,
                Up = 4,
                Down = 8
            }

            public bool HasSide(Side side)
            {
                return Sides.HasFlag(side);
            }

            public bool DoesNotHaveSide(Side side)
            {
                return !Sides.HasFlag(side);
            }

            public void SetSide(Side side)
            {
                Sides |= side;
            }

            public void RemoveSide(Side side)
            {
                Sides &= ~side;
            }

            public void ClearSides()
            {
                Sides = Side.None;
            }

        }
    }
}
