using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace myGame
{
    public class PauseMenu
    {

        List<GameObject> objects;

        public virtual void start()
        {
            objects = new List<GameObject>();
            GameObject background = new GameObject(300, 60, 2, 3, "blank", Color.Aqua);
            objects.Add(background);
        }

        public virtual void update(GameTime gameTime)
        {

        }

        public virtual void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                GameObject obj = objects[i];
                spriteBatch.Draw(spritesheet, obj.localPosition, new Rectangle(obj.textureData["x"], obj.textureData["y"], obj.textureData["width"], obj.textureData["height"]), obj.color, obj.rotation, obj.pivot, obj.scale, SpriteEffects.None, obj.z);
            }
        }
    }
}
