using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class FlyingBubble : Actor
    {
        private Vector2 _direction;
        private float _speed;

        private Vector2 positionOld = Vector2.Zero;

        public FlyingBubble(Vector2 direction, float speed) : base()
        {
            _direction = direction;
            _speed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            Vector2 pos = Collider.Position;
            MoveY(_direction.Y * _speed, HitY);
            Collider.Update(gameTime);
            MoveX(_direction.X * _speed, HitX);
            Collider.Update(gameTime);

            if (pos == Collider.Position)
            {
                if (positionOld != pos)
                    positionOld = pos;
                else
                    Squish();
            }
        }

        private void HitX()
        {
            _direction.X = -_direction.X;
        }

        private void HitY()
        {
            _direction.Y = -_direction.Y;
        }

        public override void Squish()
        {
            base.Squish();
            Entity.Destroy(_entity);
        }
    }
}
