using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class TransformAnimator
    {
        Vector3 A;
        Vector3 B;
        GameObject callbackObject;
        int length;
        int progress = 0;
        Vector3 slice;
        Vector3 currentTransform;
        public TransformAnimator(Vector3 A, Vector3 B, GameObject callbackObject, int length) // length = frames
        {
            this.A = A;
            this.B = B;
            this.callbackObject = callbackObject;
            this.length = length;

            slice = (B - A) / length;
            currentTransform = A;
        }

        public Vector3 nextTransform()
        {
            if (progress == length - 1)
            {
                endAnimation();
                return strip(B);
            }
            progress++;
            currentTransform += slice;
            return currentTransform;
        }

        public Vector3 strip(Vector3 vec)
        {
            return new Vector3(vec.X % 360, vec.Y % 360, vec.Z % 360);
        }

        void endAnimation()
        {
            callbackObject.transformAnimatorCallback();
        }
    }
}
