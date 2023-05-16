using System.Numerics;
using Raylib_cs;

namespace Spaceinvaders
{
    public class Transform
    {
        public float speed;
        public float size;
        public Vector2 position;
        public Vector2 direction;


        public Transform(Vector2 position, Vector2 direction, float speed, float size)
        {
            this.speed = speed;
            this.size = size;
            this.position = position;
            this.direction = direction;

        }
    }
}


namespace Spaceinvaders
{
    internal class Bullets
    {
        public Vector2 position;
        public Vector2 velocity;

        public Bullets(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity / 1f;
        }

        public void Update()
        {
            position += velocity;
        }

        public void Draw()
        {
            Raylib.DrawCircle((int)position.X, (int)position.Y, 5, Raylib_cs.Color.RED);
        }

    }
}