using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments
{
    public class ScreenManager
    {
        public bool Initialized { get; private set; } = false;
        public RenderTarget2D RenderTarget { get; private set; }
        public RenderTarget2D RenderTargetUI { get; private set; }

        private Game _game;

        private int _width = 960;
        private int _height = 540;

        public int RenderScale { get; private set; } = 1;

        private float _windowAspect;
        private float _preferredAspect;
        public Rectangle RenderRectangle { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        
        public ScreenManager(Game game, int width, int height)
        {
            _game = game;
            _width = width;
            _height = height;
        }
        public ScreenManager(Game game, Vector2 resolution) : this(game, (int)resolution.X, (int)resolution.Y) { }

        public void Init(GraphicsDeviceManager graphicsDeviceManager, GameWindow window, SpriteBatch spriteBatch)
        {
            graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
            _game.IsFixedTimeStep = true;
            _game.TargetElapsedTime = TimeSpan.FromSeconds(1f / 60);

            SpriteBatch?.Dispose();
            SpriteBatch = spriteBatch;
            RenderTarget?.Dispose();
            RenderTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, _width, _height);
            RenderScale = 4;

            // I'm not sure why this is needed, but with this set to false (the default value), the window shrinks when dragged around
            // the screen, on specific monitors.
            window.AllowUserResizing = true;

            window.IsBorderless = false;
            
            Game.Graphics.HardwareModeSwitch = false;

            ResetWindowSizeToCurrentScale();

            Game.Graphics.ApplyChanges();

            Initialized = true;
        }

        public void ResetWindowSizeToCurrentScale()
        {
            Game.Graphics.PreferredBackBufferWidth = _width * RenderScale;
            Game.Graphics.PreferredBackBufferHeight = _height * RenderScale;
            Game.Graphics.ApplyChanges();

            UpdateRenderRectangle(_game.Window);
        }

        public void UpdateRenderRectangle(GameWindow window)
        {
            _windowAspect = (float)window.ClientBounds.Width / (float)window.ClientBounds.Height;
            _preferredAspect = (float)RenderTarget.Width / (float)RenderTarget.Height;

            int scale = 1;

            while (RenderTarget.Width * (scale + 1) <= window.ClientBounds.Width && RenderTarget.Height * (scale + 1) <= window.ClientBounds.Height)
                scale++;

            Rectangle dst = new Rectangle(0, 0, RenderTarget.Width * scale, RenderTarget.Height * scale);
            if (_windowAspect <= _preferredAspect)
            {
                // Output is taller than it is wider, bars on top/bottom
                float posY = (window.ClientBounds.Height - dst.Height) / 2;
                dst.Y = (int)MathF.Floor(posY);
                float posX = (window.ClientBounds.Width - dst.Width) / 2;
                dst.X = (int)MathF.Floor(posX);
            }
            else
            {
                // Output is wider than it is tall, bars left/right
                float posY = (window.ClientBounds.Height - dst.Height) / 2;
                dst.Y = (int)MathF.Floor(posY);
                float posX = (window.ClientBounds.Width - dst.Width) / 2;
                dst.X = (int)MathF.Floor(posX);
            }
            RenderRectangle = dst;
            RenderScale = scale;
        }

        private void ChangeWindowSize(int width, int height)
        {

        }

        public void ToggleFullScreen()
        {
            bool targetState = !Game.Graphics.IsFullScreen;

            if (targetState)
            {
                Game.Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Game.Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                Game.Graphics.PreferredBackBufferWidth = _width * 4;
                Game.Graphics.PreferredBackBufferHeight = _height * 4;
            }

            Game.Graphics.IsFullScreen = targetState;

            Game.Graphics.ApplyChanges();

            //UpdateRenderRectangle(_game.Window);

        }
    }
}
