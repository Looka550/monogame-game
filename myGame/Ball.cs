using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace myGame
{
    public class Ball : GameObject
    {
        Vector2 velocity;

        public Ball(float x, float y)
            : base(x, y, "ball_down", Color.White)
        {
            velocity = new Vector2(120f, 0f);
        }

        public override void update(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
