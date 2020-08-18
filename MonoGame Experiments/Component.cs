using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments
{
    public abstract class Component
    {
        protected Entity _baseObject;

        public void Initialize(Entity baseObject)
        {
            _baseObject = baseObject;
        }

        public int GetOwnerId()
        {
            return _baseObject.ID;
        }

        public void RemoveMe()
        {
            _baseObject.RemoveComponent(this);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
