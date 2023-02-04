using AStar.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private readonly int[,] _map = new int[16, 16];
        private readonly Dictionary<int, Color> _tileColorDictionary;
        private readonly MapNode[,] _nodes;
        private readonly SpriteFont _spriteFont;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly MonogameTestGraphAlgs.Source.Algorithms.AStar _astar;

        private TilePosition currentPosition;
        private TilePosition targetPosition;

        private MouseState previouseMouseState;
        private MouseState currentMouseState;
        private MapNodeType latestClickedNodeType;
        private TilePosition latestClickedTilePosition;
        private bool isLeftButtonMouseStateHold = false;

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

        public Rectangle WorkAreaRectangle
        {
            get => new Rectangle(
                0,
                0,
                _map.GetLength(0) * TileSize, 
                _map.GetLength(1) * TileSize
            );
        }

        public MapNode[,] Field
        {
            get => _nodes;
        }

        public int TileSize
        {
            get => _tileSize;
        }

        public void Update(ApplicationStage applicationStage)
        {
            UpdateMouseState();

            switch (applicationStage)
            {
                case ApplicationStage.Preset:
                    {
                        TilePosition currentTile = ConvertVectorToTilePosition(currentMouseState);

                        if (!IsInWorkArea(currentMouseState))
                            return;

                        UpdateImportantTile(currentTile);
                        UpdateBlackTile(currentTile);
                    }
                    break;
                case ApplicationStage.Work:
                    {
                        _astar.Update(
                            currentPosition,
                            targetPosition
                        );
                    }
                    break;
            }
        }

        public void InitializePositioning(TilePosition current, TilePosition target)
        {
            UpdateCurrentPosition(current);
            UpdateTargetPosition(target);
        }

        public void UpdateCurrentPosition(TilePosition tilePosition)
        {
            currentPosition = tilePosition;
            _nodes[currentPosition.X, currentPosition.Y].UpdateType(MapNodeType.StartPoint, _tileColorDictionary[(int)MapNodeType.StartPoint]);
        }

        public void UpdateTargetPosition(TilePosition tilePosition)
        {
            targetPosition = tilePosition;
            _nodes[targetPosition.X, targetPosition.Y].UpdateType(MapNodeType.EndPoint, _tileColorDictionary[(int)MapNodeType.EndPoint]);
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

        private void UpdateMouseState()
        {
            previouseMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (!IsInWorkArea(currentMouseState))
                return;

            if (previouseMouseState.LeftButton == ButtonState.Released &&
                currentMouseState.LeftButton == ButtonState.Pressed)
            {
                latestClickedTilePosition = ConvertVectorToTilePosition(currentMouseState);
                latestClickedNodeType = (MapNodeType)GetMapValueByTilePosition(latestClickedTilePosition);
            }

            if (previouseMouseState.LeftButton == ButtonState.Pressed &&
                currentMouseState.LeftButton == ButtonState.Pressed)
                isLeftButtonMouseStateHold = true;
            else
                isLeftButtonMouseStateHold = false;
        }

        private void UpdateImportantTile(TilePosition currentTile)
        {
            if (!IsImportantTileReadyToMove())
                return;

            //if (_nodes[currentTile.X, currentTile.Y].Type != MapNodeType.EndPoint || _nodes[currentTile.X, currentTile.Y].Type != MapNodeType.StartPoint)
            {
                if (latestClickedNodeType == MapNodeType.EndPoint)
                {
                    _nodes[targetPosition.X, targetPosition.Y].UpdateType(MapNodeType.Casual, _tileColorDictionary[(int)MapNodeType.Casual]);
                    _nodes[currentTile.X, currentTile.Y].UpdateType(MapNodeType.EndPoint, _tileColorDictionary[(int)MapNodeType.EndPoint]);

                    targetPosition = currentTile;
                    return;
                }

                if (latestClickedNodeType == MapNodeType.StartPoint)
                {
                    _nodes[currentPosition.X, currentPosition.Y].UpdateType(MapNodeType.Casual, _tileColorDictionary[(int)MapNodeType.Casual]);
                    _nodes[currentTile.X, currentTile.Y].UpdateType(MapNodeType.StartPoint, _tileColorDictionary[(int)MapNodeType.StartPoint]);

                    currentPosition = currentTile;
                    return;
                }
            }
        }

        private void UpdateBlackTile(TilePosition currentTile)
        {
            if (!IsNotImportantTile(currentTile))
                return;

            if (GetMapValueByTilePosition(currentTile) == 1 && 
                previouseMouseState.RightButton == ButtonState.Pressed)
            {
                SetValueToMapByTilePosition(currentTile, 0);
                return;
            }
                
            if (GetMapValueByTilePosition(currentTile) == 0 && 
                previouseMouseState.LeftButton == ButtonState.Pressed)
            {
                SetValueToMapByTilePosition(currentTile, 1);
                return;
            }
        }

        private TilePosition ConvertVectorToTilePosition(MouseState mouseState)
        {
            int xtile = (int)Math.Floor((double)mouseState.X / (double)TileSize);
            int ytile = (int)Math.Floor((double)mouseState.Y / (double)TileSize);

            return new TilePosition()
            {
                X = xtile,
                Y = ytile,
            };
        }

        private bool IsInWorkArea(MouseState mouseState) =>
            WorkAreaRectangle.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1));

        private bool IsNotImportantTile(TilePosition tilePosition) =>
            !IsTilesMatch(tilePosition, currentPosition) && !IsTilesMatch(tilePosition, targetPosition);

        private bool IsTilesMatch(TilePosition tilePosition1, TilePosition tilePosition2) => tilePosition1 == tilePosition2;

        private int GetMapValueByTilePosition(TilePosition tilePosition) => (int)_nodes[tilePosition.X, tilePosition.Y].Type;

        private void SetValueToMapByTilePosition(TilePosition tilePosition, int value)
        {
            _nodes[tilePosition.X, tilePosition.Y].UpdateType((MapNodeType)value, _tileColorDictionary[value]);
        }

        private bool IsImportantTileReadyToMove() => (latestClickedNodeType == MapNodeType.EndPoint || latestClickedNodeType == MapNodeType.StartPoint) && isLeftButtonMouseStateHold;

        private MapNodeType GetTypeByNodeValue(int node)
        {
            if (!_validatedNodeValues.Any(x => x == node))
                return MapNodeType.Casual;

            return (MapNodeType)node;
        }
    }
}
