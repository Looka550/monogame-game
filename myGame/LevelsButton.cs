using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class LevelsButton : GameObject
    {

        public LevelsButton(float x, float y)
            : base(x, y, "levels_button", Color.White)
        {
            drawCondition = "mainmenu";
            addCollider("square");
            localPosition -= new Vector2(w / 2, h / 2); // centered
            localPosition += new Vector2(w, 0);
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOver(mouseWorldPos))
            {
                Main.stage = "levelsmenu";
            }
        }
    }
}
