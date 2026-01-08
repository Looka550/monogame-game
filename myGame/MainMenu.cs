using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class MainMenu : GameObject
    {

        GameObject play;

        public MainMenu()
            : base(0, 0, "blank", Color.HotPink)
        {
            Vector2 screenCenter = new Vector2(
                Main.viewport.Width / Main.viewportScale * 0.5f,
                Main.virtualHeight * 0.5f
            );

            Console.WriteLine($"screen center: {screenCenter}");

            play = new GameObject(screenCenter.X, screenCenter.Y, "play_button", Color.White);
            play.localPosition -= new Vector2(play.w / 2, play.h / 2);
            addChild(play);

            
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            Console.WriteLine($"mouse position: {mouseWorldPos}");
        }
    }
}
