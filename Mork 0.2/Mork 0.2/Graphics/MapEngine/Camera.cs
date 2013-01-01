using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork.Graphics.MapEngine
{
    public abstract class Camera
    {
        Matrix view;
        Matrix projection;
        Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
            protected set
            {
                projection = value;
                generateFrustum();
            }
        }

        public Matrix View
        {
            get { return view; }
            protected set
            {
                view = value;
                generateFrustum();
            }
        }

        public BoundingFrustum Frustum { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;

            generatePerspectiveProjectionMatrix(MathHelper.PiOver4);
        }

        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            float aspectRatio = (float)pp.BackBufferWidth /
                (float)pp.BackBufferHeight;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000000.0f);
        }

        public virtual void Update()
        {
        }

        private void generateFrustum()
        {
            Matrix viewProjection = View * Projection;
            Frustum = new BoundingFrustum(viewProjection);
        }

        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }


    public class FreeCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Target { get; private set; }

        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }

        public Vector3 translation;

        public FreeCamera(Vector3 Position, float Yaw, float Pitch,
            GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            this.Roll = 0;

            translation = Vector3.Zero;
        }

        public void Rotate(float YawChange, float PitchChange, float _Roll)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
            this.Roll += _Roll;
        }

        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }

        public override void Update()
        {
            // Calculate the rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            // Offset the position and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;

            // Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);

            this.Up = up;
            this.Right = Vector3.Cross(forward, up);
        }
    }
}
