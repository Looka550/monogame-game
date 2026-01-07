using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame
{
    public abstract class Collider
    {
        public GameObject owner;
        public Vector2 offset;
        public bool isDynamic;
        public bool isSolid = false;

        protected Collider(GameObject _owner, Vector2 _offset, bool _isDynamic)
        {
            owner = _owner;
            offset = _offset;
            isDynamic = _isDynamic;
        }

        public Vector2 worldPosition =>
            owner.worldPosition + offset;
        public abstract bool intersects(Collider other);

        public abstract bool containsPoint(Point point);

        public abstract void debugDraw(SpriteBatch spriteBatch);

        public abstract void moveAway(GameObject obj); // only for square colliders
    }
}
