using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class MenuButton : GameObject
    {

        public MenuButton(float x, float y)
            : base(x, y, "menu_button", Color.White)
        {
            drawCondition = "level";
            addCollider("square");
            localPosition += new Vector2(0, 256);
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOverUI(mouseWorldPos))
            {
                Main.stage = "mainmenu";
            }
        }
    }
}
