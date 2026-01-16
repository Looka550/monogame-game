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
        Model originalPlayButtonModel;
        Object3D model;
        bool alreadyAnimated = false;
        bool animating = false;
        TransformAnimator transformAnim;
        FramesAnimator framesAnim;

        List<(Vector3 endTransform, int length, string transformType)> transformAnimationPipeline = new();
        List<(List<Model> frames, int frameLength)> framesAnimationPipeline = new();
        List<string> animationPipeline = new();
        int animationProgress = 0;
        int transformAnimationProgress = 0;
        int framesAnimationProgress = 0;
        public PlayButton(float x, float y, Model playButtonModel, List<Model> playButtonAnimationFrames)
            : base(x, y, "transparent_play_button", Color.White)
        {
            addCollider("square");
            localPosition -= new Vector2(w / 2, h / 2); // centered
            localPosition -= new Vector2(w, 0);

            originalPlayButtonModel = playButtonModel;
            model = new Object3D(playButtonModel);
            addChild(model);
            setupModel();

            transformAnimationPipeline.Add((new Vector3(0, 0, 90), 50, "rotation")); // first rotation
            transformAnimationPipeline.Add((new Vector3(360, 0, 90), 50, "rotation")); // spin
            transformAnimationPipeline.Add((new Vector3(-3.15f, -6.8f, 0), 50, "position")); // move down

            framesAnimationPipeline.Add((playButtonAnimationFrames, 2)); // frames

            animationPipeline.Add("transform:rotation");
            animationPipeline.Add("transform:rotation");
            animationPipeline.Add("frames");
            animationPipeline.Add("transform:position");
            //animationPipeline.Add("transform");
        }

        void setupModel()
        {
            model.position3 = new Vector3(-3.15f, 0, 0);
            model.rotation3 = new Vector3(0, 0, 180);
            model.model = originalPlayButtonModel;

            animationProgress = 0;
            transformAnimationProgress = 0;
            framesAnimationProgress = 0;
        }

        public override void onStageChange(string stage)
        {
            if (stage == "mainmenu")
            {
                setupModel();
                alreadyAnimated = false;
            }
        }

        public override void update(GameTime gameTime)
        {
            if (animating)
            {
                if (animationPipeline[animationProgress].StartsWith("transform"))
                {
                    if (animationPipeline[animationProgress].EndsWith("rotation"))
                    {
                        Vector3 nextRotation = transformAnim.nextTransform();
                        model.rotation3 = nextRotation;
                    }
                    else
                    {
                        Vector3 nextPosition = transformAnim.nextTransform();
                        model.position3 = nextPosition;
                    }
                }
                else
                {
                    model.model = framesAnim.nextFrame();
                }
            }
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOver(mouseWorldPos))
            {
                Main.objectsClicked.Add("play button");
                if (!alreadyAnimated)
                {
                    startAnimation();
                }
            }
        }

        void startAnimation()
        {
            if (!animating)
            {
                animating = true;
                if (animationPipeline[animationProgress].StartsWith("transform"))
                {
                    if (transformAnimationPipeline[transformAnimationProgress].transformType == "rotation")
                    {
                        transformAnim = new TransformAnimator(model.rotation3, transformAnimationPipeline[transformAnimationProgress].endTransform, this, transformAnimationPipeline[transformAnimationProgress].length, transformAnimationPipeline[transformAnimationProgress].transformType);
                    }
                    else
                    {
                        transformAnim = new TransformAnimator(model.position3, transformAnimationPipeline[transformAnimationProgress].endTransform, this, transformAnimationPipeline[transformAnimationProgress].length, transformAnimationPipeline[transformAnimationProgress].transformType);
                    }
                }
                else
                {
                    framesAnim = new FramesAnimator(framesAnimationPipeline[framesAnimationProgress].frames, model, this, framesAnimationPipeline[framesAnimationProgress].frameLength);
                }

            }
        }

        void animationsCompleted()
        {
            alreadyAnimated = true;
            //Console.WriteLine("all animations completed");
            Main.changeStage($"level{Main.nextLevel}");
        }

        public override void transformAnimatorCallback()
        {
            animating = false;
            animationProgress++;
            transformAnimationProgress++;
            if (animationProgress < animationPipeline.Count)
            {
                startAnimation();
            }
            else
            {
                animationsCompleted();
            }
        }

        public override void framesAnimatorCallback()
        {
            animating = false;
            animationProgress++;
            framesAnimationProgress++;
            if (animationProgress < animationPipeline.Count)
            {
                startAnimation();
            }
            else
            {
                animationsCompleted();
            }
        }

    }
}
