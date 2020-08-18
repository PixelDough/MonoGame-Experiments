using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Interfaces
{
    public interface IRigidbody
    {
        public Collider Collider { get; }
        public void MoveX(float amount, Action onCollide);
        public void MoveY(float amount, Action onCollide);
    }
}
