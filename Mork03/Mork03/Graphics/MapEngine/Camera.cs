using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork.Graphics.MapEngine {
    public abstract class Camera {
        private Matrix projection;
        private Matrix view;

        public Camera(GraphicsDevice graphicsDevice) {
            GraphicsDevice = graphicsDevice;

            generatePerspectiveProjectionMatrix(MathHelper.ToRadians(45));
        }

        public Vector3 Position { get; set; }

        public Matrix Projection {
            get { return projection; }
            protected set {
                projection = value;
                generateFrustum();
            }
        }

        public Matrix View {
            get { return view; }
            set {
                view = value;
                generateFrustum();
            }
        }

        public BoundingFrustum Frustum { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        private void generatePerspectiveProjectionMatrix(float FieldOfView) {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            int aspectRatio = Main.resx/Main.resy;

            Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, aspectRatio, 0.1f, 1000000.0f);
        }

        public virtual void Update() {}

        public void generateFrustum() {
            Frustum = new BoundingFrustum(View*Projection);
        }

        public bool BoundingVolumeIsInView(BoundingSphere sphere) {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        public bool BoundingVolumeIsInView(BoundingBox box) {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }


    public class FreeCamera : Camera {
        public Vector3 Position;
        public float RotationF;
        public Vector3 Target;
        public float cameradistance = 30;
        public Vector3 translation;

        public FreeCamera(Vector3 Position, float Yaw, float Pitch,
                          GraphicsDevice graphicsDevice)
            : base(graphicsDevice) {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            Roll = 0;

            translation = Vector3.Zero;
        }

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }

        public void Rotate(float YawChange, float PitchChange, float _Roll) {
            Yaw += YawChange;
            Pitch += PitchChange;
            Roll += _Roll;
        }

        public void Move(Vector3 Translation) {
            translation += Translation;
        }

        public void Update(Vector3 moving) {
            //// Calculate the rotation matrix
            //Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            //// Offset the position and reset the translation
            //translation = Vector3.Transform(translation, rotation);
            //Position += translation;
            //translation = Vector3.Zero;

            //// Calculate the new target
            //Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            //Target = Position + forward;

            //// Calculate the up vector
            //Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            //// Calculate the view matrix
            //View = Matrix.CreateLookAt(Position, Target, up);

            //this.Up = up;
            //this.Right = Vector3.Cross(forward, up);
            Target += moving;
            View = BuildViewMatrix(Target, (float) Math.PI/5, 0, MathHelper.ToRadians(RotationF), cameradistance);
            generateFrustum();
            Matrix invv = Matrix.Invert(View);
            Position.X = invv.M41;
            Position.Y = invv.M42;
            Position.Z = invv.M43;
        }

        /// <summary>
        /// Вращение камеры вокруг объекта
        /// </summary>
        /// <param name="cameraTarget">Координаты объекта</param>
        /// <param name="cameraRotationX">Вращение вокруг X</param>
        /// <param name="cameraRotationY">Вращение вокруг Y</param>
        /// <param name="cameraTargetDistance">Расстояние от камеры до объекта</param>
        /// <returns></returns>
        public static Matrix BuildViewMatrix(Vector3 cameraTarget, float cameraRotationX, float cameraRotationY,
                                             float cameraRotationZ, float cameraTargetDistance) {
            //матрица вращений
            Matrix cameraRot = Matrix.CreateRotationX(cameraRotationX)
                               *Matrix.CreateRotationY(cameraRotationY)*Matrix.CreateRotationZ(cameraRotationZ);

            //вычисляем куда смотреть
            var cameraOriginalTarget = new Vector3(0, 0, -1);
            var cameraOriginalUpVector = new Vector3(0, 1, 0);

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRot);
            Vector3 cameraFinalTarget = cameraTarget + cameraRotatedTarget;
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRot);
            Vector3 cameraFinalUpVector = cameraTarget + cameraRotatedUpVector;

            //смотрим на объект
            Matrix viewMatrix = Matrix.CreateLookAt(cameraTarget, cameraFinalTarget, cameraRotatedUpVector);

            //отдаляем камеру на нужное расстояние от объекта
            viewMatrix.Translation += new Vector3(0, 0, -cameraTargetDistance);

            return viewMatrix;
        }
    }
}