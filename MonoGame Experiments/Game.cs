using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Experiments.Components;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace MonoGame_Experiments
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager Graphics;
        private SpriteBatch _spriteBatch;

        private ScreenManager _screenManager;

        private GameObject _object1 = new GameObject();

        public Game()
        {
            Window.ClientSizeChanged += OnWindowResize;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //_graphics.IsFullScreen = false;
            //_graphics.PreferredBackBufferWidth = 320 * 4;
            //_graphics.PreferredBackBufferHeight = 180 * 4;

            Graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 60);
            Graphics.SynchronizeWithVerticalRetrace = true;

            _screenManager = new ScreenManager(this, 320, 180);
            _screenManager.Init(Graphics, Window);

            //_screenManager.ToggleFullScreen();
            Graphics.ApplyChanges();
            _screenManager.UpdateRenderRectangle(Window);

            Texture2D docTexture = Content.Load<Texture2D>("Sprites/Doc");
            _object1.AddComponent(new Sprite(docTexture, docTexture.Width, docTexture.Height, Vector2.Zero));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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

            _object1.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _screenManager.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            GraphicsDevice.SetRenderTarget(_screenManager.RenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _object1.Draw(_screenManager.SpriteBatch);
            _screenManager.SpriteBatch.End();

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Draw(_screenManager.RenderTarget, _screenManager.RenderRectangle, Color.White);
            _spriteBatch.End();


            base.Draw(gameTime);
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            _screenManager.UpdateRenderRectangle(Window);
            //_screenManager.UpdateRenderRectangle(Window);
        }
    }
}
