using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Components;
using System;

namespace MonoGame_Experiments.Scenes
{
    class SceneMenu : Scene
    {

        private RenderTarget2D _tileRenderTarget;
        private Tilemap _currentTilemap;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_tileRenderTarget != null) spriteBatch.Draw(_tileRenderTarget, Vector2.Zero, Color.White);
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.Draw(spriteBatch);
        }

        public override void Initialize()
        {
            _tileRenderTarget?.Dispose();
            if (_tileRenderTarget == null) _tileRenderTarget = new RenderTarget2D(Game.Graphics.GraphicsDevice, 512, 512);
            if (_tileRenderTarget != null)
            {
                Game.ScreenManager.SpriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
                Game.ScreenManager.SpriteBatch.GraphicsDevice.SetRenderTargets(_tileRenderTarget);
                Game.ScreenManager.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Game.Graphics.GraphicsDevice.Clear(Color.Transparent);
                Tilemap tilemap = OgmoTilemapManager.LoadLevelData("Level1");
                if (tilemap != null)
                {
                    _currentTilemap = tilemap;
                    foreach (TilemapLayer layer in tilemap.layers)
                    {
                        if (layer.tileset != null)
                        {
                            foreach (Tile tile in layer.GetTiles())
                            {
                                Game.ScreenManager.SpriteBatch.Draw(layer.GetTexture2D(), tile.Position, tile.SpriteRectangle, Color.White);
                            }
                        }
                    }
                }
                Game.ScreenManager.SpriteBatch.End();
                Game.ScreenManager.SpriteBatch.GraphicsDevice.SetRenderTargets(null);
            }

            Entity object1 = new Entity(new Vector2(64, 128));
            Texture2D texture = ContentHandler.Instance.Load<Texture2D>("Sprites/SlimeCube");
            object1.AddComponent(new Player());
            object1.AddComponent(new Sprite(texture, texture.Width, texture.Height, Vector2.Zero, .5f));
            object1.AddComponent(new Collider(new Vector2(1, 3), 14, 13));

            gameObjects.Add(object1);

            Entity movingBlock = new Entity(new Vector2(64, 256 + 48));
            movingBlock.AddComponent(new MovingBlock());
            movingBlock.AddComponent(new Sprite(ContentHandler.Instance.Load<Texture2D>("Sprites/Pixel"), 24, 8, Vector2.Zero));
            movingBlock.AddComponent(new Collider(new Vector2(0, 0), 24, 8));
            gameObjects.Add(movingBlock);

            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                float randomX = random.Next(0, _currentTilemap.width - 16);
                float randomY = random.Next(0, _currentTilemap.height - 16);
                Entity flyingBubble = new Entity(new Vector2(randomX, randomY));

                float randomDirX = random.Next(2) == 0 ? -1 : 1;
                float randomDirY = random.Next(2) == 0 ? -1 : 1;
                flyingBubble.AddComponent(new FlyingBubble(new Vector2(randomDirX, randomDirY), 1f));
                flyingBubble.AddComponent(new Sprite(ContentHandler.Instance.Load<Texture2D>("Sprites/ZeldaBubble"), 16, 15, Vector2.Zero, spriteRectangle: new Rectangle(random.Next(0, 4) * 16, 0, 16, 15)));
                flyingBubble.AddComponent(new Collider(new Vector2(0, 0), 16, 15));
                gameObjects.Add(flyingBubble);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.PreUpdate(gameTime);
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.Update(gameTime);

            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.LateUpdate(gameTime);
            
            
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _tileRenderTarget.Dispose();
        }

        public override void DebugDraw(SpriteBatch spriteBatch)
        {
            base.DebugDraw(spriteBatch);

        }
    }
}
