using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Experiments.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;

namespace MonoGame_Experiments
{
    public static class DebugConsole
    {
        private static string _currentString = "";
        private static List<string> _previousStrings = new List<string>();

        private static SpriteFont spriteFont = ContentHandler.Instance.Load<SpriteFont>("Fonts/system");

        public static List<Command> commands = new List<Command>()
        {
            new Command("bobby", Bobby),
            new Command("load", LoadLevel),
            new Command("cls", CLS),
            new Command("reload", delegate(string[] parameters) { Game.ChangeScenes(Game._currentScene); }),
            new Command("colliders", delegate(string[] parameters) { DebugManager.ShowCollisionRectangles = !DebugManager.ShowCollisionRectangles; }),
        };

        public static void TextInputHandler(object sender, TextInputEventArgs args)
        {
            if (!Game.DebugMode) return;
            var pressedKey = args.Key;

            if (pressedKey == Keys.Back)
            {
                _currentString = _currentString.Substring(0, (int)MathF.Max(_currentString.Length - 1, 0));
                return;
            }
            if (pressedKey == Keys.Tab)
            {
                if (_previousStrings.Count > 0)
                    _currentString = _previousStrings[_previousStrings.Count - 1];
                return;
            }
            if (pressedKey == Keys.Enter)
            {
                RunCommand(_currentString);
                return;
            }

            var character = args.Character;
            if (char.IsControl(character))
                return;
            _currentString += character;
        }

        public static void RunCommand(string commandString)
        {
            _previousStrings.Add(_currentString);

            try
            {
                Queue<string> inputs = new Queue<string>(_currentString.Split(" ", StringSplitOptions.RemoveEmptyEntries));
                string commandName = inputs.Dequeue();
                string[] parameters = inputs.ToArray();

                Command command = commands.Find(c => c.Name == commandName);
                if (command.Action == null) throw new Exception("ERROR: Command \"" + commandName + "\" not found!");
                command.Action?.Invoke(parameters);
            }
            catch(Exception e)
            {
                _previousStrings.Add(e.Message);
            }

            while (_previousStrings.Count > 10)
                _previousStrings.RemoveAt(0);

            _currentString = "";
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (!Game.DebugMode) return;
            for (int i = 0; i < _previousStrings.Count; i++)
            {
                string s = _previousStrings[i];
                spriteBatch.DrawString(spriteFont, s, new Vector2(4, Game.Graphics.PreferredBackBufferHeight - spriteFont.LineSpacing - 32 - ((_previousStrings.Count - 1 - i) * 24)), Color.Gray);
            }

            spriteBatch.DrawString(spriteFont, "> " + _currentString, new Vector2(4, Game.Graphics.PreferredBackBufferHeight - spriteFont.LineSpacing), Color.White);
            spriteBatch.DrawString(spriteFont, "|", new Vector2(4 + spriteFont.MeasureString("> " + _currentString).X, Game.Graphics.PreferredBackBufferHeight - spriteFont.LineSpacing), Color.White);
        }

        public struct Command
        {
            public string Name;
            public Action<string[]?> Action;
            
            public Command(string name, Action<string[]> action)
            {
                Name = name;
                Action = action;
            }
        }

        public static void Bobby(params string[] paramList)
        {
            _previousStrings.Add("jones nevachange lezgomin: " + paramList[0]);
            Game.ChangeScenes(new SceneMenu());
        }

        public static void LoadLevel(params string[] paramList)
        {
            Type sceneType = Type.GetType("MonoGame_Experiments.Scenes." + paramList[0]);
            if (sceneType == null) throw new Exception("ERROR: Scene with name \"" + paramList[0] + "\" does not exist!");
            Scene scene = (Scene)Activator.CreateInstance(sceneType);
            Game.ChangeScenes(scene);
        }

        public static void CLS(params string[] paramList)
        {
            _previousStrings.Clear();
        }
    }
}
