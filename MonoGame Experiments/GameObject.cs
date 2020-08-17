using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments
{
    public class GameObject
    {
        public int ID { get; set; }
        //public Transform _transform
        //{
        //    get; protected set;
        //}

        private List<GameObject> _children;
        private readonly List<Component> _components;

        public GameObject()
        {
            //_transform = new Transform();
            _children = new List<GameObject>();
            _components = new List<Component>();
        }

        public void AddChild(GameObject child)
        {
            _children.Add(child);
        }

        public void AddComponent(Component component)
        {
            _components.Add(component);
            component.Initialize(this);
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

        public void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
            foreach (GameObject child in _children)
            {
                child.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var component in _components)
            {
                component.Draw(spriteBatch);
            }
            foreach (GameObject child in _children)
            {
                child.Draw(spriteBatch);
            }
        }

    }
}
