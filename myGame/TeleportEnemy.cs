using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame;

namespace myGame
{
    public class TeleportEnemy : Enemy
    {
        public Vector2 left;
        public Vector2 right;
        public float teleportInterval; // in seconds
        private float timer;
        private bool onLeft;

        public TeleportEnemy(Vector2 left, Vector2 right, float interval, int degrees)
            : base((int)left.X, (int)left.Y, degrees, "spike")
        {
            this.left = left;
            this.right = right;
            teleportInterval = interval;

            localPosition = left;
            onLeft = true;
            timer = 0f;

            name = "teleport enemy";
        }

        public override void update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= teleportInterval)
            {
                // teleport
                if (onLeft)
                {
                    localPosition = right;
                }
                else
                {
                    localPosition = left;
                }
                onLeft = !onLeft;
                timer = 0f;
            }
        }
    }
}
