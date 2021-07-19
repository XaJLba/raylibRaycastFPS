using Raylib_cs;
using System.Numerics;

namespace raylibRaycastFPSnew
{
    struct Wall
    {
        public readonly LineSegment lineSegment;
        public readonly Color color;
        public readonly float height;
        public Wall(Vector2 startPosition, Vector2 endPosition, float height, Color color)
        {
            lineSegment = new LineSegment(startPosition, endPosition);
            this.color = color;
            this.height = height;
        }
    }
}
