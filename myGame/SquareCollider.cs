using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame
{
    public class SquareCollider : Collider
    {
        public int width;
        public int height;

        public List<Vector2> pointsToDraw = new List<Vector2>();

        public SquareCollider(GameObject owner, Vector2 offset, int width, int height, bool isDynamic)
            : base(owner, offset, isDynamic)
        {
            this.width = width;
            this.height = height;
        }
        public SquareCollider(GameObject owner, Vector2 offset, int width, int height, bool isDynamic, bool isSolid)
            : base(owner, offset, isDynamic)
        {
            this.width = width;
            this.height = height;
            this.isSolid = isSolid;
        }

        public Rectangle worldRect =>
            new Rectangle(
                (int)(owner.worldPosition.X + offset.X),
                (int)(owner.worldPosition.Y + offset.Y),
                width,
                height
            );


        public override bool intersects(Collider other)
        {
            if (other is SquareCollider square)
            {
                return worldRect.Intersects(square.worldRect);
            }
            else if (other is CircleCollider circle)
            {
                return circleIntersectsSquare(circle, this);
            }

            return false;
        }

        bool circleIntersectsSquare(CircleCollider circle, SquareCollider square)
        {
            Vector2 center = circle.getCenter();
            Rectangle rect = square.worldRect;

            // closest point on rect to circle center
            float closestX = MathHelper.Clamp(center.X, rect.Left, rect.Right);
            float closestY = MathHelper.Clamp(center.Y, rect.Top, rect.Bottom);

            // dist from center to point
            float dx = center.X - closestX;
            float dy = center.Y - closestY;

            return (dx * dx + dy * dy) <= circle.radius * circle.radius;
        }

        public override bool containsPoint(Point point)
        {
            return worldRect.Contains(point);
        }

        public override void debugDraw(SpriteBatch spriteBatch)
        {
            Rectangle r = worldRect;
            int t = 2;
            Color c = Color.Red;

            // top
            spriteBatch.Draw(Main.debugPixel, new Rectangle(r.X, r.Y, r.Width, t), c);
            // bottom
            spriteBatch.Draw(Main.debugPixel, new Rectangle(r.X, r.Bottom - t, r.Width, t), c);
            // left
            spriteBatch.Draw(Main.debugPixel, new Rectangle(r.X, r.Y, t, r.Height), c);
            // right
            spriteBatch.Draw(Main.debugPixel, new Rectangle(r.Right - t, r.Y, t, r.Height), c);
        }

        void resolveCircleCollision(CircleCollider circle)
        {
            Vector2 center = circle.getCenter();
            Rectangle rect = worldRect;

            // closest point on square to circle center
            float closestX = MathHelper.Clamp(center.X, rect.Left, rect.Right);
            float closestY = MathHelper.Clamp(center.Y, rect.Top, rect.Bottom);

            Vector2 closestPoint = new Vector2(closestX, closestY);
            Main.pointsToDraw.Add(closestPoint);

            Vector2 rectCenter = new Vector2(rect.Left + ((rect.Right - rect.Left) / 2f), rect.Top + ((rect.Bottom - rect.Top) / 2f));
            Main.pointsToDraw.Add(rectCenter);

            Vector2 topc = new Vector2(rectCenter.X, rect.Top);
            Vector2 bottomc = new Vector2(rectCenter.X, rect.Bottom);
            Vector2 rightc = new Vector2(rect.Right, rectCenter.Y);
            Vector2 leftc = new Vector2(rect.Left, rectCenter.Y);

            float topd = dist(closestPoint, topc);
            float bottomd = dist(closestPoint, bottomc);
            float rightd = dist(closestPoint, rightc);
            float leftd = dist(closestPoint, leftc);

            float min = MathF.Min(MathF.Min(topd, bottomd), MathF.Min(rightd, leftd));
            Vector2 movingForce;

            if (min == topd)
            {
                Main.pointsToDraw.Add(topc);
                movingForce = new Vector2(0f, -1f);
            }
            else if (min == bottomd)
            {
                Main.pointsToDraw.Add(bottomc);
                movingForce = new Vector2(0f, 1f);
            }
            else if (min == rightd)
            {
                Main.pointsToDraw.Add(rightc);
                movingForce = new Vector2(-1f, 0f);
            }
            else
            {
                Main.pointsToDraw.Add(leftc);
                movingForce = new Vector2(1f, 0f);
            }

            for (int i = 0; i < 1000; i++) // not infinite loop just in case of bugs
            {
                if (!intersects(circle))
                {
                    break;
                }
                else
                {
                    circle.owner.worldPosition += movingForce; // changes position instantly (realligns itself with updatetransform next frame)
                    circle.owner.localPosition += movingForce; // correct way to change position
                }
            }
        }

        float dist(Vector2 a, Vector2 b)
        {
            Vector2 subtracted = b - a;
            return subtracted.Length();
        }




        public override void moveAway(GameObject obj)
        {
            if (obj.collider is CircleCollider circle)
            {
                Vector2 center = circle.getCenter();
                Rectangle rect = worldRect;

                // closest point on square to circle center
                float closestX = MathHelper.Clamp(center.X, rect.Left, rect.Right);
                float closestY = MathHelper.Clamp(center.Y, rect.Top, rect.Bottom);

                Vector2 closestPoint = new Vector2(closestX, closestY);
                //Main.pointsToDraw.Add(closestPoint);

                Vector2 rectCenter = new Vector2(rect.Left + ((rect.Right - rect.Left) / 2f), rect.Top + ((rect.Bottom - rect.Top) / 2f));
                //Main.pointsToDraw.Add(rectCenter);

                Vector2 topc = new Vector2(rectCenter.X, rect.Top);
                Vector2 bottomc = new Vector2(rectCenter.X, rect.Bottom);
                Vector2 rightc = new Vector2(rect.Right, rectCenter.Y);
                Vector2 leftc = new Vector2(rect.Left, rectCenter.Y);

                float topd = dist(closestPoint, topc);
                float bottomd = dist(closestPoint, bottomc);
                float rightd = dist(closestPoint, rightc);
                float leftd = dist(closestPoint, leftc);

                float min = MathF.Min(MathF.Min(topd, bottomd), MathF.Min(rightd, leftd));
                Vector2 movingForce;

                if (min == topd)
                {
                    //Main.pointsToDraw.Add(topc);
                    movingForce = new Vector2(0f, -1f);
                }
                else if (min == bottomd)
                {
                    //Main.pointsToDraw.Add(bottomc);
                    movingForce = new Vector2(0f, 1f);
                }
                else if (min == rightd)
                {
                    //Main.pointsToDraw.Add(rightc);
                    movingForce = new Vector2(1f, 0f);
                }
                else
                {
                    //Main.pointsToDraw.Add(leftc);
                    movingForce = new Vector2(-1f, 0f);
                }

                float penetration;

                if (min == topd || min == bottomd)
                {
                    penetration = circle.radius - MathF.Abs(center.Y - closestPoint.Y); // Y axis
                }
                else
                {
                    penetration = circle.radius - MathF.Abs(center.X - closestPoint.X); // X axis
                }

                circle.owner.localPosition += movingForce * penetration;
            }
            else
            {
                Console.WriteLine($"Unimplemented solid collider interaction: {obj.name} : {owner.name}");
            }
        }

    }
}