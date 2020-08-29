using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class MovingBlock : Solid
    {
        private float _timePosition = 0;
        private Vector2 _startPosition = Vector2.Zero;

        public MovingBlock() : base(SolidTypes.Block)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Awake()
        {
            base.Awake();

            _startPosition = Transform.Position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector2 pos = _startPosition + new Vector2(0, MathF.Sin(_timePosition * 5) * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds * 60);

            _timePosition += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Move(0, pos.Y - Transform.Position.Y);
        }
    }
}
