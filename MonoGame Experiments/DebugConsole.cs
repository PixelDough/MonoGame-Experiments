using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Experiments.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonoGame_Experiments
{
    public static class DebugConsole
    {
        private static string _currentString = "";
        private static List<string> _previousStrings = new List<string>();
        private static int _previousStringsMax = 20;

        private static SpriteFont spriteFont = ContentHandler.Instance.Load<SpriteFont>("Fonts/system");

        public static List<Command> Commands = new List<Command>()
        {
            new Command("help", Help),
            new Command("load", LoadLevel),
            new Command("cls", (x) => _previousStrings.Clear()),
            new Command("reload", (x) => Game.ChangeScenes(Game._currentScene)),
            new Command("colliders", (x) => DebugManager.ShowCollisionRectangles = !DebugManager.ShowCollisionRectangles),
            new Command("rick", (x) => OpenLink("https://www.youtube.com/watch?v=dQw4w9WgXcQ")),
            new Command("aha", (x) => OpenLink("https://youtu.be/djV11Xbc914?t=3")),
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

                Command command = Commands.Find(c => c.Name == commandName);
                if (command.Action == null) throw new Exception("ERROR: Command \"" + commandName + "\" not found!");

                if (parameters.Contains("-c"))
                    Game.DebugMode = false;
                command.Action?.Invoke(parameters);
            }
            catch(Exception e)
            {
                _previousStrings.Add(e.Message);
            }

            while (_previousStrings.Count > _previousStringsMax)
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


        public delegate void CommandAction(params string[] args);
        public struct Command
        {
            public string Name;
            public CommandAction Action;
            
            public Command(string name, CommandAction action)
            {
                Name = name;
                Action = action;
            }
        }

        public static void Help(params string[] paramList)
        {
            Commands = Commands.OrderBy(x => x.Name).ToList();
            if (paramList.Length == 0)
            {
                _previousStrings.Add("-----HELP-----");
                foreach (var command in Commands)
                {
                    _previousStrings.Add(command.Name);
                }
            }
        }

        public static void LoadLevel(params string[] paramList)
        {
            Type sceneType = Type.GetType("MonoGame_Experiments.Scenes." + paramList[0]);
            if (sceneType == null) throw new Exception("ERROR: Scene with name \"" + paramList[0] + "\" does not exist!");
            Scene scene = (Scene)Activator.CreateInstance(sceneType);
            Game.ChangeScenes(scene);
        }

        public static void OpenLink(string link)
        {
            var ps = new ProcessStartInfo(link)
            {
                UseShellExecute = true
            };
            Process.Start(ps);
        }
    }
}
