using Raylib_cs;
using System;
using System.Numerics;

namespace raylibRaycastFPSnew
{
    class Player : Entity
    {
        public float walkingSpeed { get; private set; }
        public float turningSpeedInRadians { get; private set; }
        public float lookAngleInRadians { get; private set; }
        public float fovInRadians { get; }
        private float _walkingSprintFactor { get; }
        private float _turningSprintFactor { get; }
        public Player(Vector2 position, float sideLength, float walkingSpeed, float turningSpeedInRadians, float fovInRadians, float walkingSprintFactor, float turningSprintFactor)
        {
            rectangle = new Rectangle(position.X, position.Y, sideLength, sideLength);
            this.walkingSpeed = walkingSpeed;
            this.turningSpeedInRadians = turningSpeedInRadians;
            lookAngleInRadians = 0f;
            this.fovInRadians = fovInRadians;
            _walkingSprintFactor = walkingSprintFactor;
            _turningSprintFactor = turningSprintFactor;
        }
        public void Sprint()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT_SHIFT))
            {
                walkingSpeed *= _walkingSprintFactor;
                turningSpeedInRadians *= _turningSprintFactor;
            }
            if (Raylib.IsKeyReleased(KeyboardKey.KEY_LEFT_SHIFT))
            {
                walkingSpeed /= _walkingSprintFactor;
                turningSpeedInRadians /= _turningSprintFactor;
            }
        }

        public void WalkForwardsAndBackwards()
        {
            Vector2 direction;

            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            {
                direction = new Vector2(MathF.Cos(lookAngleInRadians), MathF.Sin(lookAngleInRadians));
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            {
                direction = new Vector2(-MathF.Cos(lookAngleInRadians), -MathF.Sin(lookAngleInRadians));
            }

            else
            {
                return;
            }

            rectangle = new Rectangle(rectangle.x + walkingSpeed * direction.X * Raylib.GetFrameTime(), rectangle.y + walkingSpeed * direction.Y * Raylib.GetFrameTime(), rectangle.width, rectangle.width);
        }

        public void WalkStrafe()
        {
            Vector2 direction;

            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                direction = new Vector2(-MathF.Sin(lookAngleInRadians), MathF.Cos(lookAngleInRadians));
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                direction = new Vector2(MathF.Sin(lookAngleInRadians), -MathF.Cos(lookAngleInRadians));
            }

            else
            {
                return;
            }

            rectangle = new Rectangle(rectangle.x + walkingSpeed * direction.X * Raylib.GetFrameTime(), rectangle.y + walkingSpeed * direction.Y * Raylib.GetFrameTime(), rectangle.width, rectangle.width);

        }

        public void Rotate()
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                lookAngleInRadians -= turningSpeedInRadians * Raylib.GetFrameTime();
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                lookAngleInRadians += turningSpeedInRadians * Raylib.GetFrameTime();
            }
            lookAngleInRadians = Geometry.FixAngle(lookAngleInRadians);
        }

    }
}
