
using Raylib_cs;
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

        public bool moveRight = true;
        public static bool shouldChangeDirection = false;
        public bool moveDown = false;
        public float speed = 0.2f;

        enum GameState { Playing, Win, Lose, Start };
        GameState gameState = GameState.Start;

        void init()
        {
            startScreen = new StartScreen();

            Raylib.InitWindow(screenWidth, screenHeight, "Space Invaders");

            Raylib.InitAudioDevice();


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
                switch (gameState)
                {
                    case GameState.Start:
                        if (startScreen.Update())
                        {
                            gameState = GameState.Playing;
                        }
                       
                        break;
                    case GameState.Playing:
                        UpdateEnemies();
                        player.Update(enemies);
                        foreach (Enemy enemy in enemies.ToList())
                        {
                            enemy.Update(player);
                        }
                        break;
                    case GameState.Win:
                        break;
                    case GameState.Lose:
                        break;
                }

                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.BLACK);

                switch (gameState)
                {
                    case GameState.Start:
                        startScreen.Draw();
                        break;
                    case GameState.Playing:
                        drawGameStart();
                        break;
                    case GameState.Win:
                        drawGameOver();
                        break;
                    case GameState.Lose:
                        drawGameOver();
                        break;
                }
               

                Raylib.EndDrawing();

                if (player.pHealth <= 0 || enemies.Count == 0)
                {
                    gameState = player.pHealth <= 0 ? GameState.Lose : GameState.Win;
                }

                
            }
        }


        void drawGameOver()
        {
            if (gameState == GameState.Lose)
            {
                Raylib.ClearBackground(Raylib_cs.Color.RED);
                Raylib.DrawText("Game Over!", 300, 500, 50, Raylib_cs.Color.BLACK);
            }
            else if (gameState == GameState.Win)
            {
                Raylib.ClearBackground(Raylib_cs.Color.LIME);
                Raylib.DrawText("You Win!", 300, 500, 50, Raylib_cs.Color.BLACK);
            }
            Raylib.DrawText("Press ENTER to start again", 270, 600, 30, Raylib_cs.Color.BLACK);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                Reset();
                gameState = GameState.Playing;
            }
        }

        void drawGameStart()
        {
            Raylib.ClearBackground(Raylib_cs.Color.WHITE);
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
    }
}

