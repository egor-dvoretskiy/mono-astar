using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameTestGraphAlgs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonogameTestGraphAlgs.Models
{
    public class Map
    {
        private readonly int _tileSize = 48;
        private readonly int _tileOffset = 2;
        private readonly byte[] _validatedNodeValues;
        private readonly int[,] _map = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 6 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };
        private readonly Dictionary<int, Color> _tileColorDictionary;
        private readonly MapNode[,] _nodes;
        private readonly GraphicsDevice _graphicsDevice;

        public Map(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _validatedNodeValues = Enum.GetValues(typeof(MapNodeType)).Cast<byte>().ToArray();
            _tileColorDictionary = new Dictionary<int, Color>()
            {
                { 0, Color.White },
                { 1, Color.Black },
                { 6, Color.Blue },
                { 7, Color.Red },
            };

            _nodes = new MapNode[_map.GetLength(0), _map.GetLength(1)];
            FillNodes();
        }

        public MapNode[,] Field
        {
            get => _nodes;
        }

        public int TileSize
        {
            get => _tileSize;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    _nodes[i, j].Draw(spriteBatch);
                }
            }
        }

        private void FillNodes()
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    int nodeValue = _map[j, i];
                    Color color = _tileColorDictionary[nodeValue];

                    Texture2D texture = new Texture2D(_graphicsDevice, 1, 1);
                    texture.SetData(new Color[] { color });

                    _nodes[i, j] = new MapNode()
                    {
                        Texture = texture,
                        Rectangle = new Rectangle(i * _tileSize + _tileOffset, j * _tileSize + _tileOffset, _tileSize - _tileOffset * 2, _tileSize - _tileOffset * 2),
                        Color = color,
                        Type = GetTypeByNodeValue(nodeValue),
                    };
                }
            }
        }

        private MapNodeType GetTypeByNodeValue(int node)
        {
            if (!_validatedNodeValues.Any(x => x == node))
                return MapNodeType.Casual;

            return (MapNodeType)node;
        }
    }
}
