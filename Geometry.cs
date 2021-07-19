using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace raylibRaycastFPSnew
{
    static class Geometry
    {
        public static float FixAngle(float angleInRadians)
        {
            if (angleInRadians > 6.28318530f) // = PI * 2
            {
                angleInRadians = 0;
            }
            if (angleInRadians < 0)
            {
                angleInRadians = 6.28318530f; // = PI * 2
            }
            return angleInRadians;
        }

        public static Vector2 GetDirectionFromAngle(float angleInRadians)
        {
            return new Vector2(MathF.Sin(angleInRadians), MathF.Cos(angleInRadians));
        }
    }
}
