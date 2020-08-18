using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Solid : Component, IRigidbody
    {

        public Collider Collider { get { return _baseObject.GetComponent<Collider>(); } }
        public static List<IRigidbody> Solids = new List<IRigidbody>();

        public Solid()
        {
            Solids.Add(this);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public void MoveX(float amount, Action onCollide)
        {
            
        }

        public void MoveY(float amount, Action onCollide)
        {
            
        }

    }
}
