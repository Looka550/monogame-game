using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class Tile : GameObject
    {
        public Vector2 velocity;
        static Random rnd = new Random();

        public Tile(float x, float y)
            : base(x, y, "tile", new Color(245 + rnd.Next(-20, 10), 245 + rnd.Next(-20, 10), 245 + rnd.Next(-20, 10)))
        {
            velocity = new Vector2(120f, 0f);
        }

        public override void onCollision(GameObject other)
        {

        }

        public override void onMouseClicked(MouseState mouse)
        {
            if (isMouseOver(mouse))
            {

            }
        }
    }
}
