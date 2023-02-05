using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTestGraphAlgs.Enums;
using MonogameTestGraphAlgs.Models;
using MonogameTestGraphAlgs.Models.GUI;
using MonogameTestGraphAlgs.Source.Algorithms;
using System;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace MonogameTestGraphAlgs
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Button buttonRun;
        private Map map;
        private ApplicationStage applicationStage;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = false;
            //IsFixedTimeStep = true;
            //MaxElapsedTime = TimeSpan.FromSeconds(10);
            //TargetElapsedTime = TimeSpan.FromSeconds(1);
            applicationStage = ApplicationStage.Preset;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();

            map = new Map(GraphicsDevice, Content);

            buttonRun = new Button(GraphicsDevice, 
                new Rectangle
                (
                    _graphics.PreferredBackBufferHeight,
                    0,
                    _graphics.PreferredBackBufferWidth - _graphics.PreferredBackBufferHeight,
                    _graphics.PreferredBackBufferHeight
                )
            );
            buttonRun.OnMouseClick += ButtonRun_OnMouseClick;

            base.Initialize();
        }

        private void ButtonRun_OnMouseClick()
        {
            applicationStage = applicationStage == ApplicationStage.Preset ? ApplicationStage.Work : ApplicationStage.Preset;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Fonts/ButtonFont");

            // TODO: use this.Content to load your game content here
        }

        protected override async void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            buttonRun.Update(applicationStage);
            map.Update(applicationStage);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            buttonRun.Draw(spriteBatch, spriteFont);
            DrawMainArea();
            map.Draw(gameTime, spriteBatch);

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