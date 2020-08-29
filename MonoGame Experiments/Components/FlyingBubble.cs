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

        public override void Awake()
        {
            base.Awake();

            if (Collider.CollideAt(Game._currentScene.Solids, Collider.Position))
            {
                Squish();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            Vector2 pos = Collider.Position;
            MoveY(_direction.Y * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds * 60, HitY);
            Collider.Update(gameTime);
            MoveX(_direction.X * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds * 60, HitX);
            Collider.Update(gameTime);
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
