using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace myGame
{
    public class GameObject
    {
        public Dictionary<string, int> textureData;
        public Vector2 position;
        public Vector2 scale;
        public float rotation;
        public Color color;
        public Vector2 pivot;
        public int z;
        public Rectangle hitbox;



        public GameObject(float x, float y, float width, float height, string spriteName, Color _color)
        {
            position = new Vector2(x, y);
            scale = new Vector2(width, height);
            rotation = 0;
            loadTexture(spriteName);
            color = _color;
            z = 0;

            Init();
        }

        public GameObject(float x, float y, string spriteName, Color _color)
        {
            position = new Vector2(x, y);
            scale = new Vector2(1, 1);
            rotation = 0;
            loadTexture(spriteName);
            color = _color;
            z = 0;

            Init();
        }

        public GameObject(float x, float y, string spriteName)
        {
            position = new Vector2(x, y);
            scale = new Vector2(1, 1);
            rotation = 0;
            loadTexture(spriteName);
            color = Color.White;
            z = 0;

            Init();
        }

        public GameObject()
        {
            position = new Vector2(0, 0);
            scale = new Vector2(1, 1);
            rotation = 0;

            Init();
        }

        public void Init()
        {

        }

        public void loadTexture(string spriteName)
        {
            var rawData = Main.spriteLoader.getSprite(spriteName);
            textureData = new Dictionary<string, int>();
            textureData.Add("x", (int)rawData["x"]);
            textureData.Add("y", (int)rawData["y"]);
            textureData.Add("width", (int)rawData["width"]);
            textureData.Add("height", (int)rawData["height"]);

            pivot = new Vector2((float)rawData["pivotx"], (float)rawData["pivoty"]);
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)scale.X * textureData["width"], (int)scale.Y * textureData["height"]);
        }

        public virtual void start()
        {

        }

        public virtual void update(GameTime gameTime)
        {

        }

        public virtual void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            if (textureData != null)
            {
                spriteBatch.Draw(spritesheet, position, new Rectangle(textureData["x"], textureData["y"], textureData["width"], textureData["height"]), color, rotation, pivot, scale, SpriteEffects.None, z);
            }
        }
    }
}
