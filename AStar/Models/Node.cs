using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar.Models
{
    public struct Node
    {
        public TilePosition TilePosition;

        public int StepApproach;

        public int EuristicApproach;

        public int Weight
        { 
            get => StepApproach + EuristicApproach;
        }
    }
}
