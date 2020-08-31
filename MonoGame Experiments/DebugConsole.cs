using Microsoft.VisualBasic;
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

        private static int _previousStringCheckout = 0;
        private static int _currentCharacterSlot = 0;

        private static Texture2D _pixelTexture = ContentHandler.Instance.Load<Texture2D>("Sprites/Pixel");

        private static SpriteFont _spriteFont = ContentHandler.Instance.Load<SpriteFont>("Fonts/monogram24");

        private static float _time = 0;

        // TODO: Make Command an interface, and create a file with all the commands that inherit from Command, where each one has a method called "DoAction" or something.
        public static List<Command> Commands = new List<Command>()
        {
            new Command(
                "help",
                "help [COMMAND NAME]",
                Help),
            new Command(
                "print",
                "print STRING",
                (x) => _previousStrings.Add(string.Join(" ", x))),
            new Command(
                "load",
                "load STRING",
                LoadLevel),
            new Command(
                "unload",
                "unload",
                Unload),
            new Command(
                "cls",
                "cls",
                (x) => _previousStrings.Clear()),
            new Command(
                "reload",
                "reload",
                (x) => Game.ChangeScenes(Game._currentScene)),
            new Command(
                "colliders",
                "colliders",
                (x) => DebugManager.ShowCollisionRectangles = !DebugManager.ShowCollisionRectangles),
            new Command(
                "spawn",
                "spawn STRING INT INT",
                (x) => Spawn(x[0], int.Parse(x[1]), int.Parse(x[2]))),
            new Command(
                "PixelDough",
                "PixelDough",
                (x) => { }),
            new Command(
                "rick",
                "rick",
                (x) => OpenLink("https://www.youtube.com/watch?v=dQw4w9WgXcQ")),
            new Command(
                "aha",
                "aha",
                (x) => OpenLink("https://youtu.be/djV11Xbc914?t=3")),
        };

        public static void TextInputHandler(object sender, TextInputEventArgs args)
        {
            if (!Game.DebugMode) return;
            var pressedKey = args.Key;

            if (pressedKey == Keys.Tab)
            {
                if (_previousStrings.Count > 0)
                {
                    _previousStringCheckout++;
                    _previousStringCheckout = (int)MathF.Min(_previousStringCheckout, _previousStrings.Count);
                    _currentString = _previousStrings[_previousStrings.Count - _previousStringCheckout];
                    SetCurrentCharacterSlotToEnd();
                }
                return;
            }
            _previousStringCheckout = 0;

            if (pressedKey == Keys.Back)
            {
                if (_currentString.Length < 1) return;
                if (_currentCharacterSlot - 1 < 0) return;
                _currentCharacterSlot = (int)MathF.Max(_currentCharacterSlot - 1, 0);
                _currentString = _currentString.Remove(_currentCharacterSlot, 1);
                return;
            }
            if (pressedKey == Keys.Enter)
            {
                RunCommand(_currentString);
                SetCurrentCharacterSlotToEnd();
                return;
            }

            var character = args.Character;
            if (char.IsControl(character))
                return;
            _currentString = _currentString.Insert(_currentCharacterSlot, character.ToString());
            _currentCharacterSlot++;
            _time = 0;
        }

        public static void SetCurrentCharacterSlotToEnd()
        {
            _currentCharacterSlot = _currentString.Length;
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
            _time++;

            if (Input.IsKeyPressed(Keys.Left))
            {
                _currentCharacterSlot = (int)MathF.Max(_currentCharacterSlot - 1, 0);
                _time = 0;
            }
            if (Input.IsKeyPressed(Keys.Right))
            {
                _currentCharacterSlot = (int)MathF.Min(_currentCharacterSlot + 1, _currentString.Length);
                _time = 0;
            }

            for (int i = 0; i < _previousStrings.Count; i++)
            {
                string s = _previousStrings[i];

                if (s.Contains("PixelDough"))
                {
                    s = Strings.Replace(s, "PixelDough", "[c:#ffaa00]Pixel[c:#00aaff]Dough[/c]");
                }

                Vector2 position = new Vector2(4, Game.Graphics.PreferredBackBufferHeight - _spriteFont.LineSpacing - ((_previousStrings.Count - i) * _spriteFont.MeasureString(s).Y));

                if (i == _previousStrings.Count - _previousStringCheckout)
                {
                    spriteBatch.Draw(_pixelTexture, new Rectangle(position.ToPoint(), _spriteFont.MeasureString(s).ToPoint()), Color.DarkSlateGray);
                }

                Color defaultLineColor = Color.Gray;

                if (s.StartsWith("ERROR"))
                {
                    defaultLineColor = Color.Red;
                }

                if (s.Contains("[c:"))
                {
                    int currentOffset = 0;

                    string[] splits = s.Split(new string[] { "[c:" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string _str in splits)
                    {
                        string str = _str;
                        // TODO: Make easier method of setting a default line color. Possibly using a new class to represent a "line". which just stores the string and color.
                        if (str.StartsWith("#"))
                        {
                            string color = str.Substring(0, 7);

                            string[] msgs = str.Substring(8).Split(new string[] { "[/c]" }, StringSplitOptions.RemoveEmptyEntries);

                            System.Drawing.Color clrColor = System.Drawing.ColorTranslator.FromHtml(color);
                            Color xColor = new Color(clrColor.R, clrColor.G, clrColor.B, clrColor.A);

                            spriteBatch.DrawString(_spriteFont, msgs[0], position + Vector2.UnitX * currentOffset, xColor);
                            currentOffset += (int)_spriteFont.MeasureString(msgs[0]).X;

                            if (msgs.Length == 2)
                            {
                                spriteBatch.DrawString(_spriteFont, msgs[1], position + Vector2.UnitX * currentOffset, defaultLineColor);
                                currentOffset += (int)_spriteFont.MeasureString(msgs[1]).X;
                            }
                        }
                        else
                        {
                            spriteBatch.DrawString(_spriteFont, str, position + Vector2.UnitX * currentOffset, defaultLineColor);
                            currentOffset += (int)_spriteFont.MeasureString(str).X;
                        }
                    }
                }
                else
                {
                    spriteBatch.DrawString(_spriteFont, s, position, defaultLineColor);
                }
            }

            spriteBatch.DrawString(_spriteFont, "> " + _currentString, new Vector2(4, Game.Graphics.PreferredBackBufferHeight - _spriteFont.LineSpacing), Color.White);

            Color cursorColor = Color.DarkSlateGray;
            if (_time % 60 <= 30)
                cursorColor = Color.White;
            spriteBatch.Draw(_pixelTexture, new Rectangle(new Vector2(4 + _spriteFont.MeasureString("> " + _currentString.Substring(0, _currentCharacterSlot)).X, Game.Graphics.PreferredBackBufferHeight - _spriteFont.LineSpacing).ToPoint(), new Vector2(2, _spriteFont.LineSpacing).ToPoint()), cursorColor);
            //spriteBatch.DrawString(spriteFont, "|", new Vector2(4 + spriteFont.MeasureString("> " + _currentString).X, Game.Graphics.PreferredBackBufferHeight - spriteFont.LineSpacing), Color.White);
        }


        public delegate void CommandAction(params string[] args);
        public struct Command
        {
            public string Name;
            public string Help;
            public CommandAction Action;
            
            public Command(string name, string help, CommandAction action)
            {
                Name = name;
                Help = help;
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
            else
            {
                Command command = Commands.Find(c => c.Name == paramList[0]);
                if (command.Action == null) throw new Exception("ERROR: Command \"" + paramList[0] + "\" not found!");
                _previousStrings.Add(command.Help);
            }
        }

        public static void LoadLevel(params string[] paramList)
        {
            Type sceneType = Type.GetType("MonoGame_Experiments.Scenes." + paramList[0]);
            if (sceneType == null) throw new Exception("ERROR: Scene with name \"" + paramList[0] + "\" does not exist!");
            Scene scene = (Scene)Activator.CreateInstance(sceneType);
            Game.ChangeScenes(scene);
        }

        public static void Unload(params string[] paramList)
        {
            Game.ChangeScenes(null);
        }

        public static void OpenLink(string link)
        {
            var ps = new ProcessStartInfo(link)
            {
                UseShellExecute = true
            };
            Process.Start(ps);
        }

        public static void Spawn(string componentName, int posX, int posY)
        {
            Type componentType = Type.GetType("MonoGame_Experiments.Components." + componentName);
            if (componentType == null) throw new Exception("ERROR: Component with name \"" + componentName + "\" does not exist!");
            Entity entity = Game._currentScene.AddEntity(new Vector2(posX, posY));
            Component component = (Component)Activator.CreateInstance(componentType, entity);
            entity.AddComponent(component);
        }
    }
}
