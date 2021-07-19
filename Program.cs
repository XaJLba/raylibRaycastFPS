using System;
using System.Numerics;
using Raylib_cs;

namespace raylibRaycastFPSnew
{
    class Program
    {
        static void Main()
        {
            // doom2 window resolution is 640 * 480
            // doom2 screen resolution is 640 * 340
            // doom2 fps is 35
            const int ScreenWidth = 640;
            const int ScreenHeight = 480;
            const string Caption = "Intersections!";
            const int Fps = 60;

            const float PlayerSideLength = 20f;
            const float PlayerWalkingSpeed = 200f;
            const float PlayerTurningSpeed = 1f;
            const float PlayerFov = MathF.PI / 3;
            const float PlayerWalkingSprintFactor = 1.6f;
            const float PlayerTurningSprintFactor = 1.8f;

            const int StripWidth = 2;

            Window window = new Window(ScreenWidth, ScreenHeight, Caption, Fps);
            Player player = new Player(new Vector2(100, 100), PlayerSideLength, PlayerWalkingSpeed, PlayerTurningSpeed, PlayerFov, PlayerWalkingSprintFactor, PlayerTurningSprintFactor);
            Renderer renderer = new Renderer();

            Level level = new Level(new Wall[5]
            {
                new Wall(new Vector2(388, 424), new Vector2(354, 248), 125f, Color.RAYWHITE),
                new Wall(new Vector2(354, 248), new Vector2(522, 201), 125f, Color.MAROON),
                new Wall(new Vector2(522, 201), new Vector2(148, 112), 125f, Color.RAYWHITE),
                new Wall(new Vector2(148, 112), new Vector2(223, 253), 125f, Color.GREEN),
                new Wall(new Vector2(223, 253), new Vector2(388, 424), 125f, Color.MAROON),

            }); 

            while (!Raylib.WindowShouldClose())
            {
                window.HandleWindowClosing();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                player.Sprint();
                player.Rotate();
                player.WalkForwardsAndBackwards();
                player.WalkStrafe();

                renderer.RenderLevel(player, level, window, StripWidth, Color.SKYBLUE, Color.BEIGE);

                Debug(level, player);

                Raylib.EndDrawing();
            }
        }

        static void Debug(Level level, Player player)
        {
/*            foreach (Wall wall in level.walls)
            {
                Raylib.DrawLineV(wall.lineSegment.startPosition, wall.lineSegment.endPosition, wall.color);
            }
            Raylib.DrawRectangleRec(player.rectangle, Color.MAGENTA);
            Raylib.DrawText($"look angle in radians: {player.lookAngleInRadians}", 0, 20, 20, Color.GREEN);*/
            Raylib.DrawFPS(0, 0);
        }
    }
}
