﻿using Microsoft.Xna.Framework;
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
        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.PreUpdate(gameTime);

            if (Game.DebugMode) return;
            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.Update(gameTime);

            foreach (Entity gameObject in gameObjects.ToArray())
                gameObject.LateUpdate(gameTime);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity gameObject in gameObjects)
                gameObject.Draw(spriteBatch);
        }
        public virtual void DebugDraw(SpriteBatch spriteBatch)
        {

        }

        public virtual Entity AddEntity(Vector2 position)
        {
            Entity entity = new Entity(position);
            gameObjects.Add(entity);
            return entity;
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
                foreach(Entity entity in gameObjects)
                {
                    entity.Dispose();
                }
                gameObjects.Clear();
                EntitiesToDestroy.Clear();

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
