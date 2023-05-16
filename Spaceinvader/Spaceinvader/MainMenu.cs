using Raylib_cs;
using Spaceinvaders;

namespace SpaceInvaders
{
    class StartScreen
    {
        public StartScreen()
        {
            Raylib.SetTargetFPS(60);
        }

        public void Draw()
        {
            Raylib.ClearBackground(Color.BLACK);
            Raylib.DrawText("Space Invaders", 65, 350, 100, Color.RED);
            Raylib.DrawText("Press SPACE to start", 225, 550, 40, Color.GREEN);
        }

        public bool Update()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}