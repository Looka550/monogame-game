using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class MusicButton : GameObject
    {
        bool musicOn = true;
        Dictionary<string, int> textureOn;
        Dictionary<string, int> textureOff;

        public MusicButton(float x, float y, float scaleX, float scaleY)
            : base(x, y, scaleX, scaleY, "music_on", Color.White)
        {
            addCollider("square");
            z = 0.8f;
            ui = true;
            textureOn = loadTexture("music_on");
            textureOff = loadTexture("music_off");
        }

        public override void update(GameTime gameTime)
        {
            musicOn = Main.states["musicOn"];
        }

        public override void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos)
        {
            if (isMouseOverUI(mouseWorldPos))
            {
                musicOn = !musicOn;
                Main.states["musicOn"] = musicOn;
            }
        }
        public override void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            Rectangle texRect;
            if (musicOn)
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
