using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGame_Experiments
{
    class Input
    {
        public static KeyboardState keyboardState;
        public static KeyboardState keyboardStateLast;

        public static GamePadState gamePadState;
        public static GamePadState gamePadStateLast;

        public static Action<Keys> KeyPressed;

        public static void Update(GameTime gameTime)
        {
            keyboardStateLast = keyboardState;
            keyboardState = Keyboard.GetState();

            gamePadStateLast = gamePadState;
            gamePadState = GamePad.GetState(PlayerIndex.One);

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

        public static bool IsInputDown(List<Keys> keys = null, List<Buttons> buttons = null)
        {
            foreach (Keys key in keys ?? Enumerable.Empty<Keys>())
            {
                if (keyboardState.IsKeyDown(key))
                    return true;
            }
            foreach (Buttons button in buttons ?? Enumerable.Empty<Buttons>())
            {
                if (gamePadState.IsButtonDown(button))
                    return true;
            }

            return false;
        }

        public static bool IsKeyUp(Keys key)
        {
            return keyboardState.IsKeyUp(key);
        }

        public static bool IsInputUp(List<Keys> keys = null, List<Buttons> buttons = null)
        {
            foreach (Keys key in keys ?? Enumerable.Empty<Keys>())
            {
                if (keyboardState.IsKeyUp(key))
                    return true;
            }
            foreach (Buttons button in buttons ?? Enumerable.Empty<Buttons>())
            {
                if (gamePadState.IsButtonUp(button))
                    return true;
            }

            return false;
        }

        public static bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && keyboardStateLast.IsKeyUp(key);
        }

        public static bool IsInputPressed(List<Keys> keys = null, List<Buttons> buttons = null)
        {
            foreach (Keys key in keys ?? Enumerable.Empty<Keys>())
            {
                if (keyboardState.IsKeyDown(key) && keyboardStateLast.IsKeyUp(key))
                    return true;
            }
            foreach (Buttons button in buttons ?? Enumerable.Empty<Buttons>())
            {
                if (gamePadState.IsButtonDown(button) && gamePadStateLast.IsButtonUp(button))
                    return true;
            }

            return false;
        }

        public static bool IsKeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && keyboardStateLast.IsKeyDown(key);
        }

        public static bool IsInputReleased(List<Keys> keys = null, List<Buttons> buttons = null)
        {
            foreach (Keys key in keys ?? Enumerable.Empty<Keys>())
            {
                if (keyboardState.IsKeyUp(key) && keyboardStateLast.IsKeyDown(key))
                    return true;
            }
            foreach (Buttons button in buttons ?? Enumerable.Empty<Buttons>())
            {
                if (gamePadState.IsButtonUp(button) && gamePadStateLast.IsButtonDown(button))
                    return true;
            }

            return false;
        }
    }
}
