using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Opxel.Input;
using Opxel.Graphics;

namespace Opxel
{
    internal class FlyingCamera : Camera
    {
        public float Speed = 4f;
        public float MouseSensitivity = 14f;
        public float MinPitch = -MathF.PI / 2f;
        public float MaxPitch = 1;

        public FlyingCamera() : base()
        {
            FOV = _fov;
        }

        public void Update(float deltaTime)
        {
            if(OpxelInput.IsMouseButtonDown(MouseButton.Button1) && OpxelInput.MouseDelta != Vector2.Zero)
            {
                Rotate(_pitch - OpxelInput.MouseDelta.Y * MouseSensitivity * deltaTime, _yaw + OpxelInput.MouseDelta.X * MouseSensitivity * deltaTime);

            }

            float speedMultiplier = OpxelInput.IsKeyDown(Keys.LeftShift) ? 2f : 1f;

            if(OpxelInput.IsKeyDown(Keys.W))
            {
                Position += Front * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.S))
            {
                Position -= Front * deltaTime * Speed * speedMultiplier;
            }


            if(OpxelInput.IsKeyDown(Keys.D))
            {
                Position += Right * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.A))
            {
                Position -= Right * deltaTime * Speed * speedMultiplier;
            }

            if(OpxelInput.IsKeyDown(Keys.Space))
            {
                Position += Vector3.UnitY * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.LeftControl))
            {
                Position -= Vector3.UnitY * deltaTime * Speed * speedMultiplier;
            }
        }
    }
}
