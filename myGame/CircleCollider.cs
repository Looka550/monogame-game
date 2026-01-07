using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame
{
    public class CircleCollider : Collider
    {
        public float radius;

        public CircleCollider(GameObject owner, Vector2 offset, float radius, bool isDynamic)
            : base(owner, offset, isDynamic)
        {
            this.radius = radius;
        }

        public override bool intersects(Collider other)
        {
            if (other is CircleCollider circle)
            {
                float distSq = Vector2.DistanceSquared(
                    worldPosition,
                    circle.worldPosition
                );

                float r = radius + circle.radius;
                return distSq <= r * r;
            }
            else if (other is SquareCollider square)
            {
                return square.intersects(this);
            }

            return false;
        }

        public Vector2 getCenter()
        {
            return new Vector2(owner.worldPosition.X + owner.w / 2, owner.worldPosition.Y + owner.h / 2); ;
        }

        public override bool containsPoint(Point point)
        {
            Vector2 p = point.ToVector2();
            Vector2 center = getCenter();
            return Vector2.DistanceSquared(p, center) <= radius * radius;
        }

        public override void debugDraw(SpriteBatch spriteBatch)
        {
            int segments = 24;
            float step = MathHelper.TwoPi / segments;
            Color c = Color.Lime;
            float thickness = 2f;

            Vector2 center = getCenter();
            Vector2 prev = center + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = step * i;
                Vector2 next = center + new Vector2(
                    MathF.Cos(angle) * radius,
                    MathF.Sin(angle) * radius
                );

                DrawLine(spriteBatch, prev, next, c, thickness);
                prev = next;
            }
        }

        static void DrawLine(
            SpriteBatch spriteBatch,
            Vector2 start,
            Vector2 end,
            Color color,
            float thickness)
        {
            Vector2 edge = end - start;
            float angle = MathF.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(
                Main.debugPixel,
                start,
                null,
                color,
                angle,
                Vector2.Zero,
                new Vector2(edge.Length(), thickness),
                SpriteEffects.None,
                0
            );
        }

        public override void moveAway(GameObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
