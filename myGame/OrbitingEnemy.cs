using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame;

namespace myGame
{
    public class OrbitingEnemy : Enemy
    {
        public Vector2 center;
        public float radius;
        public float angle; // radians
        public float speed;
        bool clockwise = true;

        public OrbitingEnemy(Vector2 center, float radius, float speed, bool clockwise)
            : base((int)(center.X + radius), (int)center.Y, "spike", new Vector2(1f, 1f))
        {
            this.center = center;
            this.radius = radius;
            this.speed = speed;
            this.clockwise = clockwise;
            angle = 0f;

            name = "orbiting enemy";
        }

        public override void update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (clockwise)
            {
                angle += speed * dt;
            }
            else
            {
                angle -= speed * dt;
            }

            if (angle > MathHelper.TwoPi)
            {
                angle -= MathHelper.TwoPi;
            }
            if (angle < 0)
            {
                angle += MathHelper.TwoPi;
            }

            localPosition.X = center.X + radius * (float)Math.Cos(angle);
            localPosition.Y = center.Y + radius * (float)Math.Sin(angle);

            rotation = gameTime.TotalGameTime.Milliseconds / 40;
        }

    }
}
