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
        public bool gravity = true;

        public Ball(float x, float y)
            : base(x, y, "ball_down", Color.White)
        {
            velocity = new Vector2(12f, 0f);
        }

        public override void update(GameTime gameTime)
        {
            if (gravity)
            {
                velocity.Y = 150f;
            }
            else
            {
                velocity.Y = -150f;
            }

            if (Main.keysDown.Contains(Keys.A))
            {
                velocity.X = -200f;
            }
            else if (Main.keysDown.Contains(Keys.D))
            {
                velocity.X = 200f;
            }
            else
            {
                velocity.X = 0f;
            }

            localPosition += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }


        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOver(mouseWorldPos) && !Main.states["paused"])
            {
                gravity = !gravity;
            }
        }
    }
}
