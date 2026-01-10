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
        public float z;

        // traversal
        public GameObject parent;
        public List<GameObject> children = new List<GameObject>();
        public Vector2 worldPosition;

        // collisions
        public Collider collider;
        public int w;
        public int h;

        public bool hasStarted = false;

        public string name = "GameObject";

        public string drawCondition = "level";
        public bool enabled = true;
        public bool ui = false;




        public GameObject(float x, float y, float scaleX, float scaleY, string spriteName, Color _color)
        {
            localPosition = new Vector2(x, y);
            worldPosition = localPosition;
            scale = new Vector2(scaleX, scaleY);
            rotation = 0;
            textureData = loadTexture(spriteName);
            color = _color;
            z = 0.1f;
            Main.world.addChild(this);

            Init();
        }

        public GameObject(float x, float y, string spriteName, Color _color)
        {
            localPosition = new Vector2(x, y);
            worldPosition = localPosition;
            scale = new Vector2(1, 1);
            rotation = 0;
            textureData = loadTexture(spriteName);
            color = _color;
            z = 0.1f;
            Main.world.addChild(this);

            Init();
        }

        public GameObject(float x, float y, string spriteName)
        {
            localPosition = new Vector2(x, y);
            worldPosition = localPosition;
            scale = new Vector2(1, 1);
            rotation = 0;
            textureData = loadTexture(spriteName);
            color = Color.White;
            z = 0.1f;
            Main.world.addChild(this);

            Init();
        }

        public GameObject()
        {
            localPosition = new Vector2(0, 0);
            worldPosition = localPosition;
            scale = new Vector2(1, 1);
            rotation = 0;
            textureData = loadTexture("transparent");
            z = 0;
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
            w = (int)(textureData["width"] * scale.X);
            h = (int)(textureData["height"] * scale.Y);
        }

        public Dictionary<string, int> loadTexture(string spriteName)
        {
            var rawData = Main.spriteLoader.getSprite(spriteName);
            var textureData = new Dictionary<string, int>();
            textureData.Add("x", (int)rawData["x"]);
            textureData.Add("y", (int)rawData["y"]);
            textureData.Add("width", (int)rawData["width"]);
            textureData.Add("height", (int)rawData["height"]);

            pivot = new Vector2((float)rawData["pivotx"], (float)rawData["pivoty"]);
            return textureData;
        }

        public virtual void start() { }
        public virtual void update(GameTime gameTime) { }
        public virtual void lateUpdate(GameTime gameTime) { }

        public virtual void onMouseDown(MouseState mouse, Vector2 mouseWorldPos) { }
        public virtual void onMouseUp(MouseState mouse, Vector2 mouseWorldPos) { }
        public virtual void onMouseReleased(MouseState mouse, Vector2 mouseWorldPos) { }
        public virtual void onMouseClicked(MouseState mouse, Vector2 mouseWorldPos) { }

        public virtual void onKeyDown(Keys key) { }
        public virtual void onKeyUp(Keys key) { }
        public virtual void onKeyPressed(Keys key) { }
        public virtual void onKeyReleased(Keys key) { }

        public virtual void onCollision(GameObject other) { }

        public virtual void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {

            if (textureData != null && drawCondition == Main.stage && enabled)
            {
                spriteBatch.Draw(
                    spritesheet,
                    worldPosition,
                    new Rectangle(textureData["x"], textureData["y"], textureData["width"], textureData["height"]),
                    color,
                    rotation,
                    Vector2.Zero,
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


        public void addCollider(Collider _collider)
        {
            collider = _collider;
        }

        public void addCollider(string colliderType, Nullable<Vector2> colScale = null, bool isDynamic = false)
        {
            if (colScale == null)
            {
                colScale = Vector2.One;
            }

            Vector2 scale = (Vector2)colScale;

            int colWidth = (int)(w * scale.X);
            int colHeight = (int)(h * scale.Y);

            Vector2 offset = new Vector2(
                (w - colWidth) / 2f,
                (h - colHeight) / 2f
            );


            if (colliderType == "circle")
            {
                int r = colWidth / 2;
                collider = new CircleCollider(
                    this,
                    Vector2.Zero,
                    r,
                    isDynamic
                );
            }
            else if (colliderType == "square")
            {
                collider = new SquareCollider(
                    this,
                    offset,
                    colWidth,
                    colHeight,
                    isDynamic
                );
            }
            else if (colliderType == "border")
            {
                collider = new SquareCollider(
                    this,
                    offset,
                    colWidth,
                    colHeight,
                    isDynamic,
                    true
                );
            }
            else
            {
                Console.WriteLine("Invalid collider type");
            }
        }

        public bool isMouseOver(Vector2 mouseWorldPos)
        {
            if (collider == null)
            {
                return false;
            }

            return collider.containsPoint(new Point((int)mouseWorldPos.X + Main.scrollX, (int)mouseWorldPos.Y));
        }

        public bool isMouseOverUI(Vector2 mouseWorldPos)
        {
            if (collider == null)
            {
                return false;
            }

            return collider.containsPoint(new Point((int)mouseWorldPos.X, (int)mouseWorldPos.Y));
        }

        public void debugDraw(SpriteBatch spriteBatch)
        {
            if (drawCondition == Main.stage)
            {
                collider?.debugDraw(spriteBatch);
            }
        }

        public void destroy()
        {
            enabled = false;
            parent = null;
        }

    }
}
