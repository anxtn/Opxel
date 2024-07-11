using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    internal class Chunk : IDisposable
    {
        public readonly OpxelWorld World;
        public readonly ChunkMesh ChunkMesh;
        public readonly ChunkLayer[] Layers;
        public readonly Vector3i Position;
        public int NoAirBlockCount;

        public static readonly int Size = 32;
        public static readonly int LayerSize = 32 * 32;
        public static readonly int VolumeSize = 32 * 32 * 32;

        private bool disposed;

        public Chunk(OpxelWorld world, Vector3i position)
        {
            this.World = world;
            this.Position = position;
            ChunkMesh = new ChunkMesh(this);
            Layers = new ChunkLayer[Size];
            NoAirBlockCount = 0;
            for(int i = 0;i < Layers.Length;i++) Layers[i] = new ChunkLayer(this);

            disposed = false;

            GenerateTerrain();
            ChunkMesh.GenerateMesh();
        }



        public void GenerateTerrain()
        {
            Random rnd = new Random();
            for(int x = 0;x < Size;x++)
            {
                for(int z = 0;z < Size;z++)
                {
                    for(int y = 0;y < rnd.Next(Size);y++)
                    if(rnd.Next() % 2 == 0)
                        SetBlock(x, y, z, 1);
                    else
                        SetBlock(x, y, z, 2);

                }
            }
        }

        public void SetBlock(int x, int y, int z, int block)
        {
            if(!IsInside(x, y, z))
            {
                throw new ArgumentOutOfRangeException($"The block position was out of range (position: x:{x}, y:{y}, z:{z})");
            }
            if(block != 0 && GetBlock(x, y, z) == 0)
                NoAirBlockCount++;
            else if(GetBlock(x, y, z) != 0)
                NoAirBlockCount--;

            Layers[y].SetBlock(x, z, block);
        }

        public int GetBlock(int x, int y, int z)
        {
#if DEBUG
            if(!IsInside(x, y, z))
            {
                throw new ArgumentOutOfRangeException($"The block position was out of range (position: x:{x}, y:{y}, z:{z})");
            }
#endif
            return Layers[y].GetBlock(x, z);
        }

        public bool IsInside(int x, int y, int z)
        {
            return (x < Size) || (y < Size) || (z < Size);
        }

        public static Vector3i PositionToChunkPosition(Vector3 Position)
        {
            return new Vector3i(
                    (int)(Position.X - (Position.X % Size)),
                    (int)(Position.Y - (Position.Y % Size)),
                    (int)(Position.Z - (Position.Z % Size))
                );
        }

        public bool IsPositionInside(Vector3i worldPosition)
        {
            return (worldPosition.X >= Position.X) && (worldPosition.X < (Position.X + Size)) && (worldPosition.Z >= Position.Z) && (worldPosition.Z < (Position.Z + Size));
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

