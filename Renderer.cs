using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace raylibRaycastFPSnew
{
    class Renderer
    {
        public void RenderLevel(Player player, Level level, Window window, int stripWidth, Color ceilingColor, Color floorColor)
        {
            DrawCeilingAndFloor(window, ceilingColor, floorColor);

            float rayAngleStep = stripWidth * player.fov / window.width;
            int amountOfTimesToIncreaseRayAngle = (int)(player.fov / rayAngleStep);

            for (int i = 0; i < amountOfTimesToIncreaseRayAngle; i++)
            {
                float rayAngle = player.lookAngle - player.fov / 2 + rayAngleStep * i;
                LineSegment ray = CastRay(Trigonometry.GetRectangleCenter(player.rectangle), Trigonometry.GetDirectionFromAngle(rayAngle));

                Vector2 closestIntersectionPoint;
                Wall closestIntersectedWall;
                (closestIntersectionPoint, closestIntersectedWall) = GetRayIntersectionInfo(level, ray);
                
                float distanceToClosestIntersectionPoint = Trigonometry.GetPointToPointRealDistance(ray.startPosition, closestIntersectionPoint);
                DrawWallPart(window, stripWidth, i, closestIntersectedWall, distanceToClosestIntersectionPoint);
            }

        }

        private void DrawWallPart(Window window, int stripWidth, int i, Wall closestIntersectedWall, float distanceToClosestIntersectionPoint)
        {
            int stripHeight = (int)(window.height / distanceToClosestIntersectionPoint * closestIntersectedWall.height);
            int stripX = i * stripWidth;
            int stripY = (window.height - stripHeight) / 2;
            Color stripColor = closestIntersectedWall.color;
            Raylib.DrawRectangle(stripX, stripY, stripWidth, stripHeight, stripColor);
        }

        private void DrawCeilingAndFloor(Window window, Color ceilingColor, Color floorColor)
        {
            Raylib.DrawRectangle(0, 0, window.width, window.height / 2, ceilingColor);
            Raylib.DrawRectangle(0, window.height / 2, window.width, window.height / 2, floorColor);
        }

        private (Vector2, Wall) GetRayIntersectionInfo(Level level, LineSegment ray)
        {
            // finding (all intersection points between ray and walls) and (walls that ray intersected)
            Dictionary<Vector2, Wall> rayIntersectionPointsAndIntersectedWalls = new Dictionary<Vector2, Wall>();
            foreach (Wall wall in level.walls)
            {
                var intersectionInfo = GetLineLineIntersection(ray, wall.lineSegment);
                if (intersectionInfo.Item1)
                {
                    rayIntersectionPointsAndIntersectedWalls.Add(intersectionInfo.Item2, wall); // TODO: fix argument exception that sometimes appears
                }
            }

            // finding the closest intersection point and closest intersected wall if intersection points exist
            Vector2 closestPoint = default;
            Wall closestWall = default;
            if (rayIntersectionPointsAndIntersectedWalls.Count > 0)
            {
                float minDistanceToPoint = float.MaxValue;

                foreach (var pointWallPair in rayIntersectionPointsAndIntersectedWalls)
                {
                    float squaredDistanceToPoint = Trigonometry.GetPointToPointSquareDistance(ray.startPosition, pointWallPair.Key);
                    if (squaredDistanceToPoint < minDistanceToPoint)
                    {
                        minDistanceToPoint = squaredDistanceToPoint;
                        closestPoint = pointWallPair.Key;
                        closestWall = pointWallPair.Value;
                    }
                }
            }
            return (closestPoint, closestWall);
        }

        private LineSegment CastRay(Vector2 rayStartPosition, Vector2 rayDirection)
        {
            GetLineLineIntersection(new LineSegment(new Vector2(), new Vector2()), new LineSegment(new Vector2(), new Vector2()));
            return new LineSegment(rayStartPosition, rayStartPosition + rayDirection * 3000f);
        }

        private (bool, Vector2) GetLineLineIntersection(LineSegment line1, LineSegment line2)
        {
            // formula translating from wikipedia

            float x1 = line1.startPosition.X;
            float y1 = line1.startPosition.Y;
            float x2 = line1.endPosition.X;
            float y2 = line1.endPosition.Y;

            float x3 = line2.startPosition.X;
            float y3 = line2.startPosition.Y;
            float x4 = line2.endPosition.X;
            float y4 = line2.endPosition.Y;

            float Denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (Denominator == 0) // line segments are parallel
            {
                return (false, new Vector2()); 
            }

            float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / Denominator;
            float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / Denominator;

            if ((t >= 0 && t <= 1) && (u >= 0 && u <= 1))
            {
                // there is an intersectionz`
                float intersectionX = x1 + t * (x2 - x1);
                float intersectionY = y1 + t * (y2 - y1);
                Vector2 intersection = new Vector2(intersectionX, intersectionY);
                return (true, intersection);
            }
            else
            {
                return (false, new Vector2()); // no intersection
            }
        }


    }
}
