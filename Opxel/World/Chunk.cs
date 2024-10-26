﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.World
{
    internal class Chunk : IDisposable
    {
        public readonly OpxelWorld World;
        public readonly ChunkMesh ChunkMesh;
        public readonly ChunkData BlockData;
        public readonly Vector3i Position; // Y is always 0

        private bool disposed;

        public static readonly int SizeX = 16;
        public static readonly int SizeY = 128;
        public static readonly int SizeZ = 16;
        public static readonly int LayerSize = SizeX * SizeZ;
        public static readonly int VolumeSize = SizeX * SizeY * SizeZ;

        public Chunk(OpxelWorld world, Vector3i position)
        {
            World = world;
            Position = position;
            ChunkMesh = new ChunkMesh(this);
            BlockData = World.ChunkManager.GetOrLoadChunkBlockData(position);

            disposed = false;
            ChunkMesh.GenerateMesh();
        }

        public bool IsInside(int x, int y, int z)
        {
            return x < SizeX || y < SizeY || z < SizeZ;
        }

        public static Vector3i WorldPositionToInnerChunkPosition(Vector3 Position)
        {
            return new Vector3i(
                    (int)(Position.X - Position.X % SizeX),
                    0,
                    (int)(Position.Z - Position.Z % SizeZ)
                );
        }

        public bool IsPositionInside(Vector3i worldPosition)
        {
            return worldPosition.X >= Position.X && worldPosition.X < Position.X + SizeX && worldPosition.Z >= Position.Z && worldPosition.Z < Position.Z + SizeZ;
        }

        public void Dispose()
        {
            if (disposed) return;

            disposed = true;
            ChunkMesh.Dispose();
            GC.SuppressFinalize(this);
        }

        ~Chunk()
        {
            Dispose();
        }
    }
}

