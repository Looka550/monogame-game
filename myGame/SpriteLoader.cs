using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;

namespace myGame;

public class SpriteLoader
{
    const string data = "spritesheetData.txt";
    public Dictionary<string, Dictionary<string, Object>> spritesData;
    public SpriteLoader()
    {
        string dataPath = Path.Combine(
            AppContext.BaseDirectory,
            data
        );

        spritesData = new Dictionary<string, Dictionary<string, Object>>();

        string[] lines = File.ReadAllLines(dataPath);

        for (int i = 10; i < lines.Length; i++) // data starts at line 10
        {
            string[] values = lines[i].Split(";");
            Dictionary<string, Object> spriteData = new Dictionary<string, Object>();

            spriteData.Add("rotation", Int32.Parse(values[1]));
            spriteData.Add("x", Int32.Parse(values[2]));
            spriteData.Add("y", Int32.Parse(values[3]));
            spriteData.Add("width", Int32.Parse(values[4]));
            spriteData.Add("height", Int32.Parse(values[5]));
            spriteData.Add("pivotx", float.Parse(values[8]));
            spriteData.Add("pivoty", float.Parse(values[9]));

            spritesData.Add(values[0], spriteData);
        }
    }

    public Dictionary<string, Object> getSprite(string spriteName)
    {
        return spritesData[spriteName];
    }
}