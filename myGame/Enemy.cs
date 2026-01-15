using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame;

namespace myGame
{
    public class Enemy : GameObject
    {
        public Enemy(int x, int y, string spriteName, Vector2 colliderScale)
            : base(x, y, spriteName)
        {
            init(0, colliderScale);
        }

        public Enemy(int x, int y, int degrees, string spriteName)
            : base(x, y, spriteName)
        {
            init(degrees, new Vector2(0.6f, 0.6f));
        }

        public Enemy(int x, int y, int degrees)
            : base(x, y, "spike")
        {
            init(degrees, new Vector2(0.6f, 0.6f));
        }

        void init(int degrees, Vector2 colliderScale)
        {
            rotation = degreesToRadian(degrees);
            name = "spike";
            addCollider("circle", colliderScale);
        }

        public override void onCollision(GameObject other)
        {
            if (other.name == "player")
            {
                ((Ball)other).onDeath();
            }
        }

        float degreesToRadian(int degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            if (textureData != null && enabled)
            {
                Vector2 origin = new Vector2(
                    textureData["width"] / 2f,
                    textureData["height"] / 2f
                );

                Vector2 drawPos = worldPosition + origin * scale;

                spriteBatch.Draw(
                    spritesheet,
                    drawPos,
                    new Rectangle(
                        textureData["x"],
                        textureData["y"],
                        textureData["width"],
                        textureData["height"]
                    ),
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    z
                );
            }
        }
    }
}
