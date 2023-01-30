using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar.Models
{
    public struct TilePosition
    {
        public int X;

        public int Y;

        public static bool operator ==(TilePosition position1, TilePosition position2) =>
            position1.X == position2.X && position1.Y == position2.Y;

        public static bool operator !=(TilePosition position1, TilePosition position2) =>
            position1.X != position2.X || position1.Y != position2.Y;
    }
}
