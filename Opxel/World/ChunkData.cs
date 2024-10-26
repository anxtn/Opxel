using OpenTK.Mathematics;
using Opxel.Debug;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.World
{
    internal class ChunkData
    {
        public readonly ChunkLayer[] Layers;
        public readonly Vector3i ChunkPosition;
        public readonly ChunkManager ChunkManager;
        public int NoAirBlockCount { get; private set; }

        public ChunkData(ChunkManager chunkManager, Vector3i chunkPosition)
        {
            ChunkPosition = chunkPosition;
            ChunkManager = chunkManager;
            NoAirBlockCount = 0;
            Layers = new ChunkLayer[Chunk.SizeY];
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new ChunkLayer(i);
            }
        }

        public void SetBlock(Vector3i innerChunkPosition, int blockId)
        {
            SetBlock(innerChunkPosition.X, innerChunkPosition.Y, innerChunkPosition.Z, blockId);
            if(ChunkManager.IsChunkLoaded(ChunkPosition))
            {
                ChunkManager.LoadedChunks[ChunkPosition].ChunkMesh.GenerateMesh();
            }
        }

        public void SetBlock(int x, int y, int z, int blockId)
        {
#if CHECK_BLOCK_POSITION
            if(!Chunk.IsInside(x, y, z))
            {
                throw new ArgumentOutOfRangeException($"The block position was out of range (position: x:{x}, y:{y}, z:{z})");
            }
#endif
            if (blockId != 0 && GetBlock(x, y, z) == 0)
                NoAirBlockCount++;
            else if (GetBlock(x, y, z) != 0)
                NoAirBlockCount--;

            Layers[y].SetBlock(x, z, blockId);
        }

        public int GetBlock(int innerChunkPosX, int innerChunkPosY, int innerChunkPosZ)
        {
#if CHECK_BLOCK_POSITION
            if(!Chunk.IsInside(innerChunkPosX, innerChunkPosY, innerChunkPosZ))
            {
                throw new ArgumentOutOfRangeException($"The block position was out of range (position: x:{innerChunkPosX}, y:{innerChunkPosY}, z:{innerChunkPosZ})");
            }
#endif
            return Layers[innerChunkPosY].GetBlock(innerChunkPosX, innerChunkPosZ);
        }

        public int GetBlock(Vector3i innerChunkPos)
        {
#if CHECK_BLOCK_POSITION
            if(!Chunk.IsInside(innerChunkPos.X, innerChunkPos.Y, innerChunkPos.Z))
            {
                throw new ArgumentOutOfRangeException($"The block position was out of range (position: x:{x}, y:{y}, z:{z})");
            }
#endif
            return Layers[innerChunkPos.Y].GetBlock(innerChunkPos.X, innerChunkPos.Z);
        }

        ~ChunkData()
        {
            ChunkManager.UnloadChunkBlockData(ChunkPosition);
        }
    }
}
