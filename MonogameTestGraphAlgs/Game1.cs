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
        private readonly MonogameTestGraphAlgs.Source.Algorithms.AStar _astar;
        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch spriteBatch;
        private Map map;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _astar = new Source.Algorithms.AStar();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();

            map = new Map(GraphicsDevice);

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

            _astar.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            DrawMainArea();
            map.Draw(gameTime, spriteBatch);
            _astar.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawMainArea()
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.DarkGray });
            spriteBatch.Draw(texture, new Rectangle(0, 0, _graphics.PreferredBackBufferHeight, _graphics.PreferredBackBufferHeight), Color.DarkGray);
        }
    }
}