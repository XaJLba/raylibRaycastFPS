using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace raylibRaycastFPSnew
{
    struct LineSegment
    {
        public readonly Vector2 startPosition;
        public readonly Vector2 endPosition;
        public LineSegment(Vector2 startPosition, Vector2 endPosition)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
        }
    }
}
