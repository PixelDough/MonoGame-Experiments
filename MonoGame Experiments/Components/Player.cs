using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Player : Actor
    {
        private Vector2 _velocity = Vector2.Zero;
        private Vector2 _positionCheck = Vector2.Zero;
        private Vector2 _positionLast = Vector2.Zero;

        public Player() : base()
        {

        }

        public override void Initialize(Entity baseObject)
        {
            base.Initialize(baseObject);

            Transform.OnMoveEvent += UpdateFollowCameraPosition;

            UpdateFollowCameraPosition();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            _positionLast = _positionCheck;
            _positionCheck = Transform.Position;

            float moveAmountX = 0;
            if (Input.IsInputDown(new List<Keys> { Keys.Left, Keys.A }, new List<Buttons> { Buttons.DPadLeft, Buttons.LeftThumbstickLeft }))
                moveAmountX -= 2;
            if (Input.IsInputDown(new List<Keys> { Keys.Right, Keys.D }, new List<Buttons> { Buttons.DPadRight, Buttons.LeftThumbstickRight }))
                moveAmountX += 2;

            _velocity.X = MathHelper.Lerp(_velocity.X, moveAmountX, 0.2f);

            _velocity.Y += 0.2f;

            if (Input.IsInputPressed(new List<Keys> { Keys.Space }, new List<Buttons> { Buttons.A } ))
                _velocity.Y = -4 + ((_positionCheck - _positionLast).Y / 1.5f);



            Transform.Rotation += MathHelper.ToRadians(1);

            MoveY(_velocity.Y, OnCollideY);
            Collider.Update(gameTime);
            MoveX(_velocity.X, OnCollideX);
            Collider.Update(gameTime);

            //Collider.BottomCenter = Vector2.Zero;

            // TODO: Fix case where there is no x or y axis collision, but movement goes into a solid.
            //_baseObject.transform.Position += moveAmount;

        }

        public void UpdateFollowCameraPosition()
        {
            Game.Camera.Position = _entity.transform.Position - new Vector2(Game.Camera.Viewport.Width / 2, Game.Camera.Viewport.Height / 2);
        }

        public override void Squish()
        {
            base.Squish();

            Entity.Destroy(_entity);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void OnCollideX()
        {
            _velocity.X = 0;
        }
        private void OnCollideY()
        {
            _velocity.Y = 0;
        }

    }
}
