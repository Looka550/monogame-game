using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class PauseSlider : GameObject
    {
        SliderBall ball;

        public PauseSlider(float x, float y, PauseMenu parent, int initValue = 0)
            : base()
        {
            GameObject sliderLine = new GameObject(x, y, "slider_line");
            sliderLine.localPosition -= new Vector2(sliderLine.w / 2, sliderLine.h / 2);
            sliderLine.z = 0.82f;
            sliderLine.ui = true;
            addChild(sliderLine);
            parent.elements.Add(sliderLine);

            SliderBall sliderBall = new SliderBall(x, y, initValue);
            sliderBall.localPosition -= new Vector2(0, sliderBall.h / 2);
            sliderBall.z = 0.83f;
            sliderBall.ui = true;
            addChild(sliderBall);
            parent.elements.Add(sliderBall);

            ball = sliderBall;
        }

        public int getValue()
        {
            return ball.value;
        }
    }
}
