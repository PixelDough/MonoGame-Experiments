using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Components;
using System;

namespace MonoGame_Experiments.Scenes
{
    class SceneMenu : Scene
    {

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Tile.CurrentTilemapRenderTarget2D, Vector2.Zero, Color.White);
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.Draw(spriteBatch);
        }

        public override void Initialize()
        {
            Tile.CurrentTilemapRenderTarget2D = new RenderTarget2D(Game.Graphics.GraphicsDevice, 512, 512);
            Game.ScreenManager.SpriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            Game.ScreenManager.SpriteBatch.GraphicsDevice.SetRenderTarget(Tile.CurrentTilemapRenderTarget2D);
            Game.ScreenManager.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Tilemap tilemap = OgmoTilemapManager.LoadLevelData("Level1");
            if (tilemap != null)
            {
                Tile.CurrentTilemap = tilemap;
                foreach (TilemapLayer layer in tilemap.layers)
                {
                    if (layer.tileset != null)
                    {
                        foreach (Tile tile in layer.GetTiles())
                        {
                            //Entity tileObject = new Entity(tile.Position);
                            //tileObject.AddComponent(tile);
                            //Collider collider = new Collider(Vector2.Zero, tile.SpriteRectangle.Width, tile.SpriteRectangle.Height);
                            //tileObject.AddComponent(collider);

                            //gameObjects.Add(tileObject);
                            
                            //tile.DrawTile(Game.ScreenManager.SpriteBatch);
                            Game.ScreenManager.SpriteBatch.Draw(layer.GetTexture2D(), tile.Position, tile.SpriteRectangle, Color.White);
                        }
                    }
                }
            }
            Game.ScreenManager.SpriteBatch.End();
            Game.ScreenManager.SpriteBatch.GraphicsDevice.SetRenderTarget(null);

            Entity object1 = new Entity(new Vector2(64, 128));
            Texture2D texture = ContentHandler.Instance.Load<Texture2D>("Sprites/SlimeCube");
            object1.AddComponent(new Player());
            object1.AddComponent(new Sprite(texture, texture.Width, texture.Height, Vector2.Zero, .5f));
            object1.AddComponent(new Collider(new Vector2(1, 3), 14, 13));

            gameObjects.Add(object1);

            Entity movingBlock = new Entity(new Vector2(64, 256 + 48));
            //movingBlock.AddComponent(new MovingBlock());
            movingBlock.AddComponent(new Solid());
            movingBlock.AddComponent(new Sprite(ContentHandler.Instance.Load<Texture2D>("Sprites/Pixel"), 24, 8, Vector2.Zero));
            movingBlock.AddComponent(new Collider(new Vector2(0, 0), 24, 8));
            gameObjects.Add(movingBlock);

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                //float randomX = random.Next(-4, 5);
                //float randomY = random.Next(-4, 5);
                //Entity flyingBubble = new Entity(new Vector2(64 + randomX, 80 + randomY));

                float randomX = random.Next(0, 512 - 32);
                float randomY = random.Next(0, 512 - 32);
                Entity flyingBubble = new Entity(new Vector2(randomX, randomY));

                float randomDirX = random.Next(2) == 0 ? -1 : 1;
                float randomDirY = random.Next(2) == 0 ? -1 : 1;
                float randomSpeed = random.Next(1, 3);
                flyingBubble.AddComponent(new FlyingBubble(new Vector2(randomDirX, randomDirY), 1));
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
    }
}
