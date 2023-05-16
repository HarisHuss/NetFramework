using Raylib_cs;
using System.Numerics;


namespace Spaceinvaders
{
    internal class Enemy
    {
        public Transform transform;

        public Vector2 position;
        public int health;

        public bool Alive = true;

        public List<Bullets> bullets;
        private float bulletCooldown = 0;
        private float bulletCooldownMax = 12000;


        private Texture2D texture;


        private float speed;
        private float size;

        private List<Player> players;

        public Enemy(Vector2 position, Vector2 direction, int health, string texturePath, List<Player> players)
        {
            this.position = position;
            this.health = health;

            this.bullets = new List<Bullets>();
            this.players = players;

            texture = Raylib.LoadTexture(texturePath);

            this.transform = new Transform(position, direction, speed, size);


            bulletCooldownMax = Raylib.GetRandomValue(5000, 15000);
        }

        public void Update(Player player)
        {

            if (health <= 0)
            {
                Alive = false;
                player.score += 10;
            }


            bulletCooldown -= Raylib.GetFrameTime() * 1000;

            if (bulletCooldown <= 0)
            {

                bullets.Add(new Bullets(new Vector2(position.X, position.Y + 20), new Vector2(0, 1)));
                bulletCooldown = bulletCooldownMax;
            }


            foreach (Bullets bullet in bullets.ToList())
            {
                bullet.Update();

                foreach (Player otherPlayer in players)
                {
                    if (Raylib.CheckCollisionCircles(bullet.position, 5, player.position, 40))
                    {

                        player.pHealth -= 10;

                        bullets.Remove(bullet);
                        break;
                    }
                }
            }


            bullets.RemoveAll(bullet => bullet.position.Y < 0 || bullet.position.Y > Raylib.GetScreenHeight());
        }

        public void Draw()
        {
            Raylib.DrawTextureEx(texture, position, 0f, 0.1f, Raylib_cs.Color.WHITE);

            foreach (Bullets bullet in bullets)
                bullet.Draw();
        }
    }
}