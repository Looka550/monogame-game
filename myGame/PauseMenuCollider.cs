using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class PauseMenuCollider : GameObject
    {

        public PauseMenuCollider(float x, float y)
            : base(x, y, 1, 1, "blank", Color.HotPink)
        {
            addCollider("square");
            z = 0.8f;
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOver(mouseWorldPos))
            {
                Main.paused = false;
            }
        }
    }
}
