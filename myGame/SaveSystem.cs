using System;
using System.IO;
using System.Text.Json;

namespace myGame;

public static class SaveSystem
{
    static string savePath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "gravitio.json"
        );

    public static void save()
    {
        SaveData data = new SaveData
        {
            musicValue = Main.musicValue,
            soundValue = Main.soundValue,
            soundOn = Main.states["soundOn"],
            musicOn = Main.states["musicOn"],
            nextLevel = Main.nextLevel
        };

        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        Console.WriteLine($"saving: {json}");
        File.WriteAllText(savePath, json);
    }

    public static void load()
    {
        if (!File.Exists(savePath))
        {
            Console.WriteLine("no file");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonSerializer.Deserialize<SaveData>(json);

        Main.musicValue = data.musicValue;
        Main.soundValue = data.soundValue;
        Main.states["soundOn"] = data.soundOn;
        Main.states["musicOn"] = data.musicOn;
        Console.WriteLine($"nextlevel before {Main.nextLevel}");
        Main.nextLevel = data.nextLevel;

        Console.WriteLine($"nextlevel after {Main.nextLevel}");
    }
}
