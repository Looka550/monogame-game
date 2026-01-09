using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class SliderBall : GameObject
    {
        bool selected = false;
        float minX;
        float maxX;
        float mouseX = -1;
        public int value = 0;

        public SliderBall(float x, float y, int initValue = 0)
            : base(x, y, "slider_ball")
        {
            addCollider("circle", new Vector2(2, 2));
            minX = x - 150 - w / 2;
            maxX = x + 150 - w / 2;

            // set init value
            localPosition.X = minX + initValue * 3;
        }

        public override void update(GameTime gameTime)
        {
            if (selected && mouseX != -1) // -1 at initialization
            {
                localPosition.X = MathHelper.Clamp(mouseX, minX, maxX);
            }
        }

        int mapValue()
        {
            float t = (localPosition.X - minX) / (maxX - minX); // 0..1
            int value = (int)(t * 100); // 0..100
            return value;
        }


        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOverUI(mouseWorldPos))
            {
                selected = true;
            }
        }

        public override void onMouseReleased(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (selected)
            {
                selected = false;
                value = mapValue();
            }
        }

        public override void onMouseDown(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (selected)
            {
                mouseX = mouseWorldPos.X;
            }
        }
    }
}
