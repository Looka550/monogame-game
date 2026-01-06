using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class GameObject
    {
        public Dictionary<string, int> textureData;
        public Vector2 localPosition;
        public Vector2 scale;
        public float rotation;
        public Color color;
        public Vector2 pivot;
        public int z;
        public Rectangle hitbox;

        // traversal
        public GameObject parent;
        public List<GameObject> children = new List<GameObject>();
        public Vector2 worldPosition;

        public bool hasStarted = false;

        public string name = "GameObject";




        public GameObject(float x, float y, float width, float height, string spriteName, Color _color)
        {
            localPosition = new Vector2(x, y);
            worldPosition = localPosition;
            scale = new Vector2(width, height);
            rotation = 0;
            loadTexture(spriteName);
            color = _color;
            z = 0;
            Main.world.addChild(this);

            Init();
        }

        public GameObject(float x, float y, string spriteName, Color _color)
        {
            localPosition = new Vector2(x, y);
            worldPosition = localPosition;
            scale = new Vector2(1, 1);
            rotation = 0;
            loadTexture(spriteName);
            color = _color;
            z = 0;
            Main.world.addChild(this);

            Init();
        }

        public GameObject(float x, float y, string spriteName)
        {
            localPosition = new Vector2(x, y);
            worldPosition = localPosition;
            scale = new Vector2(1, 1);
            rotation = 0;
            loadTexture(spriteName);
            color = Color.White;
            z = 0;
            Main.world.addChild(this);

            Init();
        }

        public GameObject()
        {
            localPosition = new Vector2(0, 0);
            worldPosition = localPosition;
            scale = new Vector2(1, 1);
            rotation = 0;
            Main.world.addChild(this);

            Init();
        }

        public GameObject(bool isWorld) // constructor for world node
        {
            if (isWorld)
            {
                localPosition = new Vector2(0, 0);
                worldPosition = localPosition;
                scale = new Vector2(1, 1);
                rotation = 0;
            }
            else
            {
                Console.WriteLine("Invalid GameObject constructor - boolean = false");
            }
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
            hitbox = new Rectangle((int)localPosition.X, (int)localPosition.Y, (int)scale.X * textureData["width"], (int)scale.Y * textureData["height"]);
        }

        public virtual void start() { }
        public virtual void update(GameTime gameTime) { }
        public virtual void lateUpdate(GameTime gameTime) { }

        public virtual void onMouseDown(MouseState mouse) { }
        public virtual void onMouseUp(MouseState mouse) { }
        public virtual void onMouseReleased(MouseState mouse) { }
        public virtual void onMouseClicked(MouseState mouse) { }

        public virtual void onKeyDown(Keys key) { }
        public virtual void onKeyUp(Keys key) { }
        public virtual void onKeyPressed(Keys key) { }
        public virtual void onKeyReleased(Keys key) { }

        public virtual void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {

            if (textureData != null)
            {
                spriteBatch.Draw(
                    spritesheet,
                    worldPosition,
                    new Rectangle(textureData["x"], textureData["y"], textureData["width"], textureData["height"]),
                    color,
                    rotation,
                    pivot,
                    scale,
                    SpriteEffects.None,
                    z
                );
            }
        }

        // traversal
        public void addChild(GameObject child)
        {
            if (child.parent != null)
            {
                child.parent.removeChild(child);
            }

            child.parent = this;
            children.Add(child);
        }

        public void removeChild(GameObject child)
        {
            if (children.Contains(child))
            {
                children.Remove(child);
                child.parent = null;
            }
        }


        public void UpdateTransform()
        {
            if (parent == null)
            {
                worldPosition = localPosition;
            }
            else
            {
                worldPosition = parent.worldPosition + localPosition;
            }
            foreach (var child in children)
            {
                child.UpdateTransform();
            }
        }

        public void traverse(Action<GameObject> visitor)
        {
            visitor(this);

            foreach (var child in children)
            {
                child.traverse(visitor);
            }
        }

    }
}
