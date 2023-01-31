using AStar.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private readonly SpriteFont _spriteFont;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly MonogameTestGraphAlgs.Source.Algorithms.AStar _astar;

        private TilePosition currentPosition;
        private TilePosition targetPosition;

        public Map(GraphicsDevice graphicsDevice, ContentManager contentManager)
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

            _astar = new Source.Algorithms.AStar(this);
            _spriteFont = contentManager.Load<SpriteFont>("Fonts/FontAstarWeight");
            InitializePositioning(new TilePosition() { X = 4, Y = 14 }, new TilePosition() { X = 15, Y = 0 });
        }

        public MapNode[,] Field
        {
            get => _nodes;
        }

        public int TileSize
        {
            get => _tileSize;
        }

        public void Update()
        {
            //var currentPosition = GetCurrentPosition();

            //if (currentPosition != null)
                _astar.Update(
                    currentPosition,
                    targetPosition
                );
        }

        public void InitializePositioning(TilePosition current, TilePosition target)
        {
            UpdateCurrentPosition(current);
            UpdateTargetPosition(target);
        }

        public void UpdateCurrentPosition(TilePosition tilePosition)
        {
            currentPosition = tilePosition;
        }

        public void UpdateTargetPosition(TilePosition tilePosition)
        {
            targetPosition = tilePosition;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    _nodes[i, j].Draw(spriteBatch, _spriteFont);
                }
            }
        }

        public void UpdateTileNode(TilePosition tilePosition, Node node, AStarTileType type)
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    if (tilePosition == _nodes[i, j].TilePosition)
                    {
                        _nodes[i, j].Node = node;
                        _nodes[i, j].AStarTileType = type;

                        switch(type)
                        {
                            case AStarTileType.Opened:
                                _nodes[i, j].Color = new Color(Color.SkyBlue, 128);
                                break;
                            case AStarTileType.Closed:
                                _nodes[i, j].Color = new Color(Color.LimeGreen, 128);
                                break;
                        }

                        return;
                    }

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
                        TilePosition = new TilePosition()
                        {
                            X = i,
                            Y = j,
                        }
                    };
                }
            }
        }

        private TilePosition? GetCurrentPosition()
        {
            for (int i = 0; i < Field.GetLength(0); i++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    if (Field[i, j].Type == MapNodeType.StartPoint)
                        return new TilePosition()
                        {
                            X = i,
                            Y = j,
                        };
                }
            }

            return null;
        }

        private MapNodeType GetTypeByNodeValue(int node)
        {
            if (!_validatedNodeValues.Any(x => x == node))
                return MapNodeType.Casual;

            return (MapNodeType)node;
        }
    }
}
