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
        int screenWidth;
        Vector2 uiScale;
        int padding;
        Model model;
        List<Model> animationFrames;

        public LevelData(int screenWidth, Vector2 uiScale, int padding, Model model, List<Model> animationFrames)
        {
            this.screenWidth = screenWidth;
            this.uiScale = uiScale;
            this.padding = padding;
            this.model = model;
            this.animationFrames = animationFrames;
        }
        public List<GameObject> fetch(string stage)
        {
            List<GameObject> objects = new();
            Main.world = new GameObject(true);
            Main.world.name = "world";

            if (stage == "mainmenu")
            {
                objects.AddRange(mainmenu());
            }
            else if (stage == "levelsmenu")
            {
                objects.AddRange(levelsmenu());
            }
            else
            {
                switch (stage)
                {
                    case "level1":
                        objects.AddRange(level1());
                        objects.AddRange(levelUI());
                        break;
                    default:
                        Console.WriteLine("Invalid level number [LevelData]");
                        return null;
                }
            }
            return objects;
        }

        List<GameObject> getBounds(int _maxX)
        {
            List<GameObject> objects = new();
            Main.minX = 0;
            Main.maxX = _maxX;

            for (int i = 0; i < (int)(_maxX / 128f); i++) // floor
            {
                Tile tile = new Tile(i * 128, 1024 - 128);
                tile.addCollider("border");
                tile.name = $"floor[{i}]";
                objects.Add(tile);
            }
            for (int i = 0; i < (int)(_maxX / 128f); i++) // ceiling
            {
                Tile tile = new Tile(i * 128, 0 - 128);
                tile.addCollider("border");
                tile.name = $"ceiling[{i}]";
                objects.Add(tile);
            }
            for (int i = 0; i < 8; i++) // left wall
            {
                Tile tile = new Tile(-128, i * 128);
                tile.addCollider("border");
                tile.name = $"wallL[{i}]";
                objects.Add(tile);
            }
            for (int i = 0; i < 8; i++) // right wall
            {
                Tile tile = new Tile(_maxX, i * 128);
                tile.addCollider("border");
                tile.name = $"wallR[{i}]";
                objects.Add(tile);
            }


            return objects;
        }

        List<GameObject> mainmenu()
        {
            List<GameObject> objects = new();
            objects.Add(new MainMenu(model, animationFrames));

            return objects;
        }

        List<GameObject> levelsmenu()
        {
            List<GameObject> objects = new();
            objects.Add(new LevelsMenu());

            return objects;
        }

        List<GameObject> levelUI()
        {
            List<GameObject> objects = new();
            objects.Add(new PauseButton((int)(screenWidth - (128 * 1 * uiScale.X) - padding * 1), padding, uiScale.X, uiScale.Y));
            objects.Add(new MusicButton((int)(screenWidth - (128 * 2 * uiScale.X) - padding * 2), padding, uiScale.X, uiScale.Y));
            objects.Add(new SoundButton((int)(screenWidth - (128 * 3 * uiScale.X) - padding * 3), padding, uiScale.X, uiScale.Y));
            objects.Add(new PauseMenu());
            return objects;
        }

        List<GameObject> level1()
        {
            List<GameObject> objects = new();
            Ball ball = new Ball(0, 0);
            ball.addCollider("circle", null, true);
            objects.Add(ball);

            objects.AddRange(getBounds(3072));
            return objects;
        }
    }
}
