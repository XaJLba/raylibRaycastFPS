using Raylib_cs;
using System;
using System.Numerics;

namespace raylibRaycastFPSnew
{
    class Player : Entity
    {
        public float walkingSpeed { get; private set; }
        public float turningSpeed { get; private set; }
        public float lookAngle { get; private set; }
        public float fov { get; }
        private float _walkingSprintFactor { get; }
        private float _turningSprintFactor { get; }
        public Player(Vector2 position, float sideLength, float walkingSpeed, float turningSpeed, float fov, float walkingSprintFactor, float turningSprintFactor)
        {
            rectangle = new Rectangle(position.X, position.Y, sideLength, sideLength);
            this.walkingSpeed = walkingSpeed;
            this.turningSpeed = turningSpeed;
            lookAngle = 0f;
            this.fov = fov;
            _walkingSprintFactor = walkingSprintFactor;
            _turningSprintFactor = turningSprintFactor;
        }
        public void Sprint()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT_SHIFT))
            {
                walkingSpeed *= _walkingSprintFactor;
                turningSpeed *= _turningSprintFactor;
            }
            if (Raylib.IsKeyReleased(KeyboardKey.KEY_LEFT_SHIFT))
            {
                walkingSpeed /= _walkingSprintFactor;
                turningSpeed /= _turningSprintFactor;
            }
        }

        public void WalkForwardsAndBackwards()
        {
            Vector2 direction;

            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            {
                direction = new Vector2(MathF.Cos(lookAngle), MathF.Sin(lookAngle));
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            {
                direction = new Vector2(-MathF.Cos(lookAngle), -MathF.Sin(lookAngle));
            }

            else
            {
                return; // not interested in other key presses
            }

            // update position
            rectangle = new Rectangle(rectangle.x + walkingSpeed * direction.X * Raylib.GetFrameTime(), rectangle.y + walkingSpeed * direction.Y * Raylib.GetFrameTime(), rectangle.width, rectangle.width);
        }

        public void WalkStrafe()
        {
            Vector2 direction;

            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                direction = new Vector2(-MathF.Sin(lookAngle), MathF.Cos(lookAngle));
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                direction = new Vector2(MathF.Sin(lookAngle), -MathF.Cos(lookAngle));
            }

            else
            {
                return; // not interested in other key presses
            }

            // update position
            rectangle = new Rectangle(rectangle.x + walkingSpeed * direction.X * Raylib.GetFrameTime(), rectangle.y + walkingSpeed * direction.Y * Raylib.GetFrameTime(), rectangle.width, rectangle.width);

        }

        public void Rotate()
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                lookAngle -= turningSpeed * Raylib.GetFrameTime();
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                lookAngle += turningSpeed * Raylib.GetFrameTime();
            }
            lookAngle = Trigonometry.FixAngle(lookAngle);
        }

    }
}
