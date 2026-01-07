using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame
{
    public class BoxCollider : Collider
    {
        public int width;
        public int height;

        public BoxCollider(GameObject owner, Vector2 offset, int width, int height)
            : base(owner, offset)
        {
            this.width = width;
            this.height = height;
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
            if (other is BoxCollider box)
            {
                return worldRect.Intersects(box.worldRect);
            }
            else if (other is CircleCollider circle)
            {
                return circleIntersectsBox(circle, this);
            }

            return false;
        }

        bool circleIntersectsBox(CircleCollider circle, BoxCollider box)
        {
            Vector2 center = circle.getCenter();
            Rectangle rect = box.worldRect;

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

        public override void DebugDraw(SpriteBatch spriteBatch)
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


    }
}