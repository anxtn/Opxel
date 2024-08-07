﻿using OpenTK.Mathematics;
using Opxel.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Opxel.Voxels
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BlockVertex
    {
        public float X;
        public float Y;
        public float Z;

        public float S;
        public float T;

        public float NormalX;
        public float NormalY;
        public float NormalZ;

        public BlockVertex(float x, float y, float z)
        {
            this.X = x; 
            this.Y = y;
            this.Z = z;
        }

        public BlockVertex(Vector3i integerPosition, Vector2i integerUv)
        {
            this.X = integerPosition.X;
            this.Y = integerPosition.Y;
            this.Z = integerPosition.Z;

            this.S = integerUv.X / 16f;
            this.T = integerUv.Y / 16f;
        }

        public BlockVertex(Vector3i integerPosition, Vector2i integerUv, FaceDirection normalDirection)
        {
            this.X = integerPosition.X;
            this.Y = integerPosition.Y;
            this.Z = integerPosition.Z;

            this.S = integerUv.X / 16f;
            this.T = integerUv.Y / 16f;

            switch(normalDirection) {
                case FaceDirection.XPositive:
                    NormalX = 1;
                    NormalY = 0;
                    NormalZ = 0;
                    break;
                case FaceDirection.XNegative:
                    NormalX = -1;
                    NormalY = 0;
                    NormalZ = 0;
                    break;
                case FaceDirection.YPositive:
                    NormalX = 0;
                    NormalY = 1;
                    NormalZ = 0;
                    break;
                case FaceDirection.YNegative:
                    NormalX = 0;
                    NormalY = -1;
                    NormalZ = 0;
                    break;
                case FaceDirection.ZPositive:
                    NormalX = 0;
                    NormalY = 0;
                    NormalZ = 1;
                    break;
                case FaceDirection.ZNegative:
                    NormalX = 0;
                    NormalY = 0;
                    NormalZ = -1;
                    break;
            }
        }
    }
}
