using OpenTK.Mathematics;
using Opxel.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    internal class WorldDataLoader
    {
        public readonly OpxelWorld World;
        public readonly WorldGenerator WorldGenerator;

        public readonly Dictionary<Vector3i, ChunkBlockData> LoadedBlockData;

        public WorldDataLoader(OpxelWorld world)
        {
            this.World = world;
            WorldGenerator = new WorldGenerator();
            LoadedBlockData = new Dictionary<Vector3i, ChunkBlockData>();
        }

        public bool IsBlockDataLoaded(Vector3i chunkPosition)
        {
            return LoadedBlockData.ContainsKey(chunkPosition);
        }

        public void LoadNeighbourData(Vector3i chunkPosition, out ChunkBlockData xPosData, out ChunkBlockData xNegData, out ChunkBlockData zPosData, out ChunkBlockData zNegData)
        {
            Vector3i XPosNeightbour = chunkPosition + Vector3i.UnitX * Chunk.SizeX;
            Vector3i XNegNeightbour = chunkPosition - Vector3i.UnitX * Chunk.SizeX;
            Vector3i ZPosNeightbour = chunkPosition + Vector3i.UnitZ * Chunk.SizeZ;
            Vector3i ZNegNeightbour = chunkPosition - Vector3i.UnitZ * Chunk.SizeZ;

            xPosData = LoadChunkBlockData(XPosNeightbour);
            xNegData = LoadChunkBlockData(XNegNeightbour);
            zPosData = LoadChunkBlockData(ZPosNeightbour);
            zNegData = LoadChunkBlockData(ZNegNeightbour);
        }

        public ChunkBlockData LoadChunkBlockData(Vector3i chunkPosition)
        {
            if(IsBlockDataLoaded(chunkPosition))
            {
                return LoadedBlockData[chunkPosition];
            }

            ChunkBlockData blockData = new ChunkBlockData();
            Random rnd = new Random();

            for(int x = 0;x < Chunk.SizeX;x++)
            {
                for(int z = 0;z < Chunk.SizeZ;z++)
                {
                    int height = (int)(MathF.Abs(WorldGenerator.GetHeight(chunkPosition.X + x, chunkPosition.Z + z) * 10f)) + 2;
                    for(int y = 0;y < height;y++)
                        blockData.SetBlock(x, y, z, rnd.Next() % 2 == 0 ? 1 : 2);
                }
            }

            return blockData;
        }
    }
}
