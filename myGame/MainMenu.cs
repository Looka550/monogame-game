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
        GameObject levels;

        public MainMenu(Model playButtonModel)
            : base()
        {
            Vector2 screenCenter = new Vector2(
                Main.viewport.Width / Main.viewportScale * 0.5f,
                Main.virtualHeight * 0.5f
            );
            drawCondition = "mainmenu";

            //play = new PlayButton(screenCenter.X, screenCenter.Y);
            //addChild(play);
            play = new Object3D(playButtonModel);
            play.drawCondition = "mainmenu";
            addChild(play);

            levels = new LevelsButton(screenCenter.X, screenCenter.Y);
            addChild(levels);
        }
    }
}
