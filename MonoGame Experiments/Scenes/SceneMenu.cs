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
            base.Draw(spriteBatch);
        }

        public override void Initialize()
        {
            Entity tilemapEntity = new Entity(Vector2.Zero);
            Tilemap tilemap = new Tilemap(tilemapEntity, "Level1");
            tilemapEntity.AddComponent(tilemap);
            gameObjects.Add(tilemapEntity);

            Entity object1 = new Entity(new Vector2(64, 128));
            object1.AddComponent(new Player(object1));
            
            //object1.AddComponent(new Collider(new Vector2(1, 3), 14, 13));
            gameObjects.Add(object1);

            //Entity movingBlock = new Entity(new Vector2(64, 256 + 96));
            //movingBlock.AddComponent(new MovingBlock());
            //movingBlock.AddComponent(new Sprite(ContentHandler.Instance.Load<Texture2D>("Sprites/Pixel"), 24, 8, Vector2.Zero));
            //movingBlock.AddComponent(new Collider(new Vector2(0, 0), 24, 8));
            //gameObjects.Add(movingBlock);

            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                float randomX = random.Next(0, 128);
                float randomY = random.Next(0, 128);
                Entity flyingBubble = new Entity(new Vector2(randomX, randomY));

                float randomDirX = random.Next(2) == 0 ? -1 : 1;
                float randomDirY = random.Next(2) == 0 ? -1 : 1;
                flyingBubble.AddComponent(new FlyingBubble(flyingBubble, new Vector2(randomDirX, randomDirY), 1f));
                gameObjects.Add(flyingBubble);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

        }

        public override void DebugDraw(SpriteBatch spriteBatch)
        {
            base.DebugDraw(spriteBatch);

        }
    }
}
