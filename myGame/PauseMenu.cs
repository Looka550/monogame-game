using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace myGame
{
    public class PauseMenu : GameObject
    {
        public List<GameObject> elements = new List<GameObject>();
        public PauseMenu()
            : base()
        {
            Vector2 screenCenter = new Vector2(
                Main.viewport.Width / Main.viewportScale * 0.5f,
                Main.virtualHeight * 0.5f
            );
            ui = true;

            GameObject overlay = new GameObject(0, 0, 20, 8, "blank", new Color(255, 255, 255, 100));
            overlay.z = 0.79f;
            overlay.ui = true;
            addChild(overlay);
            elements.Add(overlay);

            GameObject background = new GameObject(screenCenter.X, screenCenter.Y, "pause_menu");
            background.localPosition -= new Vector2(background.w / 2, background.h / 2);
            background.z = 0.8f;
            background.ui = true;
            addChild(background);
            elements.Add(background);

            MenuButton menubutton = new MenuButton(screenCenter.X, screenCenter.Y);
            menubutton.localPosition -= new Vector2(menubutton.w / 2, menubutton.h / 2);
            menubutton.z = 0.81f;
            menubutton.ui = true;
            addChild(menubutton);
            elements.Add(menubutton);

            PauseSlider sliderSound = new PauseSlider(screenCenter.X + 64, screenCenter.Y, this, 50);
            sliderSound.localPosition -= new Vector2(0, 128);
            addChild(sliderSound);
            elements.Add(sliderSound);

            PauseSlider sliderMusic = new PauseSlider(screenCenter.X + 64, screenCenter.Y, this);
            addChild(sliderMusic);
            elements.Add(sliderMusic);

            SoundButton soundButton = new SoundButton(screenCenter.X, screenCenter.Y, 0.75f, 0.75f);
            soundButton.localPosition -= new Vector2(soundButton.w / 2 + 192, soundButton.h / 2 + 128);
            addChild(soundButton);
            elements.Add(soundButton);

            MusicButton musicButton = new MusicButton(screenCenter.X, screenCenter.Y, 0.75f, 0.75f);
            musicButton.localPosition -= new Vector2(musicButton.w / 2 + 192, musicButton.h / 2);
            addChild(musicButton);
            elements.Add(musicButton);
        }

        public override void update(GameTime gameTime)
        {
            foreach (GameObject e in elements)
            {
                e.enabled = Main.states["paused"];
            }
        }
    }
}