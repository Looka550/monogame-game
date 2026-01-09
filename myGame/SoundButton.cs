using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class SoundButton : GameObject
    {
        bool soundOn = true;
        Dictionary<string, int> textureOn;
        Dictionary<string, int> textureOff;

        public SoundButton(float x, float y, float scaleX, float scaleY)
            : base(x, y, scaleX, scaleY, "sound_on", Color.White)
        {
            addCollider("square");
            z = 0.8f;
            ui = true;
            textureOn = loadTexture("sound_on");
            textureOff = loadTexture("sound_off");
        }

        public override void update(GameTime gameTime)
        {
            soundOn = Main.states["soundOn"];
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOverUI(mouseWorldPos))
            {
                soundOn = !soundOn;
                Main.states["soundOn"] = soundOn;
            }
        }
        public override void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            Rectangle texRect;
            if (soundOn)
            {
                texRect = new Rectangle(textureOn["x"], textureOn["y"], textureOn["width"], textureOn["height"]);
            }
            else
            {
                texRect = new Rectangle(textureOff["x"], textureOff["y"], textureOff["width"], textureOff["height"]);
            }

            if (textureOn != null && drawCondition == Main.stage && enabled)
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
    }
}
