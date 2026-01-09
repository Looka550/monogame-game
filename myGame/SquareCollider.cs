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
            if (!owner.enabled)
            {
                return;
            }
            Rectangle r = worldRect;
            int t = 5;
            Color c = Color.Red;

            // top
            drawRect(spriteBatch, new Vector2(r.X, r.Y), new Vector2(r.Width, t), c);
            // bottom
            drawRect(spriteBatch, new Vector2(r.X, r.Bottom - t), new Vector2(r.Width, t), c);
            // left
            drawRect(spriteBatch, new Vector2(r.X, r.Y), new Vector2(t, r.Height), c);
            // right
            drawRect(spriteBatch, new Vector2(r.Right - t, r.Y), new Vector2(t, r.Height), c);
        }

        void drawRect(SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color)
        {
            spriteBatch.Draw(
                Main.debugPixel,
                position,
                null,
                color,
                0f,
                Vector2.Zero,
                size,
                SpriteEffects.None,
                1f
            );
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

                Main.debugPoints.Add((center, Color.DarkOrange, owner.ui));

                Vector2 rectCenter = new Vector2(rect.Left + ((rect.Right - rect.Left) / 2f), rect.Top + ((rect.Bottom - rect.Top) / 2f));
                Main.debugPoints.Add((rectCenter, Color.DarkOrange, owner.ui));

                Vector2 topc = new Vector2(rectCenter.X, rect.Top);
                Vector2 bottomc = new Vector2(rectCenter.X, rect.Bottom);
                Vector2 rightc = new Vector2(rect.Right, rectCenter.Y);
                Vector2 leftc = new Vector2(rect.Left, rectCenter.Y);

                float topd = dist(center, topc);
                float bottomd = dist(center, bottomc);
                float rightd = dist(center, rightc);
                float leftd = dist(center, leftc);

                Main.debugLines.Add((center, topc, Color.Red, owner.ui));
                Main.debugLines.Add((center, bottomc, Color.Green, owner.ui));
                Main.debugLines.Add((center, rightc, Color.Blue, owner.ui));
                Main.debugLines.Add((center, leftc, Color.Yellow, owner.ui));

                float min = MathF.Min(MathF.Min(topd, bottomd), MathF.Min(rightd, leftd));
                Vector2 movingForce;

                if (min == topd)
                {
                    Main.debugPoints.Add((topc, Color.DarkOrange, owner.ui));
                    movingForce = new Vector2(0f, -1f);
                }
                else if (min == bottomd)
                {
                    Main.debugPoints.Add((bottomc, Color.DarkOrange, owner.ui));
                    movingForce = new Vector2(0f, 1f);
                }
                else if (min == rightd)
                {
                    Main.debugPoints.Add((rightc, Color.DarkOrange, owner.ui));
                    movingForce = new Vector2(1f, 0f);
                }
                else
                {
                    Main.debugPoints.Add((leftc, Color.DarkOrange, owner.ui));
                    movingForce = new Vector2(-1f, 0f);
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
            else
            {
                Console.WriteLine($"Unimplemented solid collider interaction: {obj.name} : {owner.name}");
            }
        }

    }
}