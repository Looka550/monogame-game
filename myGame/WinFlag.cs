using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class WinFlag : GameObject
    {
        List<Dictionary<string, int>> textures = new();
        Dictionary<string, int> currentTexture;
        int counter = 0;
        int animationProgress = 0;
        bool touched = false;

        public WinFlag(float x, float y)
            : base(x, y, "win_flag1", Color.White)
        {
            textures.Add(loadTexture("win_flag1"));
            textures.Add(loadTexture("win_flag2"));
            textures.Add(loadTexture("win_flag3"));
            textures.Add(loadTexture("win_flag4"));
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
            if (counter > 10 && animationProgress < textures.Count - 1)
            {
                animationProgress++;
                counter = 0;
                currentTexture = textures[animationProgress];
                if (animationProgress == textures.Count - 1)
                {
                    onWin();
                }
            }

            if (currentTexture != null && enabled)
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
            if (other.name == "player")
            {
                touched = true;
            }
        }

        public void onWin()
        {
            Console.WriteLine("win");
        }
    }
}
