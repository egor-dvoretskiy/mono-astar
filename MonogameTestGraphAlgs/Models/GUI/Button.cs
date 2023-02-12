using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTestGraphAlgs.Enums;
using MouseInteractorLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameTestGraphAlgs.Models.GUI
{
    public class Button
    {
        private readonly string _runButtonText = "Run";
        private readonly string _stopButtonText = "Stop";
        private readonly MouseInteractor _mouseInteractor;
        private readonly Texture2D _textureCommon;
        private readonly Texture2D _textureHover;

        private ApplicationStage applicationStage;
        private bool _isHovered;

        public delegate void MouseHandler();

        public event MouseHandler OnMouseClick;

        public Rectangle OutsideBox { get; set; }

        public Rectangle Rectangle { get; set; }

        public Texture2D Texture { get; set; }

        public bool IsHovered
        {
            get => _isHovered;
        }

        public Button(GraphicsDevice graphicsDevice, Rectangle outsideBox, ContentManager content)
        {
            _textureCommon = content.Load<Texture2D>("GUI/Button");
            _textureHover = content.Load<Texture2D>("GUI/ButtonHover");

            Texture = _textureCommon;

            _mouseInteractor = new MouseInteractor();
            _mouseInteractor.OnMouseLeftClick += MouseInteractor_OnMouseLeftClick;

            OutsideBox = outsideBox;
            Rectangle = new Rectangle(
                outsideBox.X + outsideBox.Width / 2 - _textureCommon.Width / 2,
                0,
                Texture.Width,
                Texture.Height
            );
        }

        public void Update(ApplicationStage applicationStage)
        {
            this.applicationStage = applicationStage;

            CheckIfHover();
            _mouseInteractor.Update();
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(IsHovered ? _textureHover : _textureCommon,
                Rectangle,
                Color.White
            );

            switch (applicationStage)
            {
                case ApplicationStage.Preset:
                    {
                        spriteBatch.DrawString(
                            spriteFont,
                            _runButtonText,
                            new Vector2()
                            {
                                X = Rectangle.X + Rectangle.Width / 2 - spriteFont.MeasureString(_runButtonText).X / 2,
                                Y = Rectangle.Y + Rectangle.Height / 2 - spriteFont.MeasureString(_runButtonText).Y / 2,
                            },
                            Color.Black
                        );
                    }
                    break;
                case ApplicationStage.Work:
                    {
                        spriteBatch.DrawString(
                            spriteFont,
                            _stopButtonText,
                            new Vector2()
                            {
                                X = Rectangle.X + Rectangle.Width / 2 - spriteFont.MeasureString(_stopButtonText).X / 2,
                                Y = Rectangle.Y + Rectangle.Height / 2 - spriteFont.MeasureString(_stopButtonText).Y / 2,
                            },
                            Color.Black
                        );
                    }
                    break;
            }
        }

        private void CheckIfHover()
        {
            var mousePosition = Mouse.GetState();

            if (Rectangle.Intersects(new Rectangle(mousePosition.Position, new Point(1, 1))))
            {
                _isHovered = true;
                return;
            }

            _isHovered = false;
        }

        private void MouseInteractor_OnMouseLeftClick()
        {
            if (IsHovered)
                OnMouseClick?.Invoke();
        }
    }
}
