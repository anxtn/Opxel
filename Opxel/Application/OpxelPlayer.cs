using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Opxel.Graphics;
using Opxel.Input;
using Opxel.Mathematics;
using Opxel.World;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace Opxel.Application
{
    internal class OpxelPlayer
    {
        public readonly Transform Transform;
        public readonly float BlockSetRange = 10f;
        public readonly OpxelWorld World;
        public float Speed { get; set; } = 4f;
        public float MouseSensitivity { get; set; } = 5f;
        public float MinPitch { get; set; } = -MathF.PI / 2f;
        public float MaxPitch { get; set; } = 1;

        public Camera Camera { get; set; }

        private float _pitch;
        private float _yaw;

        public OpxelPlayer(OpxelWorld world) : base()
        {
            this.World = world;
            Transform = new Transform();
            Camera = new Camera(Transform);
        }

        public Vector3i GetChunkPosition()
        {
            return new Vector3i((int)(Transform.Position.X - (Transform.Position.X % Chunk.SizeX)),
                    0,
                    (int)(Transform.Position.Z - (Transform.Position.Z % Chunk.SizeZ)));
        }

        private void MoveUpdate(float deltaTime)
        {
            if(OpxelInput.IsMouseButtonDown(MouseButton.Button1) && OpxelInput.MouseDelta != Vector2.Zero)
            {
                _pitch -= OpxelInput.MouseDelta.Y * MouseSensitivity * deltaTime;
                _yaw += OpxelInput.MouseDelta.X * MouseSensitivity * deltaTime;
                Transform.RotateByRadiants(_pitch, _yaw, 0);
            }

            float speedMultiplier = OpxelInput.IsKeyDown(Keys.LeftShift) ? 2f : 1f;

            if(OpxelInput.IsKeyDown(Keys.W))
            {
                Transform.Position += Camera.Front * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.S))
            {
                Transform.Position -= Camera.Front * deltaTime * Speed * speedMultiplier;
            }


            if(OpxelInput.IsKeyDown(Keys.D))
            {
                Transform.Position += Camera.Right * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.A))
            {
                Transform.Position -= Camera.Right * deltaTime * Speed * speedMultiplier;
            }

            if(OpxelInput.IsKeyDown(Keys.Space))
            {
                Transform.Position += Vector3.UnitY * deltaTime * Speed * speedMultiplier;
            }
            else if(OpxelInput.IsKeyDown(Keys.LeftControl))
            {
                Transform.Position -= Vector3.UnitY * deltaTime * Speed * speedMultiplier;
            }

            Camera.OnTransformUpdate();
        }

        public void BlockSetUpdate()
        {
            if(OpxelInput.IsMouseButtonDown(MouseButton.Right) && World.BlockSystem.Raycast(Transform.Position, Camera.Front, BlockSetRange,
     out Vector3 hitWorldPos, out Vector3i hitWorldBeforeBlockPos))
            {
                Console.WriteLine("Setted Block ()");
                World.ChunkManager.SetBlock(hitWorldBeforeBlockPos, 1);
            }
        }

        public void Update(float deltaTime)
        {
            BlockSetUpdate();
            MoveUpdate(deltaTime);
        }
    }
}

