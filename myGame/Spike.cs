using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using myGame;

namespace myGame
{
    public class Spike : GameObject
    {
        public Spike(int x, int y, int degrees)
            : base(x, y, "spike")
        {
            rotation = degreesToRadian(degrees);
            name = "spike";
            addCollider("circle", new Vector2(0.6f, 0.6f));
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
            if (textureData != null && drawCondition == Main.stage && enabled)
            {
                Vector2 origin = new Vector2(
                    textureData["width"] / 2f,
                    textureData["height"] / 2f
                );

                Vector2 drawPos = worldPosition + origin * scale; // for rotations

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