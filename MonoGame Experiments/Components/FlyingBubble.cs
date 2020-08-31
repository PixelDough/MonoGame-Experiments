using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public FlyingBubble(Entity entity) : this(entity, Vector2.One, 1) { }
        public FlyingBubble(Entity entity, Vector2 direction, float speed) : base(entity)
        {
            _direction = direction;
            _speed = speed;

            Collider.Height = 15;

            Texture2D bubbleTexture = ContentHandler.Instance.Load<Texture2D>("Sprites/ZeldaBubble");
            entity.AddComponent(new Sprite(entity, bubbleTexture, 16, 15, Vector2.Zero, spriteRectangle: new Rectangle(0 * 16, 0, 16, 15), depth: 0.5f));
        }

        public override void Awake()
        {
            base.Awake();

            //_entity.AddComponent(new Collider(new Vector2(0, 0), 16, 15));

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
