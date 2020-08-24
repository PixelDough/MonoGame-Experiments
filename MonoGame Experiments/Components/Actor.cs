using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Components
{
    // The actor class itself has no concept of it's own speed, velocity, or gravity.
    // Classes that extend the Actor class keep track of those things.
    public class Actor : Component, IRigidbody
    {
        private Collider _collider;
        public Collider Collider {
            get
            {
                if (_collider != null) return _collider;
                else
                {
                    _collider = _entity.GetComponent<Collider>();
                    return _collider;
                }
            }
        }
        public static List<IRigidbody> Actors = new List<IRigidbody>();

        private float _velocityCap = 10f;

        public Actor()
        {
            Actors.Add(this);
        }

        public override void Initialize(Entity baseObject)
        {
            base.Initialize(baseObject);

            Collider.DebugColor = Color.LawnGreen;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public bool MoveX(float amount, Action onCollide)
        {
            amount = Math.Clamp(amount, -_velocityCap, _velocityCap);
            float xRemainder = amount;
            int move = (int)Math.Round(xRemainder);

            Collider.RefreshPositionToTransform();
            if (move != 0)
            {
                xRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    bool hitSolid = Collider.CollideAt(Solid.Solids, Collider.Position + (Vector2.UnitX * sign));

                    bool hitTilemap = false;
                    foreach(TilemapLayer layer in Tile.CurrentTilemap.layers)
                    {
                        if (layer.dataCoords == null) continue;
                        int left_tile = (int)MathF.Floor((Collider.Left + sign) / layer.gridCellWidth);
                        int right_tile = (int)MathF.Floor((Collider.Right + sign) / layer.gridCellWidth);
                        int top_tile = (int)MathF.Floor(Collider.Top / layer.gridCellHeight);
                        int bottom_tile = (int)MathF.Floor(Collider.Bottom / layer.gridCellHeight);

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
                                    hitTilemap = true;
                                }
                            }
                        }
                    }

                    if (!hitSolid && !hitTilemap)
                    {
                        // No solid immediately beside us
                        Transform.Position += Vector2.UnitX * sign;
                        Collider.RefreshPositionToTransform();
                        move -= sign;
                    }
                    else
                    {
                        // Hit a solid!
                        onCollide?.Invoke();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool MoveY(float amount, Action onCollide, int jumpCornerCorrectionAmount = 0)
        {
            amount = Math.Clamp(amount, -_velocityCap, _velocityCap);
            float yRemainder = amount;
            int move = (int)Math.Round(yRemainder);

            Collider.RefreshPositionToTransform();
            if (move != 0)
            {
                yRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    Collider.RefreshPositionToTransform();
                    bool hitSolid = Collider.CollideAt(Solid.Solids, Collider.Position + (Vector2.UnitY * sign));

                    bool hitTilemap = false;
                    foreach (TilemapLayer layer in Tile.CurrentTilemap.layers)
                    {
                        if (layer.dataCoords == null) continue;
                        int left_tile = (int)MathF.Floor(Collider.Left / layer.gridCellWidth);
                        int right_tile = (int)MathF.Floor(Collider.Right/ layer.gridCellWidth);
                        int top_tile = (int)MathF.Floor((Collider.Top + sign) / layer.gridCellHeight);
                        int bottom_tile = (int)MathF.Floor((Collider.Bottom + sign) / layer.gridCellHeight);

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
                                    hitTilemap = true;
                                }
                            }
                        }
                    }

                    if (!hitSolid && !hitTilemap)
                    {
                        // No solid immediately beside us
                        Transform.Position += Vector2.UnitY * sign;
                        Collider.RefreshPositionToTransform();
                        move -= sign;
                    }
                    else
                    {
                        // This could potentially be refactored. Right now it seems like an odd way of doing it. But it works :)
                        bool hitCornerJump = false;
                        if (jumpCornerCorrectionAmount != 0 && sign < 0)
                        {
                            hitCornerJump = JumpCornerCorrection(jumpCornerCorrectionAmount);
                        }
                        
                        if (!hitCornerJump)
                        {
                            // Hit a solid!
                            onCollide?.Invoke();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool JumpCornerCorrection(int jumpCornerCorrectionAmount)
        {
            for (int i = 1; i <= jumpCornerCorrectionAmount; i++)
            {
                for (int j = 1; j >= -1; j -= 2)
                {
                    bool hitSolid = Collider.CollideAt(Solid.Solids, Collider.Position + new Vector2(i * j, -1));

                    bool hitTilemap = false;
                    foreach (TilemapLayer layer in Tile.CurrentTilemap.layers)
                    {
                        if (layer.dataCoords == null) continue;
                        int left_tile = (int)MathF.Floor((Collider.Left + i * j) / layer.gridCellWidth);
                        int right_tile = (int)MathF.Floor((Collider.Right + i * j) / layer.gridCellWidth);
                        int top_tile = (int)MathF.Floor((Collider.Top - 1) / layer.gridCellHeight);
                        int bottom_tile = (int)MathF.Floor((Collider.Bottom - 1) / layer.gridCellHeight);

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
                                    hitTilemap = true;
                                }
                            }
                        }
                    }

                    if (!hitSolid && !hitTilemap)
                    {
                        Transform.Position += Vector2.UnitX * (i * j);
                        Collider.RefreshPositionToTransform();
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool IsRiding(Solid solid)
        {
            if (Collider.CollisionCheck(solid.Collider, new Vector2(0, 1)))
            {
                return true;
            }
            return false;
        }

        public virtual void Squish()
        {

        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Actors.Remove(this);
        }

    }
}
