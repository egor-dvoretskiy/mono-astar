using AStar.Models;
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

        public Color Color { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }
    }
}
