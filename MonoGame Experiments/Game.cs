using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Experiments.Components;
using MonoGame_Experiments.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
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

        // CHANGE LATER
        public static Scene _currentScene;

        public static Camera2D Camera;
        public static bool DebugMode = false;

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

            

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update(gameTime);

            if (Input.IsKeyPressed(Keys.F11))
            {
                _screenManager.ToggleFullScreen();
            }
            if (Input.IsKeyPressed(Keys.F4) || (Input.IsInputDown(buttons: new List<Buttons>() { Buttons.Back }) && Input.IsInputPressed(buttons: new List<Buttons>() { Buttons.Y })))
            {
                DebugMode = !DebugMode;
            }

            _currentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Make a more robust layering system. Use multiple spriteBatches, and some way of collecting anything that needs to be drawn using a certain spriteBatch. Maybe make a manager class.
            // Tip: FrontToBack: 1f = Front, 0f = Back
            _screenManager.SpriteBatch.Begin(transformMatrix: Camera.TransformationMatrix, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            GraphicsDevice.SetRenderTarget(_screenManager.RenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _currentScene.Draw(_screenManager.SpriteBatch);
            

            _screenManager.SpriteBatch.End();

            DrawRenderTargetToScreen();

            DrawDebugOverlay(gameTime);

            base.Draw(gameTime);
        }

        private void DrawRenderTargetToScreen()
        {
            //Rectangle partRectangle = new Rectangle();
            //partRectangle.X = Camera.Viewport.Width - (int)MathF.Round(Camera.Viewport.Width / Camera.Zoom);
            //partRectangle.Y = Camera.Viewport.Height - (int)MathF.Round(Camera.Viewport.Height / Camera.Zoom);
            //partRectangle.Width = (int)MathF.Round(Camera.Viewport.Width / Camera.Zoom);
            //partRectangle.Height = (int)MathF.Round(Camera.Viewport.Height / Camera.Zoom);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Draw(_screenManager.RenderTarget, _screenManager.RenderRectangle, Color.White);
            _spriteBatch.End();
        }

        private void DrawDebugOverlay(GameTime gameTime)
        {
            // TODO: Add a DebugConsole class, which will keep the main command line in the bottom left, and will store and show recent commands above it.
            // It will also store things like the system font, so basically all of this will be relocated there.
            SpriteFont font = Content.Load<SpriteFont>("Fonts/system");

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Hello World!", Vector2.Zero, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);

            Color deltaTimeTooHigh = (gameTime.ElapsedGameTime.TotalSeconds > TargetElapsedTime.TotalSeconds) ? Color.Red : Color.White;
            _spriteBatch.DrawString(font, "DeltaTime: " + gameTime.ElapsedGameTime.TotalSeconds, Vector2.UnitY * 24, deltaTimeTooHigh);
            _spriteBatch.End();
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            if (_screenManager == null) { return; }

            _screenManager.UpdateRenderRectangle(Window);
            //_screenManager.UpdateRenderRectangle(Window);
        }
    }
}
