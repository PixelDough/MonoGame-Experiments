using ChaiFoxes.FMODAudio;
using ChaiFoxes.FMODAudio.Studio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Experiments.Components;
using MonoGame_Experiments.Scenes;
using System;

namespace MonoGame_Experiments
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager Graphics;
        public ContentHandler ContentHandler;
        private SpriteBatch _spriteBatch;

        public static ScreenManager ScreenManager;

        // TODO: Make a SceneManager class
        public static Scene _currentScene;

        public static Camera2D Camera;
        public static bool DebugMode = false;
        public static bool Paused = false;

        // TODO: Make a tilemap manager

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
            FMODManager.Init(FMODMode.CoreAndStudio, "Content/FMOD/FMOD Project");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            AudioManager.LoadBank("Master.bank");
            AudioManager.LoadBank("Master.strings.bank");

            _spriteBatch?.Dispose();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager = new ScreenManager(this, 320, 180);
            ScreenManager.Init(Graphics, Window, _spriteBatch);
            ScreenManager.UpdateRenderRectangle(Window);

            Window.TextInput += DebugConsole.TextInputHandler;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            FMODManager.Update();

            Input.Update(gameTime);

            if (Input.IsKeyPressed(Keys.F11))
            {
                ScreenManager.ToggleFullScreen();
            }
            if (Input.IsKeyPressed(Keys.F4))
            {
                DebugMode = !DebugMode;
            }

            _currentScene?.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Make a more robust layering system. Use multiple spriteBatches, and some way of collecting anything that needs to be drawn using a certain spriteBatch. Maybe make a manager class.
            // Tip: FrontToBack: 1f = Front, 0f = Back
            _spriteBatch.Begin(transformMatrix: Camera.TransformationMatrix, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.Deferred);
            GraphicsDevice.SetRenderTargets(ScreenManager.RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            _currentScene?.Draw(_spriteBatch);
            DebugManager.Draw(_spriteBatch);
            _spriteBatch.End();

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
            GraphicsDevice.SetRenderTargets(null);
            //_spriteBatch.Draw(ScreenManager.RenderTarget, ScreenManager.RenderRectangle, Color.White);

            Rectangle rect = ScreenManager.RenderRectangle;
            float RATIO = (float)rect.Height / rect.Width;

            //if (Window.ClientBounds.Width > Window.ClientBounds.Height)
            //{
            //    rect.Y = 0;
            //    rect.Height = Window.ClientBounds.Height;
            //    rect.Width = (int)MathF.Round(rect.Height * (1f / RATIO));
            //}
            //else
            //{
            //    rect.X = 0;
            //    rect.Width = Window.ClientBounds.Width;
            //    rect.Height = (int)MathF.Round(rect.Width * RATIO);
            //}

            _spriteBatch.Draw(ScreenManager.RenderTarget, rect, Color.White);
            _spriteBatch.End();
        }

        private void DrawDebugOverlay(GameTime gameTime)
        {
            // TODO: Add a DebugConsole class, which will keep the main command line in the bottom left, and will store and show recent commands above it.
            // It will also store things like the system font, so basically all of this will be relocated there.
            
            SpriteFont font = Content.Load<SpriteFont>("Fonts/system");

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Hello World!", Vector2.Zero, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);

            DebugConsole.Draw(_spriteBatch);

            Color deltaTimeTooHigh = (gameTime.ElapsedGameTime.TotalSeconds > TargetElapsedTime.TotalSeconds) ? Color.Red : Color.White;
            _spriteBatch.DrawString(font, "DeltaTime: " + gameTime.ElapsedGameTime.TotalSeconds, Vector2.UnitY * 24, deltaTimeTooHigh);
            _spriteBatch.DrawString(font, "FPS: " + gameTime.ElapsedGameTime.TotalSeconds / TargetElapsedTime.TotalSeconds * 60, Vector2.UnitY * 48, Color.White);
            _spriteBatch.End();
        }

        public static void ChangeScenes(Scene scene)
        {
            _currentScene?.Dispose();
            GC.Collect();
            _currentScene = scene;
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            if (ScreenManager == null) { return; }

            ScreenManager.UpdateRenderRectangle(Window);
            //_screenManager.UpdateRenderRectangle(Window);
        }
    }
}
