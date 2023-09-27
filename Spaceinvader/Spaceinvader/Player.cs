using Raylib_CsLo;
using System.Numerics;

namespace Spaceinvaders
{

    internal class Player
    {
        public static int EnemiesAmount = 5;


        public int pHealth;
        public float pSpeed;
        public Vector2 position;
        public Vector2 size;

        public Sound playerHit;
        public Sound shootingBeam;


        public List<Bullets> bullets;

        public Texture texture;

        public int score = 0;

        Sound sound = Raylib.LoadSound("Shoot.wav");

        private int playerStart;
        private Vector2 vector2;
        private float playerSpeed;

        bool useMouseControl = true;


        public Player(Vector2 position, Vector2 size, float speed, int health)
        {
            this.position = position;
            this.pSpeed = speed / 50;
            this.pHealth = health;
            this.bullets = new List<Bullets>();
            this.size = size;
            this.texture = Raylib.LoadTexture("player.png");
            this.size.X = texture.width * 0.1f;
            this.size.Y = texture.height * 0.1f;
        }

        public Player(int playerStart, Vector2 vector2, float playerSpeed, int pHealth)
        {
            this.playerStart = playerStart;
            this.vector2 = vector2;
            this.playerSpeed = playerSpeed;
            this.pHealth = pHealth;
        }

        public void hitPlay()
        {
            Raylib.PlaySound(playerHit);
        }
        public void Score()
        {
            Raylib.DrawText($"Points: {score}", Raylib.GetScreenWidth() - 150, 20, 20, Raylib_CsLo.Raylib.BLACK);
        }


        public void Update(List<Enemy> enemies)
        {

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                bullets.Add(new Bullets(new Vector2(position.X, position.Y - 20), new Vector2(0, -5)));
                Raylib.PlaySound(sound);
            }

            

            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
            {
                useMouseControl = !useMouseControl;
            }   

           if (useMouseControl)
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        if (mousePosition.X < position.X && position.X > 20)
        {
            position.X -= pSpeed;
        }
        else if (mousePosition.X > position.X && position.X < Raylib.GetScreenWidth() - 20)
        {
            position.X += pSpeed;
        }
    }
    else
    {
                if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) && position.X < Raylib.GetScreenWidth() - 20)
                    position.X += pSpeed;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) && position.X > 20)
                    position.X -= pSpeed;
            }

             
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    if (Raylib.CheckCollisionCircles(bullets[i].position, 5, enemies[j].position, 20))
                    {

                        enemies.RemoveAt(j);
                        bullets.RemoveAt(i);

                        EnemiesAmount--;

                        score += 10;

                        break;
                    }
                }


                for (int j = bullets.Count - 1; j >= 0; j--)
                {
                    if (bullets[j].position.Y < 0 || bullets[j].position.Y > Raylib.GetScreenHeight())
                    {
                        bullets.RemoveAt(j);
                    }
                }

            }
        }
        public void Draw()
        {
            Raylib.DrawTextureEx(texture, position, 0f, 0.1f, Raylib_CsLo.Raylib.WHITE);
            Raylib.DrawRectangleLines((int)position.X, (int)position.Y, (int)size.X, (int)size.Y,  Raylib_CsLo.Raylib.RED);

            foreach (Bullets bullet in bullets)
                bullet.Draw();

            Raylib.DrawText("Health: " + pHealth, 10, 10, 20, Raylib_CsLo.Raylib.GREEN);
        }
    }
}