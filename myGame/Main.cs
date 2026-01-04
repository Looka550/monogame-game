using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace myGame;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static Texture2D atlas;
    Texture2D spritesheet;

    bool paused = false;
    Texture2D textbox;

    MouseState currentMouse;
    MouseState previousMouse;

    public static SpriteLoader spriteLoader;
    Ball ball;
    PauseMenu pauseMenu;
    GameObject pauseButton;
    GameObject musicOffButton;
    GameObject musicOnButton;
    bool musicOn = true;

    SpriteFont uiFont;

    bool won = false;

    //GameObject 

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {

        base.Initialize();

        //movingEnemy = new Enemy(new Vector2(400, 150), 3f, 300, 500);
        //circularEnemy = new CircularEnemy(new Vector2(600, 150), 100f, 0.03f);
        //teleportEnemy = new TeleportEnemy(new Vector2(200, 0), new Vector2(500, 0), 2f);

        ball = new Ball(100, 200);
        pauseButton = new GameObject(670, 15, "blank", Color.Red);
        musicOffButton = new GameObject(660 - 128, 15, "music_off");
        musicOnButton = new GameObject(660 - 128, 15, "music_on");
        pauseMenu = new PauseMenu();
        pauseMenu.start();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        atlas = Content.Load<Texture2D>("atlas2");
        spritesheet = Content.Load<Texture2D>("spritesheetTexture");
        textbox = Content.Load<Texture2D>("textbox");
        uiFont = Content.Load<SpriteFont>("font");

        spriteLoader = new SpriteLoader();
    }

    protected override void Update(GameTime gameTime)
    {

        CheckCollisions();

        ball.update(gameTime);


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        previousMouse = currentMouse;
        currentMouse = Mouse.GetState();
        mouseClicked();

        _spriteBatch.Begin();

        pauseButton.draw(_spriteBatch, spritesheet);

        if (musicOn)
        {
            musicOnButton.draw(_spriteBatch, spritesheet);
        }
        else
        {
            musicOffButton.draw(_spriteBatch, spritesheet);
        }

        if (paused)
        {
            pauseMenu.draw(_spriteBatch, spritesheet);
        }


        ball.draw(_spriteBatch, spritesheet);


        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void CheckCollisions()
    {

    }

    void mouseClicked()
    {
        /*
        if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
        {
            //gravityDown = !gravityDown;

            if (pauseButton.hitbox.Contains(currentMouse.Position))
            {
                paused = !paused;
            }

            if (musicOnButton.hitbox.Contains(currentMouse.Position))
            {
                musicOn = !musicOn;
            }

        }*/
    }
}
