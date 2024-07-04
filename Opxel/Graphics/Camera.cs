using OpenTK.Mathematics;
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
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateMatrices();
            }
        }

        public Vector3 Front { get; private set; } = new Vector3(0, 0, 1);
        public Vector3 Up { get; private set; } = new Vector3(0, 1, 0);
        public Vector3 Right { get; private set; } = new Vector3(-1, 0, 0);

        public float FOV
        {
            get => _fov;
            set
            {
                _fov = value;
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(_fov, _aspectRatio, 0.01f, 100f);
                UpdateMatrices();
            }
        }

        public float AspectRation
        {
            get => _aspectRatio;
            set
            {
                _aspectRatio = value;
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(_fov, _aspectRatio, 0.01f, 100f);
                UpdateMatrices();
            }
        }

        public float Pitch
        {
            get => _pitch;
            set
            {
                Rotate(_yaw, value);
            }
        }

        public float Yaw
        {
            get { return _yaw; }
            set
            {
                Rotate(value, _pitch);
            }
        }

        public float Roll
        {
            get { return _roll; }
            set
            {
                Rotate(_pitch, _yaw, _roll);
            }
        }

        public Quaternion Rotation { get; private set; }

        public Matrix4 ProjectionMatrix { get; private set; }
        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ViewProjectionMatrix { get; private set; }

        protected Vector3 _position;
        protected float _fov = MathF.PI / 2.2f;
        protected float _aspectRatio = 16f / 9f;
        protected float _pitch = 0f;
        protected float _yaw = 0f;
        protected float _roll = 0f;

        public Camera()
        {
            FOV = _fov;
            Pitch = 0;
            Yaw = 0;
        }

        public void Rotate(float pitch, float yaw)
        {
            this._pitch = pitch;
            this._yaw = yaw;
            Rotation = Quaternion.FromEulerAngles(pitch, yaw, _roll);
            Matrix3 rot = Matrix3.CreateFromQuaternion(Rotation);
            Front = rot * Vector3.UnitZ;
            Right = rot * (-Vector3.UnitX);
            Up = rot * Vector3.UnitY;
            UpdateMatrices();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(Quaternion rotation)
        {
            this.Rotation = rotation;
            _yaw = MathF.Atan2(2.0f * (rotation.Y * rotation.W + rotation.X * rotation.Z), 1.0f - 2.0f * (rotation.X * rotation.X + rotation.Y * rotation.Y));
            _pitch = MathF.Asin(2.0f * (rotation.X * rotation.W - rotation.Y * rotation.Z));
            _roll = MathF.Atan2(2.0f * (rotation.X * rotation.Y + rotation.Z * rotation.W), 1.0f - 2.0f * (rotation.X * rotation.X + rotation.Z * rotation.Z));
            Matrix3 rot = Matrix3.CreateFromQuaternion(rotation);
            Front = rot * Vector3.UnitZ;
            Right = rot * (-Vector3.UnitX);
            Up = rot * Vector3.UnitY;
            UpdateMatrices();
        }

        //Es wird in Gradmaß gemessen
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(Vector3 eulerAngles)
        {
            Rotation = Quaternion.FromEulerAngles(eulerAngles);
            _pitch = eulerAngles.X; MathHelper.DegreesToRadians(eulerAngles.X);
            _yaw = eulerAngles.Y; MathHelper.DegreesToRadians(eulerAngles.Y);
            _roll = eulerAngles.Z; MathHelper.DegreesToRadians(eulerAngles.Z);
            Matrix3 rot = Matrix3.CreateFromQuaternion(Rotation);
            Front = rot * Vector3.UnitZ;
            Right = rot * (-Vector3.UnitX);
            Up = rot * Vector3.UnitY;
            UpdateMatrices();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(float pitch, float yaw, float roll)
        {
            this._pitch = pitch;
            this._yaw = yaw;
            this._roll = roll;
            Rotation = Quaternion.FromEulerAngles(pitch, yaw, roll);
            Matrix3 rot = Matrix3.CreateFromQuaternion(Rotation);
            Front = rot * Vector3.UnitZ;
            Right = rot * (-Vector3.UnitX);
            Up = rot * Vector3.UnitY;
            UpdateMatrices();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateMatrices()
        {
            ViewMatrix = Matrix4.LookAt(_position, _position + Front, Up);
            ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
        }
    }

}
