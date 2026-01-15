using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class LevelsMenu : GameObject
    {

        List<GameObject> levelButtons = new();

        public LevelsMenu()
            : base()
        {
            Vector2 screenCenter = new Vector2(
                Main.viewport.Width / Main.viewportScale * 0.5f,
                Main.virtualHeight * 0.5f
            );

            GameObject levelButton1 = new LevelButton(screenCenter.X, screenCenter.Y, 1);
            levelButton1.localPosition += new Vector2(-levelButton1.w * 2 - (levelButton1.w / 4 * 2), -levelButton1.h / 1.5f);
            addChild(levelButton1);

            GameObject levelButton2 = new LevelButton(screenCenter.X, screenCenter.Y, 2);
            levelButton2.localPosition += new Vector2(-levelButton1.w - (levelButton1.w / 4), -levelButton1.h / 1.5f);
            addChild(levelButton2);

            GameObject levelButton3 = new LevelButton(screenCenter.X, screenCenter.Y, 3);
            levelButton3.localPosition += new Vector2(0, -levelButton1.h / 1.5f);
            addChild(levelButton3);

            GameObject levelButton4 = new LevelButton(screenCenter.X, screenCenter.Y, 4);
            levelButton4.localPosition += new Vector2(levelButton1.w + (levelButton1.w / 4), -levelButton1.h / 1.5f);
            addChild(levelButton4);

            GameObject levelButton5 = new LevelButton(screenCenter.X, screenCenter.Y, 5);
            levelButton5.localPosition += new Vector2(levelButton1.w * 2 + (levelButton1.w / 4 * 2), -levelButton1.h / 1.5f);
            addChild(levelButton5);

            GameObject levelButton6 = new LevelButton(screenCenter.X, screenCenter.Y, 6);
            addChild(levelButton6);
            levelButton6.localPosition += new Vector2(-levelButton1.w * 2 - (levelButton1.w / 4 * 2), levelButton1.h / 1.5f);

            GameObject levelButton7 = new LevelButton(screenCenter.X, screenCenter.Y, 7);
            levelButton7.localPosition += new Vector2(-levelButton1.w - (levelButton1.w / 4), levelButton1.h / 1.5f);
            addChild(levelButton7);

            GameObject levelButton8 = new LevelButton(screenCenter.X, screenCenter.Y, 8);
            levelButton8.localPosition += new Vector2(0, levelButton1.h / 1.5f);
            addChild(levelButton8);

            GameObject levelButton9 = new LevelButton(screenCenter.X, screenCenter.Y, 9);
            levelButton9.localPosition += new Vector2(levelButton1.w + (levelButton1.w / 4), levelButton1.h / 1.5f);
            addChild(levelButton9);

            GameObject levelButton10 = new LevelButton(screenCenter.X, screenCenter.Y, 10);
            levelButton10.localPosition += new Vector2(levelButton1.w * 2 + (levelButton1.w / 4 * 2), levelButton1.h / 1.5f);
            addChild(levelButton10);
        }
    }
}
