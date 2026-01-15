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
        public Model model;
        Matrix world;
        Matrix view;
        Matrix projection;

        public Vector3 rotation3;
        public Vector3 scale3;
        public Vector3 position3;

        public List<Model> animationFrames;
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

            rotation3 = new Vector3(0, 0, 90);
            position3 = new Vector3(0, 0, 0);
            scale3 = new Vector3(0.01f, 0.01f, 0.01f);
        }

        void setupMatrices()
        {
            view = Matrix.CreateLookAt(
                new Vector3(0, 0, 10), // camera position
                Vector3.Zero, // look at
                Vector3.Up
            );
            /*
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f),
                Main.viewport.AspectRatio,
                0.1f,
                100f
            );
            */
            float width = 20f; // size of 3d objects
            float height = width / Main.viewport.AspectRatio;

            projection = Matrix.CreateOrthographic(
                width,
                height,
                0.1f,
                100f
            );
        }

        public override void update(GameTime gameTime)
        {
            updateWorldMatrix();
        }

        void updateWorldMatrix()
        {
            float rotX = MathHelper.ToRadians(rotation3.X);
            float rotY = MathHelper.ToRadians(rotation3.Y);
            float rotZ = MathHelper.ToRadians(rotation3.Z);

            Matrix rotation =
                Matrix.CreateRotationX(rotX) *
                Matrix.CreateRotationY(rotY) *
                Matrix.CreateRotationZ(rotZ);

            Matrix scale = Matrix.CreateScale(scale3);
            Matrix translation = Matrix.CreateTranslation(position3);

            world = scale * rotation * translation;
        }

        public void drawModel()
        {
            if (!enabled)
            {
                return;
            }

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.LightingEnabled = true; // disable all lighting
                    //effect.VertexColorEnabled = true; // use material color
                    //effect.DiffuseColor = new Vector3(0.1f, 0.1f, 0.1f);
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