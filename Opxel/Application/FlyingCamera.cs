﻿using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Opxel.Input;
using Opxel.Graphics;

namespace Opxel.Application
{
    internal class FlyingCamera : Camera
    {
        public float Speed { get; set; } = 4f;
        public float MouseSensitivity { get; set; } = 5f;
        public float MinPitch { get; set; } = -MathF.PI / 2f;
        public float MaxPitch { get; set; } = 1;

        private float _pitch;
        private float _yaw;

        public FlyingCamera() : base()
        {
            FOV = _fov;
        }

        public void Update(float deltaTime)
        {

            if(OpxelInput.IsMouseButtonDown(MouseButton.Button1) && OpxelInput.MouseDelta != Vector2.Zero)
            {
                _pitch -= OpxelInput.MouseDelta.Y * MouseSensitivity * deltaTime;
                _yaw += OpxelInput.MouseDelta.X * MouseSensitivity * deltaTime;
                Transform.RotateByRadiants(_pitch, _yaw,0);
            }

            float speedMultiplier = OpxelInput.IsKeyDown(Keys.LeftShift) ? 2f : 1f;

            if(OpxelInput.IsKeyDown(Keys.W))
            {
                Transform.Position += Front * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.S))
            {
                Transform.Position -= Front * deltaTime * Speed * speedMultiplier;
            }


            if(OpxelInput.IsKeyDown(Keys.D))
            {
                Transform.Position += Right * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.A))
            {
                Transform.Position -= Right * deltaTime * Speed * speedMultiplier;
            }

            if(OpxelInput.IsKeyDown(Keys.Space))
            {
                Transform.Position += Vector3.UnitY * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.LeftControl))
            {
                Transform.Position -= Vector3.UnitY * deltaTime * Speed * speedMultiplier;
            }

           base.OnTransformUpdate();
        }
    }
}
