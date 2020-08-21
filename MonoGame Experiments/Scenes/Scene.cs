using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Scenes
{
    public abstract class Scene
    {
        public List<Entity> gameObjects = new List<Entity>();
        public List<Entity> EntitiesToDestroy = new List<Entity>();

        public Scene()
        {
            Initialize();
        }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
