using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class CircularEnemy
{
    public Vector2 Position;
    public float Radius;
    public float Angle; // current angle in radians
    public float Speed; // how fast it moves along the circle
    public Vector2 Center; // center of the circle


    public CircularEnemy(Vector2 center, float radius, float speed)
    {
        Center = center;
        Radius = radius;
        Speed = speed;
        Angle = 0f;
        Position = new Vector2(Center.X + Radius, Center.Y); // start at rightmost point
    }

    public void Update()
    {
        Angle += Speed;
        if (Angle > MathHelper.TwoPi) Angle -= MathHelper.TwoPi;

        Position.X = Center.X + Radius * (float)Math.Cos(Angle);
        Position.Y = Center.Y + Radius * (float)Math.Sin(Angle);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        spriteBatch.Draw(texture, Position, new Rectangle(0, 129, 128, 256), Color.White);
    }
}
