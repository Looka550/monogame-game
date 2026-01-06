using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class Ball : GameObject
    {
        public Vector2 velocity;

        public Ball(float x, float y)
            : base(x, y, "ball_down", Color.White)
        {
            velocity = new Vector2(120f, 0f);
        }

        public override void update(GameTime gameTime)
        {

        }

        public override void onKeyPressed(Keys key)
        {
            Console.WriteLine($"clicked: {key}");
        }

        public override void onKeyReleased(Keys key)
        {
            Console.WriteLine($"released: {key}");
        }
    }
}
