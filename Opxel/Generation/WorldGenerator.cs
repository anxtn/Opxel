using OpenTK.Mathematics;
using Opxel.Voxels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Generation
{
    internal class WorldGenerator
    { 

        public WorldGenerator()
        {
            
        }

        public float GetHeight(int x, int y)
        {
            float ax = x / 16f;
            float ay = y / 16f;
            return MathF.Abs((float)((MathF.Sin(ax) + Math.Sin(ay)) / 2f + (MathF.Sin(ax/2) + Math.Sin(ay/2))));
        }

        public ChunkBlockData GenerateChunkData(Vector3i chunkPosition)
        {
            ChunkBlockData blockData = new ChunkBlockData();
            Random rnd = new Random();

            for(int x = 0;x < Chunk.SizeX;x++)
            {
                for(int z = 0;z < Chunk.SizeZ;z++)
                {
                    int height = (int)(MathF.Abs(GetHeight(chunkPosition.X + x, chunkPosition.Z + z) * 10f)) + 2;
                    for(int y = 0;y < height;y++)
                        blockData.SetBlock(x, y, z, rnd.Next() % 2 == 0 ? 1 : 2);
                }
            }

            return blockData;
        }
    }
}
