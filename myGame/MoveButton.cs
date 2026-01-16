using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class MoveButton : GameObject
    {
        int multiplier;
        string direction;
        Ball ball;
        public MoveButton(float x, float y, float scaleX, float scaleY, string direction, Ball ball)
            : base(x, y, scaleX, scaleY, "move_button", Color.White)
        {
            this.direction = direction;
            this.ball = ball;
            addCollider("square");
            z = 0.8f;
            ui = true;

            if (direction == "right")
            {
                multiplier = 1;
            }
            else
            {
                multiplier = -1;
            }
        }


        public override void onMouseDown(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOverUI(mouseWorldPos))
            {
                ball.velocity.X = 500 * multiplier;
                Main.objectsClicked.Add("move button");
            }
        }

        public override void onMouseReleased(MouseState mouse, Vector2 mouseWorldPos)
        {
            ball.velocity.X = 0;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            SpriteEffects se = SpriteEffects.None;
            if (direction == "right")
            {
                se = SpriteEffects.FlipHorizontally;
            }

            if (enabled)
            {
                spriteBatch.Draw(
                    spritesheet,
                    worldPosition,
                    new Rectangle(textureData["x"], textureData["y"], textureData["width"], textureData["height"]),
                    color * 0.5f,
                    rotation,
                    Vector2.Zero,
                    scale,
                    se,
                    z
                );
            }
        }
    }
}
