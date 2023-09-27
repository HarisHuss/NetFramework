
using Raylib_CsLo;
using Spaceinvader;
using Spaceinvaders;
using System;
using System.Numerics;
using static SpaceInvaders.Program;

namespace SpaceInvaders
{
    internal class invaders
    {
        public static int Enemies = 25;

        const int screenWidth = 800;
        const int screenHeight = 1000;

        Player player;
        List<Bullets> bullets;
        List<Enemy> enemies;

        StartScreen startScreen;
        Developer developer;

        public bool moveRight = true;
        public static bool shouldChangeDirection = false;
        public bool moveDown = false;
        public float speed = 0.2f;

        enum GameState { Playing, Win, Lose, Start, Paused, Developer};
        Stack<GameState> gameState = new Stack<GameState>();

        private TimeSpan playingTime;
        private DateTime levelStartTime;
        private bool restartLevelRequested = false;

        private float volume = 0.5f;
        private bool isMusicOn = true;
        private int difficultyLevel = 3;
        private bool settingsMenuActive = false;
        bool playerInvincible = false;



        void init()
        {
            gameState.Push(GameState.Start);

            startScreen = new StartScreen();
            developer = new Developer();

            Raylib.InitWindow(screenWidth, screenHeight, "Space Invaders");

            Raylib.InitAudioDevice();

            Raylib.SetExitKey(KeyboardKey.KEY_BACKSPACE);

            Raylib.SetTargetFPS(250);
        }

        void Reset()
        {
            float playerSpeed = 120;
            int playerSize = 40;
            int pHealth = 40;
            Vector2 playerStart = new Vector2(screenWidth / 2, screenHeight - playerSize * 2);

            player = new Player(playerStart, new Vector2(playerSize, playerSize), playerSpeed, pHealth);

            bullets = new List<Bullets>();

            enemies = new List<Enemy>();

            for (int i = 0; i < Enemies; i++)
            {
                int row = i / 5;
                int col = i % 5;

                Vector2 position = new Vector2(col * 100 + 100, row * 100 + 100);
                Enemy enemy = new Enemy(position, new Vector2(1, 0), 50, "Enemy.png", new List<Player> { player });

                enemies.Add(enemy);
            }

        }


        public void GameLoop()
        {
            init();
            Reset();

            while (!Raylib.WindowShouldClose())
            {
                switch (gameState.Peek())
                {
                    case GameState.Start:
                        if (startScreen.Update())
                        {
                            gameState.Push(GameState.Playing);
                        }

                        break;
                    case GameState.Playing:
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                        {
                            gameState.Push(GameState.Paused);
                            levelStartTime = DateTime.Now - playingTime;
                        }
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_M))
                        {
                            #if DEBUG

                            gameState.Push(GameState.Developer);

                            #endif
                        }
                        else 
                        {
                            UpdateEnemies();
                            player.Update(enemies);
                            foreach (Enemy enemy in enemies.ToList())
                            {
                                enemy.Update(player, playerInvincible);
                            }
                        }
                        break;
                    case GameState.Win:
                        break;
                    case GameState.Lose:
                        drawGameOver();
                        break;

                    case GameState.Paused:
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                        {
                            gameState.Push(GameState.Playing);
                            playingTime = DateTime.Now - levelStartTime;
                        }

                        if (RayGui.GuiButton(new Rectangle(300, 400, 200, 50), "Restart Level"))
                        {
                            restartLevelRequested = true;
                        }

                        Raylib.DrawText($"Playing Time: {playingTime:mm\\:ss}", 300, 300, 30, Raylib.WHITE);

                        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), new Rectangle(300, 400, 200, 50)) && Raylib.IsMouseButtonReleased(0))
                        {
                            restartLevelRequested = true;
                        }

                        if (restartLevelRequested)
                        {
                            Reset();
                            gameState.Push(GameState.Playing);
                            restartLevelRequested = false;
                        }

                        if (RayGui.GuiButton(new Rectangle(300, 500, 200, 50), "Settings"))
                        {
                            settingsMenuActive = !settingsMenuActive;

                        }

                        if (settingsMenuActive)
                        {
                            DrawSettingsMenu();
                        }

                        break;

                        if (player.pHealth <= 0)
                        {
                            gameState.Push(GameState.Lose);
                        }

                    case GameState.Developer:
                        developer.Draw();

                        if (developer.IsBackPressed())
                        {
                            playerInvincible = developer.IsInvincibilityToggled();

                            gameState.Pop();
                        }
                        

                        break;


                }


                Raylib.BeginDrawing();

                Raylib.ClearBackground(Raylib.BLACK);

                switch (gameState.Peek())
                {
                    case GameState.Start:
                        startScreen.Draw();
                        break;
                    case GameState.Playing:
                        drawGameStart();
                        if (player.pHealth <= 0 || enemies.Count == 0)
                        {
                            gameState.Push(player.pHealth <= 0 ? GameState.Lose : GameState.Win);
                        }
                        break;
                    case GameState.Win:
                        drawGameOver();
                        break;
                    case GameState.Lose:
                        drawGameOver();
                        break;
                }


                Raylib.EndDrawing();



            }
        }


        void drawGameOver()
        {
            if (gameState.Peek() == GameState.Lose)
            {
                Raylib.ClearBackground(Raylib_CsLo.Raylib.RED);
                Raylib.DrawText("Game Over!", 300, 500, 50, Raylib_CsLo.Raylib.BLACK);
            }
            else if (gameState.Peek() == GameState.Win)
            {
                Raylib.ClearBackground(Raylib_CsLo.Raylib.LIME);
                Raylib.DrawText("You Win!", 300, 500, 50, Raylib_CsLo.Raylib.BLACK);
            }
            Raylib.DrawText("Press ENTER to start again", 270, 600, 30, Raylib_CsLo.Raylib.BLACK);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                Reset();
                gameState.Pop();
            }
        }

        void drawGameStart()
        {
            Raylib.ClearBackground(Raylib_CsLo.Raylib.WHITE);
            player.Draw();
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw();
            }
            player.Score();
        }

        void UpdateEnemies()
        {
            bool wallHit = false;
            float enemySpeed = speed;

            foreach (Enemy enemy in enemies)
            {
                enemy.position.X += enemySpeed * enemy.transform.direction.X;

                if (enemy.position.X - enemy.transform.size / 2 <= 0 || enemy.position.X + enemy.transform.size / 2 >= screenWidth)
                {
                    wallHit = true;
                }
            }

            if (wallHit)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.transform.direction.X *= -1.0f;
                    enemy.position.Y += 10;
                }
            }

            if (enemies.Count > 0 && moveRight && enemies.Last().position.X >= screenWidth - 20)
            {
                moveRight = false;
                shouldChangeDirection = true;
            }
            else if (enemies.Count > 0 && !moveRight && enemies.First().position.X <= 20)
            {
                moveRight = true;
                shouldChangeDirection = true;
            }

            if (shouldChangeDirection)
            {
                speed *= -1;
                shouldChangeDirection = false;
                moveDown = true;
                foreach (Enemy enemy in enemies)
                {
                    enemy.transform.direction.X *= -1.0f;
                    enemy.position.Y += 10;
                }
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

