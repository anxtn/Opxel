﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    internal class Chunk : IDisposable
    {
        public readonly OpxelWorld World;
        public readonly ChunkMesh ChunkMesh;
        public readonly ChunkBlockData BlockData;
        public readonly Vector3i Position;

        public static readonly int SizeX = 16;
        public static readonly int SizeY = 256;
        public static readonly int SizeZ = 16;

        public static readonly int LayerSize = SizeX * SizeZ;
        public static readonly int VolumeSize = SizeX * SizeY * SizeZ;

        private bool disposed;

        public Chunk(OpxelWorld world, Vector3i position)
        {
            this.World = world;
            this.Position = position;
            ChunkMesh = new ChunkMesh(this);
            BlockData = new ChunkBlockData(this);

            disposed = false;

            GenerateTerrain();
            ChunkMesh.GenerateMesh();
        }



        public void GenerateTerrain()
        {
            Random rnd = new Random();
            for(int x = 0;x < SizeX;x++)
            {
                for(int z = 0;z < SizeZ;z++)
                {
                    int height = (int)(MathF.Abs(World.WorldGenerator.GetHeight(Position.X + x, Position.Z + z) * 10f)) + 2;
                    for(int y = 0;y < height;y++)
                        BlockData.SetBlock(x, y, z, rnd.Next() % 2 == 0 ? 1 : 2);
                }
            }
        }



        public bool IsInside(int x, int y, int z)
        {
            return (x < SizeX) || (y < SizeY) || (z < SizeZ);
        }

        public static Vector3i PositionToChunkPosition(Vector3 Position)
        {
            return new Vector3i(
                    (int)(Position.X - (Position.X % SizeX)),
                    (int)(Position.Y - (Position.Y % SizeY)),
                    (int)(Position.Z - (Position.Z % SizeZ))
                );
        }

        public bool IsPositionInside(Vector3i worldPosition)
        {
            return (worldPosition.X >= Position.X) && (worldPosition.X < (Position.X + SizeX)) && (worldPosition.Z >= Position.Z) && (worldPosition.Z < (Position.Z + SizeZ));
        }

        public void Dispose()
        {
            if(disposed) return;

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

