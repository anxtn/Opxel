using OpenTK.Mathematics;
using Opxel.Generation;
using Opxel.Voxels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Application
{
    internal class WorldDataLoader
    {
        public readonly OpxelWorld World;
        public readonly WorldGenerator WorldGenerator;

        public readonly Dictionary<Vector3i, ChunkBlockData> LoadedBlockData;

        public WorldDataLoader(OpxelWorld world)
        {
            World = world;
            WorldGenerator = new WorldGenerator();
            LoadedBlockData = new Dictionary<Vector3i, ChunkBlockData>();
        }

        public bool IsChunkDataLoaded(Vector3i chunkPosition)
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
            if (IsChunkDataLoaded(chunkPosition))
            {
                return LoadedBlockData[chunkPosition];
            }

            return WorldGenerator.GenerateChunkData(chunkPosition);
        }
    }
}
