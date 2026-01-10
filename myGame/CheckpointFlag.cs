using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace myGame
{
    public class CheckpointFlag : GameObject
    {
        List<Dictionary<string, int>> textures = new();
        Dictionary<string, int> currentTexture;
        int counter = 0;
        int animationProgress = 0;
        bool touched = false;

        public CheckpointFlag(float x, float y)
            : base(x, y, "checkpoint_flag1", Color.White)
        {
            textures.Add(loadTexture("checkpoint_flag1"));
            textures.Add(loadTexture("checkpoint_flag2"));
            textures.Add(loadTexture("checkpoint_flag3"));
            textures.Add(loadTexture("checkpoint_flag4"));
            currentTexture = textures[0];
            addCollider("square");
        }

        public override void update(GameTime gameTime)
        {
            if (touched)
            {
                counter++;
            }
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            if (counter > 20 && animationProgress < textures.Count - 1)
            {
                animationProgress++;
                counter = 0;
                currentTexture = textures[animationProgress];
            }

            if (currentTexture != null && drawCondition == Main.stage && enabled)
            {
                spriteBatch.Draw(
                    spritesheet,
                    worldPosition,
                    new Rectangle(currentTexture["x"], currentTexture["y"], currentTexture["width"], currentTexture["height"]),
                    color,
                    rotation,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    z
                );
            }
        }

        public override void onCollision(GameObject other)
        {
            if (other.name == "player" && !touched)
            {
                touched = true;
                ((Ball)other).spawnpoint = localPosition - new Vector2(0, 200);
                Console.WriteLine("touched checkpoint");
            }
        }
    }
}
