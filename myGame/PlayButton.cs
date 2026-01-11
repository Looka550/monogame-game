using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class PlayButton : GameObject
    {
        Object3D model;
        bool animating = false;
        TransformAnimator anim;
        public PlayButton(float x, float y, Model playButtonModel)
            : base(x, y, "transparent_play_button", Color.White)
        {
            drawCondition = "mainmenu";
            addCollider("square");
            localPosition -= new Vector2(w / 2, h / 2); // centered
            localPosition -= new Vector2(w, 0);

            model = new Object3D(playButtonModel);
            model.drawCondition = "mainmenu";
            addChild(model);
            setupModel();
        }

        void setupModel()
        {
            model.position3 = new Vector3(-3.15f, 0, 0);
            model.rotation3 = new Vector3(0, 0, 180);
        }

        public override void onStageChange(string stage)
        {
            if (stage == "mainmenu")
            {
                setupModel();
            }
        }

        public override void onKeyDown(Keys key)
        {
            /*
            if (key == Keys.G)
            {
                model.position3.X -= 0.01f;
            }
            else if (key == Keys.H)
            {
                model.position3.X += 0.01f;
            }
            Console.WriteLine(model.position3.X);
            */
        }

        public override void update(GameTime gameTime)
        {
            if (animating)
            {
                Vector3 nextTransform = anim.nextTransform();
                model.rotation3 = nextTransform;
                Console.WriteLine($"transform: {model.rotation3}, nextTransform: {nextTransform}");
            }
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOver(mouseWorldPos))
            {
                startAnimation();
                //Main.stage = "level";
            }
        }

        void startAnimation()
        {
            if (!animating)
            {
                Console.WriteLine("animation started");
                animating = true;
                anim = new TransformAnimator(model.rotation3, new Vector3(model.rotation3.X, 360, model.rotation3.Z), this, 100);
            }
        }

        public override void transformAnimatorCallback()
        {
            Console.WriteLine("animation ended");
            animating = false;
        }

    }
}
