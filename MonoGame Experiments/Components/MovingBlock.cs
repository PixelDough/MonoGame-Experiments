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
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Initialize(Entity baseObject)
        {
            base.Initialize(baseObject);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _timePosition += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Move(0f, MathF.Sin(_timePosition * 5) * 5);
        }
    }
}
