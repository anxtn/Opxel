using OpenTK.Mathematics;
using Opxel.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Graphics
{
    internal class Camera
    {
        public Transform Transform;

        public Vector3 Front { get; private set; } = Vector3.UnitZ;
        public Vector3 Up { get; private set; } = Vector3.UnitY;
        public Vector3 Right { get; private set; } = Vector3.UnitX;

        public float FOV
        {
            get => _fov;
            set
            {
                _fov = value;
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(_fov, _aspectRatio, 0.01f, 300f);
                UpdateMatrices();
            }
        }

        public float AspectRation
        {
            get => _aspectRatio;
            set
            {
                _aspectRatio = value;
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(_fov, _aspectRatio, 0.01f, 300f);
                UpdateMatrices();
            }
        }

        public Matrix4 ProjectionMatrix { get; private set; }
        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ViewProjectionMatrix { get; private set; }
        protected float _fov = MathF.PI / 2.2f;
        protected float _aspectRatio = 16f / 9f;

        public Camera()
        {
            Transform = new Transform();
        }

        public Camera(Transform transform)
        {
            Transform = transform;
        }

        public void OnTransformUpdate() {
            Matrix3 rotMatrix = new Matrix3(Transform.RotationMatrix);
            Front = rotMatrix * Vector3.UnitZ ;
            Right = rotMatrix * (-Vector3.UnitX);
            Up = rotMatrix * Vector3.UnitY;
            UpdateMatrices();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateMatrices()
        {
            ViewMatrix = Matrix4.LookAt(Transform.Position, Transform.Position + Front, Up);
            ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
        }
    }

}
