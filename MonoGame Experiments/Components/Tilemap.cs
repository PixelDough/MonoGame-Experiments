using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Components
{
    class Tilemap : Solid
    {
        public RenderTarget2D RenderTarget2D;
        public OgmoTilemap TilemapData;
        public Tilemap(Entity entity, string tilemapName) : base(entity, SolidTypes.Tilemap)
        {
            using (OgmoTilemap tilemap = OgmoTilemapManager.LoadLevelData(tilemapName))
            {
                RenderTarget2D = new RenderTarget2D(Game.Graphics.GraphicsDevice, tilemap.width, tilemap.height);
                TilemapData = tilemap;
                UpdateRenderTarget();
            }
        }

        public void UpdateRenderTarget()
        {
            using (SpriteBatch spriteBatch = new SpriteBatch(Game.Graphics.GraphicsDevice))
            {
                spriteBatch.GraphicsDevice.SetRenderTargets(RenderTarget2D);
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                spriteBatch.GraphicsDevice.Clear(Color.Transparent);
                foreach (TilemapLayer layer in TilemapData.layers)
                {
                    if (layer.tileset != null)
                    {
                        foreach (Tile tile in layer.GetTiles())
                        {
                            spriteBatch.Draw(layer.GetTexture2D(), tile.Position, tile.SpriteRectangle, Color.White);
                        }
                    }
                }
                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTargets(null);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(RenderTarget2D, Vector2.Zero, Color.White);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            RenderTarget2D?.Dispose();
            RenderTarget2D = null;

            TilemapData?.Dispose();
            TilemapData = null;
        }
    }
}
