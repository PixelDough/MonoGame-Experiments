using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Experiments.Components;
using MonoGame_Experiments.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace MonoGame_Experiments
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager Graphics;
        public ContentHandler ContentHandler;
        private SpriteBatch _spriteBatch;

        private ScreenManager _screenManager;

        private Scene _currentScene;

        public static Camera2D Camera;
        public static bool DebugMode = true;

        public Game()
        {
            Window.ClientSizeChanged += OnWindowResize;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ContentHandler = new ContentHandler(Content.ServiceProvider, Content.RootDirectory);
            IsMouseVisible = true;

            Camera = new Camera2D(320, 180);
        }

        protected override void Initialize()
        {
            _currentScene = new SceneMenu();
            
            _screenManager = new ScreenManager(this, 320, 180);
            _screenManager.Init(Graphics, Window);

            _screenManager.UpdateRenderRectangle(Window);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Tilemap tilemap = OgmoTilemapManager.LoadLevelData("Ogmo Project/Level1.json");

            foreach (TilemapLayer layer in tilemap.layers)
            {
                if (layer.tileset != null)
                {
                    foreach (Tile tile in layer.GetTiles())
                    {
                        Entity tileObject = new Entity(tile.Position);
                        tileObject.AddComponent(tile);
                        Collider collider = new Collider(Vector2.Zero, tile.SpriteRectangle.Width, tile.SpriteRectangle.Height);
                        tileObject.AddComponent(collider);

                        _currentScene.gameObjects.Add(tileObject);
                    }
                }
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update(gameTime);

            if (Input.IsKeyPressed(Keys.F11))
            {
                _screenManager.ToggleFullScreen();
            }

            _currentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _screenManager.SpriteBatch.Begin(transformMatrix: Camera.TransformationMatrix, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            GraphicsDevice.SetRenderTarget(_screenManager.RenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _currentScene.Draw(_screenManager.SpriteBatch); 

            _screenManager.SpriteBatch.End();

            DrawRenderTargetToScreen();


            base.Draw(gameTime);
        }

        private void DrawRenderTargetToScreen()
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Draw(_screenManager.RenderTarget, _screenManager.RenderRectangle, Color.White);
            _spriteBatch.End();
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            _screenManager.UpdateRenderRectangle(Window);
            //_screenManager.UpdateRenderRectangle(Window);
        }
    }
}
