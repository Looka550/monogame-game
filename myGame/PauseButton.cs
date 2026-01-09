using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class PauseButton : GameObject
    {

        public PauseButton()
            : base(670, 15, "pause_button", Color.White)
        {
            z = 0.8f;
            ui = true;
            addCollider("square");
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOverUI(mouseWorldPos))
            {
                Main.paused = !Main.paused;
            }
        }
    }
}
