using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Scenes
{
    class SceneMenu : Scene
    {
        private GameObject _object1 = new GameObject();
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(spriteBatch);

            _object1.Draw(spriteBatch);
        }

        public override void Initialize()
        {
            Texture2D docTexture = ContentHandler.Instance.Load<Texture2D>("Sprites/Doc");
            _object1.AddComponent(new Sprite(docTexture, docTexture.Width, docTexture.Height, Vector2.Zero));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Update(gameTime);

            _object1.Update(gameTime);
        }
    }
}
