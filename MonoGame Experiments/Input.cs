using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments
{
    class Input
    {
        public static KeyboardState keyboardState;
        public static KeyboardState keyboardStateLast;

        public static Action<Keys> KeyPressed;

        public static void Update(GameTime gameTime)
        {
            keyboardStateLast = keyboardState;
            keyboardState = Keyboard.GetState();

            if (keyboardState != keyboardStateLast)
            {
                foreach(var key in keyboardState.GetPressedKeys())
                {
                    if (!keyboardStateLast.IsKeyDown(key))
                    {
                        KeyPressed?.Invoke(key);
                    }
                }
            }
        }

        public static bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return keyboardState.IsKeyUp(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && keyboardStateLast.IsKeyUp(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && keyboardStateLast.IsKeyDown(key);
        }
    }
}
