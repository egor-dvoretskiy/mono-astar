using AStar.Models;
using MonogameTestGraphAlgs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameTestGraphAlgs.Source.Algorithms
{
    public class AStar
    {
        private readonly Map _map;

        private LinkedList<Node> waypointsLinked = new LinkedList<Node>();
        private List<Node> closedNodes = new List<Node>();
        private List<Node> openedNodes;

        public AStar(Map map)
        {
            _map = map;
        }

        public void Update(TilePosition currentPosition, TilePosition targetPosition)
        {
            openedNodes = AssignOpenNodes(currentPosition, targetPosition);
        }

        public void Draw()
        {

        }

        private List<Node> AssignOpenNodes(TilePosition position, TilePosition target)
        {
            List<Node> values = new List<Node>();
            // replace by Node (pos, weight)

            var moveLeft = new TilePosition() 
            { 
                X = position.X - 1,
                Y = position.Y,
            };
            if (IsBoundsKeeps(moveLeft) &&
                _map.Field[moveLeft.X, moveLeft.Y].Type != Enums.MapNodeType.Obstacle &&
                !IsClosedNode(moveLeft))
            {
                values.Add(
                    new Node()
                    {
                        TilePosition = moveLeft,
                        EuristicApproach = CalculateEuristicApproach(moveLeft, target),
                        StepApproach = 1
                    }
                );
            }

            var moveTop = new TilePosition()
            {
                X = position.X,
                Y = position.Y - 1,
            };
            if (IsBoundsKeeps(moveTop) &&
                _map.Field[moveTop.X, moveTop.Y].Type != Enums.MapNodeType.Obstacle &&
                !IsClosedNode(moveTop))
            {
                values.Add(
                    new Node()
                    {
                        TilePosition = moveTop,
                        EuristicApproach = CalculateEuristicApproach(moveTop, target),
                        StepApproach = 1
                    }
                );
            }

            var moveRight = new TilePosition()
            {
                X = position.X + 1,
                Y = position.Y,
            };
            if (IsBoundsKeeps(moveRight) &&
                _map.Field[moveRight.X, moveRight.Y].Type != Enums.MapNodeType.Obstacle &&
                !IsClosedNode(moveRight))
            {
                values.Add(
                    new Node()
                    {
                        TilePosition = moveRight,
                        EuristicApproach = CalculateEuristicApproach(moveRight, target),
                        StepApproach = 1
                    }
                );
            }

            var moveBottom = new TilePosition()
            {
                X = position.X,
                Y = position.Y + 1,
            };
            if (IsBoundsKeeps(moveBottom) &&
                _map.Field[moveBottom.X, moveBottom.Y].Type != Enums.MapNodeType.Obstacle &&
                !IsClosedNode(moveBottom))
            {
                values.Add(
                    new Node()
                    {
                        TilePosition = moveBottom,
                        EuristicApproach = CalculateEuristicApproach(moveBottom, target),
                        StepApproach = 1
                    }
                );
            }

            return values;
        }

        private bool IsBoundsKeeps(TilePosition tilePosition)
        {
            return
                tilePosition.X >= 0 &&
                tilePosition.X < _map.Field.GetLength(0) &&
                tilePosition.Y >= 0 &&
                tilePosition.Y < _map.Field.GetLength(1);
        }

        private bool IsClosedNode(TilePosition tilePosition)
        {
            return closedNodes.Any(x => x.TilePosition == tilePosition);
        }

        private int CalculateEuristicApproach(TilePosition currentPosition, TilePosition targetPosition)
        {
            var xapproach = Math.Abs(currentPosition.X - targetPosition.X);
            var yapproach = Math.Abs(currentPosition.Y - targetPosition.Y);

            return xapproach + yapproach;
        }
    }
}
