using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace myGame;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //Vector2 resolution = new Vector2(844, 390);
    Vector2 resolution = new Vector2(844 * 2, 390 * 2);
    public static Texture2D atlas;
    Texture2D spritesheet;

    bool paused = false;
    Texture2D textbox;

    MouseState currentMouse;
    MouseState previousMouse;

    public static SpriteLoader spriteLoader;
    Ball ball;
    Tile tileCol;
    Tile tileCol2;
    Ball ball2;
    Ball ball3;
    PauseMenu pauseMenu;
    GameObject pauseButton;
    GameObject musicOffButton;
    GameObject musicOnButton;
    bool musicOn = true;

    SpriteFont uiFont;

    bool won = false;

    public static List<Vector2> pointsToDraw;
    public static GameObject world = new GameObject(true);

    // input
    KeyboardState currentKeyboard;
    KeyboardState previousKeyboard;

    List<Keys> keysDown = new();
    List<Keys> keysPressed = new();
    List<Keys> keysReleased = new();

    public static Texture2D debugPixel;


    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {

        base.Initialize();




        _graphics.PreferredBackBufferWidth = (int)resolution.X;
        _graphics.PreferredBackBufferHeight = (int)resolution.Y;

        _graphics.ApplyChanges();


        //movingEnemy = new Enemy(new Vector2(400, 150), 3f, 300, 500);
        //circularEnemy = new CircularEnemy(new Vector2(600, 150), 100f, 0.03f);
        //teleportEnemy = new TeleportEnemy(new Vector2(200, 0), new Vector2(500, 0), 2f);

        ball = new Ball(0, 0);
        tileCol = new Tile(200, 0);
        ball2 = new Ball(200, 350);
        //ball2 = new Ball(200, 220);
        //ball3 = new Ball(150, 150);
        //ball.addChild(ball2);
        //ball2.addChild(ball3);
        pauseButton = new GameObject(670, 15, "blank", Color.Red);
        musicOffButton = new GameObject(660 - 128, 15, "music_off");
        musicOnButton = new GameObject(660 - 128, 15, "music_on");
        pauseMenu = new PauseMenu();

        ball.name = "ball";
        tileCol.name = "moving_tile";
        ball2.name = "ball2";
        //ball3.name = "ball3";
        world.name = "world";

        ball.addCollider("circle", true);
        ball2.addCollider("circle", true);
        tileCol.addCollider("border");

        for (int i = 0; i < 10; i++)
        {
            Tile tile = new Tile(i * 128, 600);
            tile.addCollider("border");
            tile.name = $"tile[{i}]";
        }
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        atlas = Content.Load<Texture2D>("atlas2");
        spritesheet = Content.Load<Texture2D>("spritesheetTexture");
        textbox = Content.Load<Texture2D>("textbox");
        uiFont = Content.Load<SpriteFont>("font");

        spriteLoader = new SpriteLoader();

        debugPixel = new Texture2D(GraphicsDevice, 1, 1);
        debugPixel.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        pointsToDraw = new List<Vector2>();

        world.traverse(obj => // calling start
        {
            if (!obj.hasStarted)
            {
                obj.start();
                obj.hasStarted = true;
            }
        });

        world.traverse(obj => // calling update
        {
            obj.update(gameTime);
        });

        world.traverse(obj => // calling late update
        {
            obj.lateUpdate(gameTime);
        });


        ball.localPosition += ball.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        tileCol.localPosition += new Vector2(-12f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        ball2.localPosition += new Vector2(0f, 30f) * (float)gameTime.ElapsedGameTime.TotalSeconds;



        // technical things
        checkCollisions();
        checkInput();
        world.UpdateTransform(); // update transforms of child objects
        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        world.traverse(obj => // calling draw
        {
            obj.draw(_spriteBatch, spritesheet);
        });

        world.traverse(obj =>
        {
            obj.debugDraw(_spriteBatch);
        });

        for (int i = 0; i < pointsToDraw.Count; i++)
        {
            drawPoint(_spriteBatch, pointsToDraw[i]);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    void drawPoint(SpriteBatch spriteBatch, Vector2 position, int size = 12)
    {
        spriteBatch.Draw(
            Main.debugPixel,
            new Rectangle(
                (int)position.X - size / 2,
                (int)position.Y - size / 2,
                size,
                size
            ),
            Color.DarkOrange
        );
    }

    void checkInput()
    {
        keysDown.Clear();
        keysPressed.Clear();
        keysReleased.Clear();

        previousKeyboard = currentKeyboard;
        currentKeyboard = Keyboard.GetState();

        foreach (var key in currentKeyboard.GetPressedKeys())
        {
            keysDown.Add(key);

            if (!previousKeyboard.IsKeyDown(key))
            {
                keysPressed.Add(key);
            }
        }

        foreach (var key in previousKeyboard.GetPressedKeys())
        {
            if (!currentKeyboard.IsKeyDown(key))
            {
                keysReleased.Add(key);
            }
        }

        foreach (var key in keysDown)
        {
            world.traverse(obj => obj.onKeyDown(key));
        }

        foreach (var key in keysPressed)
        {
            world.traverse(obj => obj.onKeyPressed(key));
        }

        foreach (var key in keysReleased)
        {
            world.traverse(obj => obj.onKeyReleased(key));
        }

        foreach (var key in previousKeyboard.GetPressedKeys())
        {
            if (!currentKeyboard.IsKeyDown(key))
            {
                world.traverse(obj => obj.onKeyUp(key));
            }
        }

        // mouse
        previousMouse = currentMouse;
        currentMouse = Mouse.GetState();
        checkMouseInput();
    }


    void checkMouseInput()
    {
        // left
        if (currentMouse.LeftButton == ButtonState.Pressed) // mouse down
        {
            world.traverse(obj => obj.onMouseDown(currentMouse));
        }

        if (currentMouse.LeftButton == ButtonState.Released) // mouse up
        {
            // mouse up
            world.traverse(obj => obj.onMouseUp(currentMouse));
        }

        if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed) // mouse release
        {
            // release
            world.traverse(obj => obj.onMouseReleased(currentMouse));
        }

        if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released) // mouse click
        {
            // click
            world.traverse(obj => obj.onMouseClicked(currentMouse));
        }

        // right
        if (currentMouse.RightButton == ButtonState.Pressed) // mouse down
        {
            world.traverse(obj => obj.onMouseDown(currentMouse));
        }

        if (currentMouse.RightButton == ButtonState.Released) // mouse up
        {
            // mouse up
            world.traverse(obj => obj.onMouseUp(currentMouse));
        }

        if (currentMouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed) // mouse release
        {
            // release
            world.traverse(obj => obj.onMouseReleased(currentMouse));
        }

        if (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released) // mouse click
        {
            // click
            world.traverse(obj => obj.onMouseClicked(currentMouse));
        }
    }



    void checkCollisions()
    {
        List<GameObject> colliders = new List<GameObject>();
        List<GameObject> dynamics = new List<GameObject>();

        world.traverse(obj =>
        {
            if (obj.collider != null)
            {
                colliders.Add(obj);
                if (obj.collider.isDynamic)
                {
                    dynamics.Add(obj);
                }
            }
        });

        for (int i = 0; i < dynamics.Count; i++)
        {
            for (int j = 0; j < colliders.Count; j++)
            {
                var a = dynamics[i];
                var b = colliders[j];
                if (a == b)
                {
                    continue;
                }

                if (a.collider.intersects(b.collider))
                {
                    if (b.collider.isSolid)
                    {
                        //Console.WriteLine($"onTouch: {a.name} : {b.name}");
                        b.collider?.moveAway(a);
                    }
                    else
                    {
                        //Console.WriteLine($"onCollision: {a.name} : {b.name}");
                        a.onCollision(b);
                        b.onCollision(a);
                    }
                }
            }
        }

    }
}
