using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame;

namespace myGame
{
    public class MovingEnemy : Enemy
    {
        public Vector2 pointA;
        public Vector2 pointB;
        public float speed;
        bool direction;

        public MovingEnemy(Vector2 pointA, Vector2 pointB, float speed, int degrees)
            : base((int)pointA.X, (int)pointA.Y, degrees, "spike")
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.speed = speed;

            localPosition = pointA;
            direction = true;

            name = "moving enemy";
        }

        public override void update(GameTime gameTime)
        {
            if (direction)
            {
                localPosition += Vector2.Normalize(pointB - localPosition) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Vector2.Distance(localPosition, pointB) < 4f)
                {
                    direction = !direction;
                }
            }
            else
            {
                localPosition += Vector2.Normalize(pointA - localPosition) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Vector2.Distance(localPosition, pointA) < 4f)
                {
                    direction = !direction;
                }
            }
        }
    }
}
