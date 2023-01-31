using AStar.Models;
using MonogameTestGraphAlgs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MonogameTestGraphAlgs.Source.Algorithms
{
    public class AStar
    {
        private readonly Map _map;

        private LinkedList<Node> waypointsLinked = new LinkedList<Node>();
        private List<Node> closedNodes = new List<Node>();
        private List<Node> openedNodes = new List<Node>();

        public AStar(Map map)
        {
            _map = map;
        }

        public void Update(TilePosition currentPosition, TilePosition targetPosition)
        {
            if (currentPosition == targetPosition)
                return;

            Node currentNode = new Node
            {
                StepApproach = 1,
                EuristicApproach = CalculateEuristicApproach(currentPosition, targetPosition),
                TilePosition = currentPosition,
            };

            AssignClosedNode(currentNode);
            openedNodes.AddRange(AssignOpenNodes(currentPosition, targetPosition));
            AssignCurrentPosition();

            InitializeMapTypeNodes();
        }

        private void InitializeMapTypeNodes()
        {
            for (int i = 0; i < openedNodes.Count; i++)
            {
                _map.UpdateTileNode(openedNodes[i].TilePosition, openedNodes[i], Enums.AStarTileType.Opened);
            }

            for (int i = 0; i < closedNodes.Count; i++)
            {
                _map.UpdateTileNode(closedNodes[i].TilePosition, closedNodes[i], Enums.AStarTileType.Closed);
            }
        }

        private void AssignClosedNode(Node currentNode)
        {
            if (closedNodes.Any(x => x.TilePosition == currentNode.TilePosition))
                return; 

            closedNodes.Add(currentNode);

            var nodesToRemoveFromOpened = openedNodes.Where(x => closedNodes.Contains(x));
            for (int i = 0; i < nodesToRemoveFromOpened.Count(); i++)
            {
                openedNodes.Remove(nodesToRemoveFromOpened.ElementAt(i));
            }
        }

        private void AssignCurrentPosition()
        {
            /*var nodeIndex = openedNodes.FindIndex(x => x.Weight == openedNodes.Select(y => y.Weight).Min());
            if (nodeIndex < 0)
                return;*/

            var minArray = openedNodes.Where(x => x.Weight == openedNodes.Select(y => y.Weight).Min());
            if (minArray.Count() == 0)
                return;

            var nodeIndex = Random.Shared.Next(minArray.Count());
            Node appliedNode = minArray.ElementAt(nodeIndex);

            _map.UpdateCurrentPosition(appliedNode.TilePosition);
        }

        private List<Node> AssignOpenNodes(TilePosition position, TilePosition target)
        {
            List<Node> values = new List<Node>();
            // replace by Node (pos, weight)

            AssignOpenNodeLeftDirection(position, target, values);
            AssignOpenNodeTopDirection(position, target, values);
            AssignOpenNodeRightDirection(position, target, values);
            AssignOpenNodeBottomDirection(position, target, values);

            return values;
        }

        private void AssignOpenNodeBottomDirection(TilePosition position, TilePosition target, List<Node> values)
        {
            var moveBottom = new TilePosition()
            {
                X = position.X,
                Y = position.Y + 1,
            };
            if (IsBoundsKeeps(moveBottom) &&
                _map.Field[moveBottom.X, moveBottom.Y].Type != Enums.MapNodeType.Obstacle &&
                !IsClosedNode(moveBottom) &&
                !IsInOpenNodes(moveBottom))
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
        }

        private void AssignOpenNodeRightDirection(TilePosition position, TilePosition target, List<Node> values)
        {
            var moveRight = new TilePosition()
            {
                X = position.X + 1,
                Y = position.Y,
            };
            if (IsBoundsKeeps(moveRight) &&
                _map.Field[moveRight.X, moveRight.Y].Type != Enums.MapNodeType.Obstacle &&
                !IsClosedNode(moveRight) &&
                !IsInOpenNodes(moveRight))
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
        }

        private void AssignOpenNodeTopDirection(TilePosition position, TilePosition target, List<Node> values)
        {
            var moveTop = new TilePosition()
            {
                X = position.X,
                Y = position.Y - 1,
            };
            if (IsBoundsKeeps(moveTop) &&
                _map.Field[moveTop.X, moveTop.Y].Type != Enums.MapNodeType.Obstacle &&
                !IsClosedNode(moveTop) &&
                !IsInOpenNodes(moveTop))
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
        }

        private void AssignOpenNodeLeftDirection(TilePosition position, TilePosition target, List<Node> values)
        {
            var moveLeft = new TilePosition()
            {
                X = position.X - 1,
                Y = position.Y,
            };
            if (IsBoundsKeeps(moveLeft) &&
                _map.Field[moveLeft.X, moveLeft.Y].Type != Enums.MapNodeType.Obstacle &&
                !IsClosedNode(moveLeft) &&
                !IsInOpenNodes(moveLeft))
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
        }

        private bool IsInOpenNodes(TilePosition tilePosition)
        {
            for (int i = 0; i < openedNodes.Count; i++)
            {
                if (openedNodes[i].TilePosition == tilePosition)
                    return true;
            }

            return false;
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

            return xapproach + yapproach - 1;
        }
    }
}
