using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class FramesAnimator
    {
        List<Model> animationModels;
        Object3D obj;
        GameObject callbackObject;
        int frameLength;
        int progress = 0;
        int interFrameProgress = 0;
        public FramesAnimator(List<Model> animationModels, Object3D obj, GameObject callbackObject, int frameLength) // frameLength = number of frames per animation frame
        {
            this.animationModels = animationModels;
            this.obj = obj;
            this.callbackObject = callbackObject;
            this.frameLength = frameLength;
        }

        public Model nextFrame()
        {
            if (progress == animationModels.Count - 1 && interFrameProgress == frameLength - 1)
            {
                endAnimation();
            }

            Model next = animationModels[progress];

            if (interFrameProgress == frameLength - 1)
            {
                progress++;
                interFrameProgress = 0;
            }
            else
            {
                interFrameProgress++;
            }

            return next;
        }


        void endAnimation()
        {
            callbackObject.framesAnimatorCallback();
        }
    }
}
