using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments
{
    public abstract class Component
    {
        protected Entity _entity;
        public Entity.Transform Transform
        {
            get { return _entity.transform; }
        }

        public virtual void Initialize(Entity baseObject)
        {
            _entity = baseObject;
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
    }
}
