using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class LevelButton : GameObject
    {
        int level;
        public LevelButton(float x, float y, int level)
            : base(x, y, 2, 2, $"level{level}", Color.White)
        {
            addCollider("square");
            this.level = level;

            localPosition -= new Vector2(w / 2, h / 2);
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOver(mouseWorldPos))
            {
                Main.objectsClicked.Add("level button");
                Main.changeStage($"level{level}");
            }
        }
    }
}
