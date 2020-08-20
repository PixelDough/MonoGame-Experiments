using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Interfaces
{
    public interface IRigidbody
    {
        public Collider Collider { get; }
        public bool MoveX(float amount, Action onCollide);
        public bool MoveY(float amount, Action onCollide);
    }
}
