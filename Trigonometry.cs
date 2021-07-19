using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace raylibRaycastFPSnew
{
    static class Trigonometry
    {
        public static float FixAngle(float angle)
        {
            if (angle > 6.28318530f) // = PI * 2
            {
                angle = 0;
            }
            if (angle < 0)
            {
                angle = 6.28318530f; // = PI * 2
            }
            return angle;
        }

        public static Vector2 GetDirectionFromAngle(float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        public static Vector2 GetRectangleCenter(Rectangle rectangle)
        {
            return new Vector2(rectangle.x + rectangle.width / 2, rectangle.y + rectangle.height / 2);
        }
        public static float GetPointToPointSquareDistance(Vector2 p1, Vector2 p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }
        public static float GetPointToPointRealDistance(Vector2 p1, Vector2 p2)
        {
            return MathF.Sqrt(GetPointToPointSquareDistance(p1, p2));
        }
    }
}
