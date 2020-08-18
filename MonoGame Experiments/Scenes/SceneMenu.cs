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
            foreach (Entity gameObject in gameObjects)
                gameObject.Draw(spriteBatch);
        }

        public override void Initialize()
        {
            Entity object1 = new Entity(new Vector2(32, 32));
            Texture2D texture = ContentHandler.Instance.Load<Texture2D>("Sprites/Toast_die");
            object1.AddComponent(new Player());
            object1.AddComponent(new Sprite(texture, texture.Width, texture.Height, Vector2.Zero));
            object1.AddComponent(new Collider(Vector2.Zero, texture.Width, texture.Height));
            gameObjects.Add(object1);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity gameObject in gameObjects)
                gameObject.Update(gameTime);
            
            
        }
    }
}
