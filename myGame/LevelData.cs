using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class LevelData
    {
        public List<GameObject> fetch(int levelNumber)
        {
            switch (levelNumber)
            {
                case 1:
                    return level1();
                default:
                    Console.WriteLine("Invalid level number [LevelData]");
                    return null;
            }
        }

        List<GameObject> level1()
        {
            List<GameObject> objects = new();
            objects.Add(new GameObject(0, 0, 2, 2, "blank", Color.HotPink));
            objects.Add(new GameObject(300, 250, 2, 2, "blank", Color.HotPink));
            return objects;
        }
    }
}
