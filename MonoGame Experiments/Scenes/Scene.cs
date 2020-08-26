using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Scenes
{
    public abstract class Scene : IDisposable
    {
        public List<Entity> gameObjects = new List<Entity>();
        public List<IRigidbody> Actors = new List<IRigidbody>();
        public List<IRigidbody> Solids = new List<IRigidbody>();
        public List<Entity> EntitiesToDestroy = new List<Entity>();
        private bool disposedValue;

        public Scene()
        {
            Initialize();
        }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual void DebugDraw(SpriteBatch spriteBatch) 
        { 

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Scene()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
