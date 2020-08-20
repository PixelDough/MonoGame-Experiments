using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Experiments.Components;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace MonoGame_Experiments.Scenes
{
    class SceneMenu : Scene
    {

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.Draw(spriteBatch);
        }

        public override void Initialize()
        {
            Tilemap tilemap = OgmoTilemapManager.LoadLevelData("Ogmo Project/Level1.json");

            foreach (TilemapLayer layer in tilemap.layers)
            {
                if (layer.tileset != null)
                {
                    foreach (Tile tile in layer.GetTiles())
                    {
                        Entity tileObject = new Entity(tile.Position);
                        tileObject.AddComponent(tile);
                        Collider collider = new Collider(Vector2.Zero, tile.SpriteRectangle.Width, tile.SpriteRectangle.Height);
                        tileObject.AddComponent(collider);

                        gameObjects.Add(tileObject);
                    }
                }
            }

            Entity object1 = new Entity(new Vector2(64, 128));
            Texture2D texture = ContentHandler.Instance.Load<Texture2D>("Sprites/Toast_die");
            object1.AddComponent(new Player());
            object1.AddComponent(new Sprite(texture, texture.Width, texture.Height, Vector2.Zero));
            object1.AddComponent(new Collider(new Vector2(2, 3), 12, 13));

            gameObjects.Add(object1);

            Entity movingBlock = new Entity(new Vector2(64, 256 + 48));
            movingBlock.AddComponent(new MovingBlock());
            movingBlock.AddComponent(new Sprite(ContentHandler.Instance.Load<Texture2D>("Sprites/Pixel"), 24, 8, Vector2.Zero));
            movingBlock.AddComponent(new Collider(new Vector2(0, 0), 24, 8));
            gameObjects.Add(movingBlock);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.PreUpdate(gameTime);
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.Update(gameTime);
            
            
        }
    }
}
