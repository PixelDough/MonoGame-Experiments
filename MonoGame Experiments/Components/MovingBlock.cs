using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class MovingBlock : Solid
    {
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
            Move(0f, MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds * 5) * 5);
        }
    }
}
