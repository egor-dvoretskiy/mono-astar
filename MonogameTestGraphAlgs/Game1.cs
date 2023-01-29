using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTestGraphAlgs.Models;
using System;
using System.Security.AccessControl;

namespace MonogameTestGraphAlgs
{
    public class Game1 : Game
    {
        private readonly Map _map;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            _map = new Map();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = false;
            /*IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);*/
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            DrawMainArea();
            _map.Draw(gameTime, spriteBatch, GraphicsDevice);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawMainArea()
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.DarkGray });
            spriteBatch.Draw(texture, new Rectangle(0, 0, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferHeight), Color.DarkGray);
        }
    }
}