using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class TeleportEnemy
{
    public Vector2 Position;
    public Vector2 LeftSpot;
    public Vector2 RightSpot;
    public float TeleportInterval; // in seconds
    private float timer;
    private bool atLeftSpot;

    public TeleportEnemy(Vector2 left, Vector2 right, float interval)
    {
        LeftSpot = left;
        RightSpot = right;
        TeleportInterval = interval;
        Position = LeftSpot;
        atLeftSpot = true;
        timer = 0f;
    }

    public void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer >= TeleportInterval)
        {
            // Teleport to the other spot
            Position = atLeftSpot ? RightSpot : LeftSpot;
            atLeftSpot = !atLeftSpot;
            timer = 0f;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        spriteBatch.Draw(texture, Position, new Rectangle(0, 129, 128, 256), Color.White);
    }
}