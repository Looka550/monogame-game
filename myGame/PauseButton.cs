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

        public PauseButton(int x, int y, float scaleX, float scaleY)
            : base(x, y, scaleX, scaleY, "pause_button", Color.White)
        {
            z = 0.8f;
            ui = true;
            addCollider("square");
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOverUI(mouseWorldPos))
            {
                Main.states["paused"] = !Main.states["paused"];
            }
        }

        public override void onStageChange(string stage)
        {
            Main.states["paused"] = false;
        }
    }
}
