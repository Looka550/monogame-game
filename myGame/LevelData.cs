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
                    case "level2":
                        objects.AddRange(level2());
                        objects.AddRange(levelUI());
                        break;
                    case "level3":
                        objects.AddRange(level3());
                        objects.AddRange(levelUI());
                        break;
                    case "level4":
                        objects.AddRange(level4());
                        objects.AddRange(levelUI());
                        break;
                    case "level5":
                        objects.AddRange(level5());
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
            objects.Add(new WinFlag(2024, 672));

            objects.Add(new Enemy(4 * 128, 8 * 128 - 172 - 128, 0, "spike"));
            objects.Add(new Enemy(5 * 128, 8 * 128 - 172 - 128, 0, "spike"));
            objects.Add(new Enemy(6 * 128, 8 * 128 - 172 - 128, 0, "spike"));
            objects.Add(new Enemy(7 * 128, 8 * 128 - 172 - 128, 0, "spike"));

            objects.Add(new Enemy(11 * 128, 0, 180, "spike"));
            objects.Add(new Enemy(12 * 128, 0, 180, "spike"));
            objects.Add(new Enemy(13 * 128, 0, 180, "spike"));
            objects.Add(new Enemy(14 * 128, 0, 180, "spike"));

            objects.AddRange(getBounds(128 * 18));
            return objects;
        }

        List<GameObject> level2()
        {
            List<GameObject> objects = new();
            Ball ball = new Ball(0, 0);
            ball.addCollider("circle", null, true);
            objects.Add(ball);
            objects.Add(new WinFlag(2024 - 128 * 6, 672));

            for (int i = 0; i < 6; i++) // col
            {
                Tile tile = new Tile(1024 - 256, 256 + i * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }
            for (int i = 0; i < 6; i++) // col
            {
                Tile tile = new Tile(1024 - 256 + 256, 256 + i * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }

            for (int i = 0; i < 8; i++) // line
            {
                Tile tile = new Tile(1024 + i * 128, 256);
                tile.addCollider("border");
                objects.Add(tile);
            }

            for (int i = 0; i < 8; i++) // col
            {
                Tile tile = new Tile(21 * 128, i * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }

            objects.Add(new Enemy(1024 - 128 * 3, 7 * 128 - 172, 0, "spike"));
            objects.Add(new Enemy(1024 + 7 * 128, -32, 180, "spike"));
            objects.Add(new Enemy(1024 + 7 * 128, 128 * 3, 180, "spike"));

            objects.Add(new MovingEnemy(new Vector2(2024 - 128 * 7, 1024 - 128 - 172), new Vector2(20 * 128, 1024 - 128 - 172), 300, 0));
            objects.Add(new MovingEnemy(new Vector2(1024 - 256 + 128, 0), new Vector2(1024 - 256 + 128, 1024 - 128 - 172), 600, 0));

            objects.AddRange(getBounds(22 * 128));
            return objects;
        }

        List<GameObject> level3()
        {
            List<GameObject> objects = new();
            Ball ball = new Ball(0, 0);
            ball.addCollider("circle", null, true);
            objects.Add(ball);
            objects.Add(new WinFlag(24 * 128, 672));

            for (int i = 0; i < 5; i++) // col
            {
                Tile tile = new Tile(128 * 6 - 44, i * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }

            for (int i = 0; i < 6; i++) // col
            {
                Tile tile = new Tile(128 * 8, 256 + i * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }


            for (int i = 0; i < 6; i++) // col
            {
                Tile tile = new Tile(128 * 17, 256 + i * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }

            for (int i = 0; i < 5; i++) // col
            {
                Tile tile = new Tile(128 * 19 + 44, i * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }

            for (int i = 0; i < 9; i++) // line
            {
                Tile tile = new Tile(128 * 8 + i * 128, 2 * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }

            objects.Add(new TeleportEnemy(new Vector2(7 * 128 - 20, 7 * 128 - 152), new Vector2(7 * 128 - 20, 4 * 128 - 152), 2f, 270));
            objects.Add(new TeleportEnemy(new Vector2(18 * 128 - 20 + 44, 5 * 128 - 152), new Vector2(18 * 128 - 20 + 44, 1 * 128 - 152), 2f, 270));

            objects.AddRange(getBounds(26 * 128));
            return objects;
        }

        List<GameObject> level4()
        {
            List<GameObject> objects = new();
            Ball ball = new Ball(0, 0);
            ball.addCollider("circle", null, true);
            objects.Add(ball);
            objects.Add(new WinFlag(33 * 128, 672));

            for (int i = 0; i < 5; i++) // col
            {
                Tile tile = new Tile(9 * 128, 128 + i * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }

            for (int i = 0; i < 5; i++) // line
            {
                Tile tile = new Tile(7 * 128 + i * 128, 3 * 128);
                tile.addCollider("border");
                objects.Add(tile);
            }


            objects.Add(new OrbitingEnemy(new Vector2(9 * 128, 3 * 128), 500f, 4f, true));

            objects.Add(new OrbitingEnemy(new Vector2(20 * 128, 5 * 128), 250f, 2f, true));
            objects.Add(new OrbitingEnemy(new Vector2(26 * 128, 1 * 128), 250f, 2f, true));
            objects.Add(new OrbitingEnemy(new Vector2(25 * 128, 3 * 128), 400f, 1f, true));

            objects.AddRange(getBounds(35 * 128));
            return objects;
        }

        List<GameObject> level5()
        {
            List<GameObject> objects = new();

            Ball ball = new Ball(0, 0);
            ball.addCollider("circle", null, true);
            objects.Add(ball);

            // section 1 - warm up
            for (int i = 4; i < 11; i++)
            {
                objects.Add(new Enemy(i * 128, 8 * 128 - 172 - 128, 0, "spike"));
            }

            for (int i = 12; i < 17; i++)
            {
                objects.Add(new Enemy(i * 128, 0, 180, "spike"));
            }

            // section 2 - moving spikes
            objects.Add(new MovingEnemy(
                new Vector2(18 * 128, 8 * 128 - 172 - 128),
                new Vector2(24 * 128, 8 * 128 - 172 - 128),
                500, 0));

            objects.Add(new MovingEnemy(
                new Vector2(20 * 128, 0),
                new Vector2(20 * 128, 6 * 128),
                700, 180));

            objects.Add(new MovingEnemy(
                new Vector2(26 * 128, 0 * 128),
                new Vector2(26 * 128, 6 * 128),
                700, 180));

            // section 3 - teleporting spikes
            objects.Add(new TeleportEnemy(
                new Vector2(30 * 128 - 20, 7 * 128 - 152),
                new Vector2(30 * 128 - 20, 1 * 128 - 152),
                1.5f, 270));

            objects.Add(new TeleportEnemy(
                new Vector2(33 * 128 - 20, 1 * 128 - 152),
                new Vector2(33 * 128 - 20, 7 * 128 - 152),
                1.2f, 270));

            objects.Add(new TeleportEnemy(
                new Vector2(36 * 128 - 20, 7 * 128 - 152),
                new Vector2(36 * 128 - 20, 5 * 128 - 152),
                1.8f, 270));

            objects.Add(new TeleportEnemy(
                new Vector2(36 * 128 - 20, 3 * 128 - 152),
                new Vector2(36 * 128 - 20, 1 * 128 - 152),
                1.8f, 270));


            objects.Add(new CheckpointFlag(37 * 128, 2 * 128 - 200));
            GameObject block = new Tile(37 * 128, 2 * 128);
            block.addCollider("border");
            objects.Add(block);


            // section 4 - orbiting spikes
            objects.Add(new OrbitingEnemy(
                new Vector2(40 * 128, 4 * 128),
                300f, 2.5f, true));

            objects.Add(new OrbitingEnemy(
                new Vector2(44 * 128, 3 * 128),
                450f, 1.5f, true));

            objects.Add(new OrbitingEnemy(
                new Vector2(46 * 128, 5 * 128),
                250f, 3.5f, true));

            // section 5 - final
            for (int i = 48; i < 52; i++)
                objects.Add(new Enemy(i * 128, 8 * 128 - 172 - 128, 0, "spike"));

            for (int i = 51; i < 55; i++)
                objects.Add(new Enemy(i * 128, 0, 180, "spike"));

            objects.Add(new MovingEnemy(
                new Vector2(54 * 128, 8 * 128 - 172 - 128),
                new Vector2(58 * 128, 8 * 128 - 172 - 128),
                600, 0));

            objects.Add(new TeleportEnemy(
                new Vector2(56 * 128 - 20, 6 * 128 - 152),
                new Vector2(56 * 128 - 20, 2 * 128 - 152),
                1.0f, 270));


            objects.Add(new WinFlag(59 * 128, 672));


            objects.AddRange(getBounds(61 * 128));

            return objects;
        }

    }
}
