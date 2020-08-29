using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments
{
    public abstract class Component : IDisposable
    {
        protected Entity _entity;
        public Entity Entity { get { return _entity; } }
        private bool disposedValue;

        public Component(Entity entity)
        {
            _entity = entity;
        }

        public Entity.Transform Transform
        {
            get { return _entity.transform; }
        }

        public virtual void Initialize(Entity baseObject)
        {
            _entity = baseObject;
        }

        public virtual void Awake()
        {

        }

        public int GetOwnerId()
        {
            return _entity.ID;
        }

        public void RemoveMe()
        {
            _entity.RemoveComponent(this);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        // TODO: Write a UI manager class, and implement a DrawUI method here.
        // Alternatively, simply call a method in the DrawUI class which would add a draw call to a 
        // list/delegate(?) and call each one in a batch.

        public virtual void OnDestroy() { }

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
        // ~Component()
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
