using System;
using System.Collections.Generic;
using System.Text;

namespace raylibRaycastFPSnew
{
    class Level
    {
        public Wall[] walls { get; }
        public Level(Wall[] walls)
        {
            this.walls = walls;
        }
    }
}
