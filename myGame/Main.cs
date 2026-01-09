using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace myGame;

public class Main : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch spriteBatchWorld;
    SpriteBatch spriteBatchUI;

    // resize system

    Vector2 resolution = new Vector2(844, 390);
    //Vector2 resolution = new Vector2(844 * 2, 390 * 2);
    //Vector2 resolution = new Vector2(900, 500);
    //Vector2 resolution = new Vector2(1500, 900);
    public static int virtualHeight = 1024; // normal screen dimensions
    public static Viewport viewport;
    public static float viewportScale;
    public static int scrollX = 0;
    public static string stage = "level";
    string previousStage = "mainmenu";
    public static int levelNumber = 1;


    public static Texture2D atlas;
    Texture2D spritesheet;

    bool debugMode = true;

    public static bool paused = true;
    Texture2D textbox;

    MouseState currentMouse;
    MouseState previousMouse;

    public static SpriteLoader spriteLoader;
    Ball ball;
    Tile tileCol;
    Tile tileCol2;
    Ball ball2;
    Ball ball3;
    MainMenu mainmenu;
    LevelsMenu levelsmenu;
    PauseMenu pauseMenu;
    PauseButton pauseButton;
    GameObject musicOffButton;
    GameObject musicOnButton;
    bool musicOn = true;

    SpriteFont uiFont;

    bool won = false;

    public static List<(Vector2 pos, Color color, bool ui)> debugPoints;
    public static List<(Vector2 start, Vector2 end, Color color, bool ui)> debugLines;
    public static GameObject world = new GameObject(true);

    // input
    KeyboardState currentKeyboard;
    KeyboardState previousKeyboard;

    public static List<Keys> keysDown = new List<Keys>();
    public static List<Keys> keysPressed = new List<Keys>();
    public static List<Keys> keysReleased = new List<Keys>();

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


        viewport = GraphicsDevice.Viewport;

        viewportScale = viewport.Height / (float)virtualHeight;




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
        pauseButton = new PauseButton();
        musicOffButton = new GameObject(660 - 128, 15, "music_off");
        musicOnButton = new GameObject(660 - 128, 15, "music_on");
        pauseMenu = new PauseMenu();
        pauseMenu.start();
        mainmenu = new MainMenu();
        levelsmenu = new LevelsMenu();

        ball.name = "ball";
        tileCol.name = "moving_tile";
        ball2.name = "ball2";
        //ball3.name = "ball3";
        world.name = "world";

        ball.addCollider("circle", new Vector2(2, 2), true);
        ball2.addCollider("circle", null, true);
        tileCol.addCollider("border", new Vector2(2, 2));

        for (int i = 0; i < 18; i++)
        {
            Tile tile = new Tile(i * 128, 1024 - 128);
            tile.addCollider("border");
            tile.name = $"tile[{i}]";
        }
        for (int i = 0; i < 10; i++)
        {
            Tile tile = new Tile(i * 128, i * 128);
            tile.name = $"tile2[{i}]";
        }
    }

    protected override void LoadContent()
    {
        spriteBatchWorld = new SpriteBatch(GraphicsDevice);
        spriteBatchUI = new SpriteBatch(GraphicsDevice);

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
        debugPoints = new();
        debugLines = new();

        if (stage != previousStage)
        {
            updateEnableds();
        }
        previousStage = stage;


        world.traverse(obj => // calling start
        {
            if (!obj.hasStarted && (!paused || obj.ui) && obj.enabled)
            {
                obj.start();
                obj.hasStarted = true;
            }
        });

        world.traverse(obj => // calling update
        {
            if ((!paused || obj.ui) && obj.enabled)
            {
                obj.update(gameTime);
            }
        });

        world.traverse(obj => // calling late update
        {
            if ((!paused || obj.ui) && obj.enabled)
            {
                obj.lateUpdate(gameTime);
            }
        });

        if ((!paused || tileCol.ui) && tileCol.enabled)
        {
            //ball.localPosition += ball.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            tileCol.localPosition += new Vector2(-12f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //ball2.localPosition += new Vector2(0f, 30f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }


        if (keysDown.Contains(Keys.V))
        {
            scrollX++;
        }
        else if (keysDown.Contains(Keys.C))
        {
            scrollX--;
        }

        // technical things
        checkCollisions();
        checkInput();
        world.UpdateTransform(); // update transforms of child objects
        base.Update(gameTime);
    }

    void updateEnableds()
    {
        world.traverse(obj =>
        {
            obj.enabled = (obj.drawCondition == stage);
        });
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Vector2 cameraPositionWorld = new Vector2(scrollX, 0);
        Matrix cameraTransformWorld =
            Matrix.CreateTranslation(-cameraPositionWorld.X, 0, 0) *
            Matrix.CreateScale(viewportScale, viewportScale, 1f);

        spriteBatchWorld.Begin(
            transformMatrix: cameraTransformWorld,
            sortMode: SpriteSortMode.FrontToBack,
            blendState: BlendState.NonPremultiplied
        );


        world.traverse(obj => // for world objects
        {
            if (!obj.ui && obj.enabled)
            {
                obj.draw(spriteBatchWorld, spritesheet);
            }
        });

        if (debugMode)
        {
            world.traverse(obj =>
            {
                if (!obj.ui && obj.enabled)
                {
                    obj.debugDraw(spriteBatchWorld);
                }
            });

            foreach (var point in debugPoints)
            {
                if (!point.ui)
                {
                    drawPoint(spriteBatchWorld, point.pos, point.color);
                }
            }
            foreach (var line in debugLines)
            {
                if (!line.ui)
                {
                    drawLine(spriteBatchWorld, line.start, line.end, line.color);
                }
            }
        }

        spriteBatchWorld.End();

        Matrix cameraTransformUI =
            Matrix.CreateTranslation(0, 0, 0) *
            Matrix.CreateScale(viewportScale, viewportScale, 1f);
        spriteBatchUI.Begin(
            transformMatrix: cameraTransformUI,
            sortMode: SpriteSortMode.FrontToBack,
            blendState: BlendState.NonPremultiplied
        );

        world.traverse(obj => // for ui objects (must run the code twice because 2 spritebatches dont handle z values correct)
        {
            if (obj.ui && obj.enabled)
            {
                obj.draw(spriteBatchUI, spritesheet);
            }
        });

        if (debugMode)
        {
            world.traverse(obj =>
            {
                if (obj.ui && obj.enabled)
                {
                    obj.debugDraw(spriteBatchUI);
                }
            });

            foreach (var point in debugPoints)
            {
                if (point.ui)
                {
                    drawPoint(spriteBatchUI, point.pos, point.color);
                }
            }
            foreach (var line in debugLines)
            {
                if (line.ui)
                {
                    drawLine(spriteBatchUI, line.start, line.end, line.color);
                }
            }
        }

        spriteBatchUI.End();

        base.Draw(gameTime);
    }

    void drawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
    {
        int thickness = 2;
        Vector2 delta = end - start;
        float length = delta.Length();
        float angle = (float)Math.Atan2(delta.Y, delta.X);

        spriteBatch.Draw(
            debugPixel,
            start, // position
            null, // source rectangle
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, thickness), // scale X = length, Y = thickness
            SpriteEffects.None,
            1f
        );
    }


    void drawPoint(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        int size = 12;
        spriteBatch.Draw(
            debugPixel,
            position - new Vector2(size / 2f, size / 2f),
            null,
            color,
            0f,
            Vector2.Zero,
            Vector2.One,
            SpriteEffects.None,
            1f
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
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onKeyDown(key);
                }
            });
        }

        foreach (var key in keysPressed)
        {
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onKeyPressed(key);
                }
            });
        }

        foreach (var key in keysReleased)
        {
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onKeyReleased(key);
                }
            });
        }

        foreach (var key in previousKeyboard.GetPressedKeys())
        {
            if (!currentKeyboard.IsKeyDown(key))
            {
                world.traverse(obj =>
                {
                    if (obj.enabled)
                    {
                        obj.onKeyUp(key);
                    }
                });
            }
        }

        // mouse
        previousMouse = currentMouse;
        currentMouse = Mouse.GetState();
        checkMouseInput();
    }


    void checkMouseInput()
    {
        Vector2 mouseWorldPos = getMouseWorldPosition();
        // left
        if (currentMouse.LeftButton == ButtonState.Pressed) // mouse down
        {
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onMouseDown(currentMouse, mouseWorldPos);
                }
            });
        }

        if (currentMouse.LeftButton == ButtonState.Released) // mouse up
        {
            // mouse up
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onMouseUp(currentMouse, mouseWorldPos);
                }
            });
        }

        if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed) // mouse release
        {
            // release
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onMouseReleased(currentMouse, mouseWorldPos);
                }
            });
        }

        if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released) // mouse click
        {
            // click
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onMouseClicked(currentMouse, mouseWorldPos);
                }
            });
        }

        // right
        if (currentMouse.RightButton == ButtonState.Pressed) // mouse down
        {
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onMouseDown(currentMouse, mouseWorldPos);
                }
            });
        }

        if (currentMouse.RightButton == ButtonState.Released) // mouse up
        {
            // mouse up
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onMouseUp(currentMouse, mouseWorldPos);
                }
            });
        }

        if (currentMouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed) // mouse release
        {
            // release
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onMouseReleased(currentMouse, mouseWorldPos);
                }
            });
        }

        if (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released) // mouse click
        {
            // click
            world.traverse(obj =>
            {
                if (obj.enabled)
                {
                    obj.onMouseClicked(currentMouse, mouseWorldPos);
                }
            });
        }
    }

    Vector2 getMouseWorldPosition()
    {
        return new Vector2(
            currentMouse.X / viewportScale,
            currentMouse.Y / viewportScale
        );
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
            var a = dynamics[i];
            for (int j = 0; j < colliders.Count; j++)
            {
                var b = colliders[j];
                if (a == b || !a.enabled || !b.enabled)
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
