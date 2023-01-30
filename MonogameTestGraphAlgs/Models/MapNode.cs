﻿using AStar.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameTestGraphAlgs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MonogameTestGraphAlgs.Models
{
    public class MapNode
    {
        private readonly byte[] _validatedNodeValues;

        public MapNode()
        {
            _validatedNodeValues = Enum.GetValues(typeof(MapNodeType)).Cast<byte>().ToArray();
        }

        public Node Node { get; set; }

        public Texture2D Texture { get; set; }

        public Rectangle Rectangle { get; set; }

        public MapNodeType Type { get; set; }

        public AStarTileType AStarTileType { get; set; }

        public int Value
        {
            get => (int)Type;
        }

        public Color Color { get; set; }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);

            //if (AStarTileType == AStarTileType.Opened)
                spriteBatch.DrawString(
                    spriteFont, 
                    "5", 
                    new Vector2(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2),
                    Color.Magenta);
        }
    }
}
