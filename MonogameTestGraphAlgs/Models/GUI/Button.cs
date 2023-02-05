using Microsoft.Xna.Framework;
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
        private readonly string runButtonText = "Run";
        private readonly string stopButtonText = "Stop";
        private readonly MouseInteractor _mouseInteractor;

        private ApplicationStage applicationStage;

        public delegate void MouseHandler();

        public event MouseHandler OnMouseClick;

        public Rectangle OutsideBox { get; set; }

        public Rectangle Rectangle { get; set; }

        public Texture2D Texture { get; set; }

        public Color Color { get; set; }

        public Button(GraphicsDevice graphicsDevice, Rectangle outsideBox)
        {
            Color = Color.Wheat;
            Texture = new Texture2D(graphicsDevice, 1, 1);
            Texture.SetData(new Color[] { Color });

            _mouseInteractor = new MouseInteractor();
            _mouseInteractor.OnMouseLeftClick += MouseInteractor_OnMouseLeftClick;

            OutsideBox = outsideBox;
            Rectangle = new Rectangle(
                outsideBox.X,
                0,
                outsideBox.Width,
                30
            );
        }

        private void MouseInteractor_OnMouseLeftClick()
        {
            var mousePosition = Mouse.GetState();

            if (Rectangle.Intersects(new Rectangle(mousePosition.Position, new Point(1, 1))))
                OnMouseClick?.Invoke();
        }

        public void Update(ApplicationStage applicationStage)
        {
            this.applicationStage = applicationStage;

            _mouseInteractor.Update();
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(Texture,
                Rectangle,
                Color.Wheat
            );

            switch (applicationStage)
            {
                case ApplicationStage.Preset:
                    {
                        spriteBatch.DrawString(
                            spriteFont,
                            runButtonText,
                            new Vector2()
                            {
                                X = Rectangle.X + Rectangle.Width / 2 - spriteFont.MeasureString(runButtonText).X / 2,
                                Y = Rectangle.Y + Rectangle.Height / 2 - spriteFont.MeasureString(runButtonText).Y / 2,
                            },
                            Color.Magenta
                        );
                    }
                    break;
                case ApplicationStage.Work:
                    {
                        spriteBatch.DrawString(
                            spriteFont,
                            stopButtonText,
                            new Vector2()
                            {
                                X = Rectangle.X + Rectangle.Width / 2 - spriteFont.MeasureString(stopButtonText).X / 2,
                                Y = Rectangle.Y + Rectangle.Height / 2 - spriteFont.MeasureString(stopButtonText).Y / 2,
                            },
                            Color.Magenta
                        );
                    }
                    break;
            }
        }
    }
}
