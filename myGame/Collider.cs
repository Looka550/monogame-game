using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame
{
    public abstract class Collider
    {
        public GameObject owner;
        public Vector2 offset;

        protected Collider(GameObject _owner, Vector2 _offset)
        {
            owner = _owner;
            offset = _offset;
        }

        public Vector2 worldPosition =>
            owner.worldPosition + offset;
        public abstract bool intersects(Collider other);

        public abstract bool containsPoint(Point point);

        public abstract void DebugDraw(SpriteBatch spriteBatch);
    }
}
