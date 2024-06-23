using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Mathematics
{
    internal class Transform
    {
        private Transform? _parent;

        public Transform? Parent
        {
            get { return _parent; }
            set {

                _parent?._children.Remove(this);
                _parent = value;

                if(_parent == null)
                {
                    relativeTo = Matrix4.Identity;
                }
                else
                {
                    _parent._children.Add(this);
                    relativeTo = _parent.ModelMatrix;
                }
                    
            }
        }

        private List<Transform> _children;

        public IReadOnlyList<Transform> Children;


        private Matrix4 _relativeTo;

        public Matrix4 RelativeTo
        {
            get { return _relativeTo; }
            private set { 
                UpdateModelMatrix();
                _relativeTo = value;
            }
        }


        public Matrix4 relativeTo
        {
            get { return RelativeTo; }
            set { RelativeTo = value;
                UpdateModelMatrix(); }
        }


        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateTranslationMatrix();
            }
        }

        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                UpdateScaleMatrix();
            }
        }

        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                UpdateRotationMatrix();
            }
        }

        private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;

        public Matrix4 TranslationMatrix { get; private set; }
        public Matrix4 ScaleMatrix { get; private set; }
        public Matrix4 ModelMatrix { get; private set; }
        public Matrix4 RotationMatrix { get; private set; }
        public Transform()
        {
            _children = new List<Transform>();
            Children = _children.AsReadOnly();
            Parent = null;
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Quaternion.FromEulerAngles(0, 0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateModelMatrix()
        {
            //TRS convention
            ModelMatrix = TranslationMatrix * RotationMatrix * ScaleMatrix * relativeTo;
            for(int i = 0; i < _children.Count;i++)
            {
                _children[i].RelativeTo = ModelMatrix;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateTranslationMatrix()
        {
            TranslationMatrix = Matrix4.CreateTranslation(_position);
            UpdateModelMatrix();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateScaleMatrix()
        {
            ScaleMatrix = Matrix4.CreateScale(_scale);
            UpdateModelMatrix();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateRotationMatrix()
        {
            RotationMatrix = Matrix4.CreateFromQuaternion(_rotation);
            UpdateModelMatrix();
        }

        public void Translate(Vector3 position)
        {
            this.Position = position;
        }

        //angles in degrees
        public void RotateByDegrees(Vector3 eulerAngles)
        {
            this.Rotation = Quaternion.FromEulerAngles(eulerAngles);
        }

        //angles in radiants
        public void RotateByRadiants(Vector3 pitchYawRoll)
        {
            this.Rotation = Quaternion.FromEulerAngles(pitchYawRoll.X, pitchYawRoll.Y, pitchYawRoll.Z);
        }

        //angles in radiants
        public void RotateByRadiants(float pitch, float yaw, float roll)
        {
            this.Rotation = Quaternion.FromEulerAngles(pitch, yaw, roll);
        }
    }
}
