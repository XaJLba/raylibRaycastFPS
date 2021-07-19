using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace raylibRaycastFPSnew
{
    class Window
    {
        public int width { get; }
        public int height { get; }
        public Window(int width, int height, string caption, int fps)
        {
            Raylib.InitWindow(width, height, caption);
            this.width = width;
            this.height = height;
            Raylib.SetTargetFPS(fps);
        }

        public void HandleWindowClosing()
        {
            if (Raylib.WindowShouldClose())
            {
                Raylib.CloseWindow();
            }
        }
    }
}
