using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class Enemy
{
    public Vector2 Position;
    public float Speed;
    private bool movingRight;
    public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, 128, 256); // Same size as spike

    private float leftBound;
    private float rightBound;

    public Enemy(Vector2 startPos, float speed, float leftBound, float rightBound)
    {
        Position = startPos;
        Speed = speed;
        movingRight = true;
        this.leftBound = leftBound;
        this.rightBound = rightBound;
    }

    public void Update()
    {
        if (movingRight)
        {
            Position.X += Speed;
            if (Position.X >= rightBound) movingRight = false;
        }
        else
        {
            Position.X -= Speed;
            if (Position.X <= leftBound) movingRight = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        spriteBatch.Draw(texture, Position, new Rectangle(0, 129, 128, 256), Color.White); // same as bottomSpike
    }
}