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
        static Random rnd = new Random();

        public Tile(float x, float y)
            : base(x, y, "tile", new Color(245 + rnd.Next(-20, 10), 245 + rnd.Next(-20, 10), 245 + rnd.Next(-20, 10)))
        {
            name = "tile";
        }
    }
}
