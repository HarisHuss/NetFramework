using Raylib_CsLo;
using Spaceinvaders;

namespace SpaceInvaders
{
    class StartScreen
    {

        private bool settingsMenuActive = false;
        private bool playButtonPressed;
        public StartScreen()
        {
            Raylib.SetTargetFPS(60);
        }

        public void Draw()
        {
            Raylib.DrawText("Move: W, A, D", 300, 600, 30, Raylib.WHITE);
            Raylib.DrawText("Shoot: Left Mouse Button", 300, 650, 30, Raylib.WHITE);
            Raylib.DrawText("Toggle Settings: ESC", 300, 700, 30, Raylib.WHITE);
            Raylib.DrawText("Exit Game: Backspace", 300, 750, 30, Raylib.WHITE);
            Raylib.DrawText("Developer Menu: M", 300, 800, 30, Raylib.WHITE);


            if (RayGui.GuiButton(new Rectangle(300, 300, 200, 50), "Play"))
            {
                playButtonPressed = true;
            }
            else
            {
                playButtonPressed = false;
            }


            if (RayGui.GuiButton(new Rectangle(300, 500, 200, 50), "Exit"))
            {
                Environment.Exit(0);
            }
            


            if (RayGui.GuiButton(new Rectangle(300, 400, 200, 50), "Settings"))
            {
                settingsMenuActive = !settingsMenuActive;
            }

            if (settingsMenuActive)
            {
                DrawSettingsMenu();
            }

            Raylib.ClearBackground(Raylib.BLACK);
            Raylib.DrawText("Space Invaders", 25, 35, 95, Raylib.RED);


            
        }

        public bool Update()
        {
            if (playButtonPressed)
            {
                playButtonPressed = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DrawSettingsMenu()
        {
            Raylib.DrawRectangle(200, 300, 400, 400, Raylib_CsLo.Raylib.LIGHTGRAY);
            Raylib.DrawText("Settings", 345, 320, 30, Raylib_CsLo.Raylib.DARKGRAY);

            Rectangle backButton = new Rectangle(300, 420, 100, 50);
            bool backButtonHovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), backButton);

            if (RayGui.GuiButton(new Rectangle(300, 600, 200, 50), "Back"))
            {
                settingsMenuActive = false;
            }

        }
    }


}