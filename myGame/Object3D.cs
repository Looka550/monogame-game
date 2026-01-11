using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;


namespace myGame
{
    public class Object3D : GameObject
    {
        Model model;
        Matrix world;
        Matrix view;
        Matrix projection;
        public Object3D(Model model)
            : base()
        {
            Vector2 screenCenter = new Vector2(
                Main.viewport.Width / Main.viewportScale * 0.5f,
                Main.virtualHeight * 0.5f
            );
            ui = true;

            this.model = model;
            setupMatrices();

        }

        void setupMatrices()
        {
            view = Matrix.CreateLookAt(
                new Vector3(0, 0, 10),   // camera position
                Vector3.Zero,            // look at
                Vector3.Up
            );

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f),
                Main.viewport.AspectRatio,
                0.1f,
                100f
            );
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            drawModel();
        }

        void drawModel()
        {
            // rotation angles in radians
            float rotX = MathHelper.ToRadians(0f); // 90 degrees X
            float rotY = MathHelper.ToRadians(0f); // 90 degrees Y
            float rotZ = MathHelper.ToRadians(90f); // 90 degrees Z

            // create rotation matrices
            Matrix rotation =
                Matrix.CreateRotationX(rotX) *
                Matrix.CreateRotationY(rotY) *
                Matrix.CreateRotationZ(rotZ);

            // create scale and translation
            Matrix scale = Matrix.CreateScale(0.01f);
            Matrix translation = Matrix.CreateTranslation(new Vector3(0, 0, 0)); // move to desired position

            // combine them: scale → rotate → translate
            world = scale * rotation * translation;



            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting(); // REMOVE this line
                    effect.LightingEnabled = true; // disable all lighting
                    effect.VertexColorEnabled = false; // use material color
                    //effect.DiffuseColor = new Vector3(0.1f, 0.1f, 0.1f); // solid red, or whatever you want
                    effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.3f);
                    effect.PreferPerPixelLighting = true;
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}