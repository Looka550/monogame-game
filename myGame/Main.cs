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
    public static string stage = "mainmenu";
    public static int nextLevel = 1;

    public static int minX = 0;
    public static int maxX = 3072;

    int screenWidth;
    int padding;

    Texture2D spritesheet;

    bool debugMode = true;

    public static Dictionary<string, bool> states = new();

    MouseState currentMouse;
    MouseState previousMouse;

    public static SpriteLoader spriteLoader;

    SpriteFont uiFont;

    bool initializationComplete = false;

    public static List<(Vector2 pos, Color color, bool ui)> debugPoints;
    public static List<(Vector2 start, Vector2 end, Color color, bool ui)> debugLines;
    public static GameObject world;

    // input
    KeyboardState currentKeyboard;
    KeyboardState previousKeyboard;

    public static List<Keys> keysDown = new List<Keys>();
    public static List<Keys> keysPressed = new List<Keys>();
    public static List<Keys> keysReleased = new List<Keys>();

    public static Texture2D debugPixel;

    List<GameObject> objects;
    LevelData levelData;

    public static bool scheduledStageChange = false;
    public static string nextStage;
    public static List<string> objectsClicked = new();

    public static int musicValue = 50;
    public static int soundValue = 50;


    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {

        base.Initialize();

        states["paused"] = false;
        states["won"] = false;
        states["soundOn"] = true;
        states["musicOn"] = true;

        SaveSystem.load();

        _graphics.PreferredBackBufferWidth = (int)resolution.X;
        _graphics.PreferredBackBufferHeight = (int)resolution.Y;
        _graphics.ApplyChanges();


        viewport = GraphicsDevice.Viewport;
        viewportScale = viewport.Height / (float)virtualHeight;
        screenWidth = (int)(viewport.Width / viewportScale);
        Vector2 uiScale = new Vector2(1.4f, 1.4f);
        padding = 15;

        Model model = Content.Load<Model>("play_button_animation_fixed");
        List<Model> animationFrames = new();
        for (int i = 0; i <= 20; i++)
        {
            animationFrames.Add(Content.Load<Model>($"play_button_animation_frame{i}"));
        }

        levelData = new LevelData(screenWidth, uiScale, padding, model, animationFrames);
        objects = levelData.fetch(stage);

    }

    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        Console.WriteLine("on exiting");
        SaveSystem.save();
        base.OnExiting(sender, args);
    }

    public static void changeStage(string _nextStage)
    {
        nextStage = _nextStage;
        scheduledStageChange = true;
    }

    protected override void LoadContent()
    {
        spriteBatchWorld = new SpriteBatch(GraphicsDevice);
        spriteBatchUI = new SpriteBatch(GraphicsDevice);

        spritesheet = Content.Load<Texture2D>("spritesheetTexture");
        uiFont = Content.Load<SpriteFont>("font");

        spriteLoader = new SpriteLoader();

        debugPixel = new Texture2D(GraphicsDevice, 1, 1);
        debugPixel.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        Console.WriteLine(nextLevel);
        if (scheduledStageChange) // changes stage
        {
            if (!initializationComplete)
            {
                updateEnableds();
            }
            stage = nextStage;
            objects = levelData.fetch(stage);
            scheduledStageChange = false;
            scrollX = minX;
            states["paused"] = false;
        }


        debugPoints = new();
        debugLines = new();

        world.traverse(obj => // calling start
        {
            if (!obj.hasStarted && (!states["paused"] || obj.ui) && obj.enabled)
            {
                obj.start();
                obj.hasStarted = true;
            }
        });

        world.traverse(obj => // calling update
        {
            if ((!states["paused"] || obj.ui) && obj.enabled)
            {
                obj.update(gameTime);
            }
        });

        world.traverse(obj => // calling late update
        {
            if ((!states["paused"] || obj.ui) && obj.enabled)
            {
                obj.lateUpdate(gameTime);
            }
        });

        // technical things
        if (!initializationComplete)
        {
            initializationComplete = true;
        }
        checkCollisions();
        checkInput();
        world.UpdateTransform(); // update transforms of child objects
        base.Update(gameTime);
    }

    void updateEnableds()
    {
        world.traverse(obj =>
        {
            obj.onStageChange(stage);
        });
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        GraphicsDevice.RasterizerState = new RasterizerState
        {
            CullMode = CullMode.None
        };

        world.traverse(obj =>
        {
            if (obj is Object3D o3d)
            {
                o3d.drawModel(); // without spritebatch
            }
        });

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
        objectsClicked = new();
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
                        a.onCollision(b);
                        b.onCollision(a);
                    }
                }
            }
        }
    }
}
