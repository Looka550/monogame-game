using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class Ball : GameObject
    {
        public Vector2 velocity;
        public bool gravity = true;
        public Vector2 spawnpoint = new Vector2(40, 5 * 128);
        Dictionary<string, int> textureUp;
        Dictionary<string, int> textureDown;

        public Ball(float x, float y)
            : base(x, y, "ball_down", Color.White)
        {
            velocity = new Vector2(12f, 0f);
            name = "player";
            textureUp = loadTexture("ball_up");
            textureDown = loadTexture("ball_down");
        }

        public override void update(GameTime gameTime)
        {
            if (gravity)
            {
                velocity.Y = 300f;
            }
            else
            {
                velocity.Y = -300f;
            }

            if (Main.keysDown.Contains(Keys.A))
            {
                velocity.X = -800f;
            }
            else if (Main.keysDown.Contains(Keys.D))
            {
                velocity.X = 800f;
            }
            else
            {
                velocity.X = 0f;
            }

            localPosition += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            updateCamera();
        }

        void updateCamera()
        {
            int screenWidth = (int)(Main.viewport.Width / Main.viewportScale);
            float halfScreen = screenWidth / 2f;


            Main.scrollX = (int)(worldPosition.X + (w / 2f) - halfScreen);

            if (Main.scrollX < Main.minX)
            {
                Main.scrollX = Main.minX;
            }
            else if (Main.scrollX > Main.maxX - screenWidth)
            {
                Main.scrollX = Main.maxX - screenWidth;
            }
        }


        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOver(mouseWorldPos) && !Main.states["paused"])
            {
                gravity = !gravity;
            }
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            Rectangle texRect;
            if (gravity)
            {
                texRect = new Rectangle(textureDown["x"], textureDown["y"], textureDown["width"], textureDown["height"]);
            }
            else
            {
                texRect = new Rectangle(textureUp["x"], textureUp["y"], textureUp["width"], textureUp["height"]);
            }

            if (textureUp != null && enabled)
            {
                spriteBatch.Draw(
                    spritesheet,
                    worldPosition,
                    texRect,
                    color,
                    rotation,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    z
                );
            }
        }

        public void onDeath()
        {
            localPosition = spawnpoint;
        }
    }
}
