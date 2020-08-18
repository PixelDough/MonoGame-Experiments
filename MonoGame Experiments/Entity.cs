using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments
{
    public class Entity
    {
        public int ID { get; set; }

        public Transform transform = new Transform(Vector2.Zero, 0f, Vector2.One);

        private List<Entity> _children;
        private readonly List<Component> _components;
        

        public Entity(Vector2 position)
        {
            _children = new List<Entity>();
            _components = new List<Component>();
            transform.Position = position;
        }

        public void AddChild(Entity child)
        {
            _children.Add(child);
        }

        public Component AddComponent(Component component)
        {
            _components.Add(component);
            component.Initialize(this);

            return component;
        }

        public void AddComponent(List<Component> components)
        {
            _components.AddRange(components);
            foreach (var component in components)
            {
                component.Initialize(this);
            }
        }

        public TComponentType GetComponent<TComponentType>() where TComponentType : Component
        {
            return _components.Find(c => c is TComponentType) as TComponentType;
        }

        public void RemoveComponent(Component component)
        {
            _components.Remove(component);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
            foreach (Entity child in _children)
            {
                child.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var component in _components)
            {
                component.Draw(spriteBatch);
            }
            foreach (Entity child in _children)
            {
                child.Draw(spriteBatch);
            }
        }

        public class Transform
        {
            private Vector2 _position;
            public Vector2 Position
            {
                get { return Vector2.Round(_position); }
                set { _position = Vector2.Round(value); }
            }

            public float Rotation = 0f;
            public Vector2 Scale = Vector2.One;

            public Transform(Vector2 position, float rotation, Vector2 scale)
            {
                Position = position;
                Rotation = rotation;
                Scale = scale;
            }
        }

    }
}
