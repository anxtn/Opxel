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
        public readonly ChunkBlockData BlockData;
        public readonly Vector3i Position;


        public static readonly int Size = 32;
        public static readonly int LayerSize = Size * Size;
        public static readonly int VolumeSize = Size * Size * Size;

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
            for(int x = 0;x < Size;x++)
            {
                for(int z = 0;z < Size;z++)
                {
                    int height = (int)(World.WorldGenerator.GetHeight(Position.X + x, Position.Z + z) * 10f);
                    for(int y = 0;y < height;y++)
                        BlockData.SetBlock(x, y, z, 1);


                }
            }
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

