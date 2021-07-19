using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace raylibRaycastFPSnew
{
    class Renderer
    {
        public void RenderLevel(Player player, Level level, Window window, int stripWidth, Color ceilingColor, Color floorColor) // TODO: abstraction
        {
            // draw ceiling and floor
            Raylib.DrawRectangle(0, 0, window.width, window.height / 2, ceilingColor);
            Raylib.DrawRectangle(0, window.height / 2, window.width, window.height / 2, floorColor);

            Vector2 playerCenter = new Vector2(player.rectangle.x + player.rectangle.width / 2, player.rectangle.y + player.rectangle.height / 2);

            Vector2 rayStartPosition = playerCenter;
            float rayDistance = 1000f;

            float rayAngleStepInRadians = stripWidth * player.fovInRadians / window.width;
            int amountOfTimesToIncreaseRayAngle = (int)(player.fovInRadians / rayAngleStepInRadians);
            float initialRayAngleInRadians = player.lookAngleInRadians - player.fovInRadians / 2;
            float rayAngleInRadians;

            for (int i = 0; i < amountOfTimesToIncreaseRayAngle; i++)
            {
                // defining ray direction
                rayAngleInRadians = initialRayAngleInRadians + rayAngleStepInRadians * i;
                Vector2 rayDirection = new Vector2(MathF.Cos(rayAngleInRadians), MathF.Sin(rayAngleInRadians));

                // casting ray
                Vector2 rayNewPosition = rayStartPosition + rayDirection * rayDistance;
                LineSegment ray = new LineSegment(rayStartPosition, rayNewPosition);

                // finding all intersection points between ray and walls and walls that ray intersected
                Dictionary<Vector2, Wall> rayIntersectionPointsAndIntersectedWalls = new Dictionary<Vector2, Wall>();
                foreach (Wall wall in level.walls)
                {
                    var intersectionInfo = GetLineLineIntersection(ray, wall.lineSegment);
                    if (intersectionInfo.Item1)
                    {
                        rayIntersectionPointsAndIntersectedWalls.Add(intersectionInfo.Item2, wall); // TODO: fix argument exception that sometimes appears
                    }
                }

                // finding the closest intersection point if intersection points exist
                if (rayIntersectionPointsAndIntersectedWalls.Count > 0)
                {
                    float minDistanceToPoint = float.MaxValue;
                    Vector2 closestPoint = default;
                    Wall closestWall = default;

                    foreach (var pointWallPair in rayIntersectionPointsAndIntersectedWalls)
                    {
                        float squaredDistanceToPoint = GetPointToPointSquareDistance(rayStartPosition, pointWallPair.Key);
                        if (squaredDistanceToPoint < minDistanceToPoint)
                        {
                            minDistanceToPoint = squaredDistanceToPoint;
                            closestPoint = pointWallPair.Key;
                            closestWall = pointWallPair.Value;
                        }
                    }
                    float distanceToClosestPoint = MathF.Sqrt(GetPointToPointSquareDistance(rayStartPosition, closestPoint));
                    
                    // fix fisheye effect
                    float angleDifferenceInRadians = player.lookAngleInRadians - rayAngleInRadians;
                    if (angleDifferenceInRadians < 0)
                    {
                        angleDifferenceInRadians = 6.28318530f; // 6.28318530f = PI * 2
                    }
                    if (angleDifferenceInRadians > 6.28318530f) // 6.28318530f = PI * 2
                    {
                        angleDifferenceInRadians = 0;
                    }
                    distanceToClosestPoint *= MathF.Cos(angleDifferenceInRadians);

                    // defining the strip to draw
                    int stripHeight = (int)(window.height / distanceToClosestPoint * closestWall.height);
                    int stripX = i * stripWidth;
                    int stripY = (window.height - stripHeight) / 2;
                    Color stripColor = closestWall.color;
                    // drawing the strip
                    Raylib.DrawRectangle(stripX, stripY, stripWidth, stripHeight, stripColor);
                }
            }

        }

        (bool, Vector2) GetLineLineIntersection(LineSegment line1, LineSegment line2)
        {
            float x1 = line1.startPosition.X;
            float y1 = line1.startPosition.Y;
            float x2 = line1.endPosition.X;
            float y2 = line1.endPosition.Y;

            float x3 = line2.startPosition.X;
            float y3 = line2.startPosition.Y;
            float x4 = line2.endPosition.X;
            float y4 = line2.endPosition.Y;

            float D = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (D == 0)
            {
                return (false, new Vector2());
            }

            float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / D;
            float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / D;

            if ((t >= 0 && t <= 1) && (u >= 0 && u <= 1))
            {
                float intersectionX = x1 + t * (x2 - x1);
                float intersectionY = y1 + t * (y2 - y1);
                Vector2 intersection = new Vector2(intersectionX, intersectionY);
                return (true, intersection);
            }
            else
            {
                return (false, new Vector2());
            }
        }

        float GetPointToPointSquareDistance(Vector2 p1, Vector2 p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }
    }
}
