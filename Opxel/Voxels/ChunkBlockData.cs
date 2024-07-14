using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    internal class ChunkBlockData
    {
        public readonly Chunk Chunk;
        public readonly ChunkLayer[] Layers;
        public int NoAirBlockCount { get; private set; }
        
        public ChunkBlockData(Chunk chunk)
        {
            this.Chunk = chunk;
            NoAirBlockCount = 0;
            Layers = new ChunkLayer[Chunk.Size];
            for(int i = 0;i < Layers.Length;i++) Layers[i] = new ChunkLayer(chunk);

        }
        public void SetBlock(int x, int y, int z, int block)
        {
#if CHECK_BLOCK_POSITION
            if(!Chunk.IsInside(x, y, z))
            {
                throw new ArgumentOutOfRangeException($"The block position was out of range (position: x:{x}, y:{y}, z:{z})");
            }
#endif
            if(block != 0 && GetBlock(x, y, z) == 0)
                NoAirBlockCount++;
            else if(GetBlock(x, y, z) != 0)
                NoAirBlockCount--;

            Layers[y].SetBlock(x, z, block);
        }

        public int GetBlock(int x, int y, int z)
        {
#if CHECK_BLOCK_POSITION
            if(!Chunk.IsInside(x, y, z))
            {
                throw new ArgumentOutOfRangeException($"The block position was out of range (position: x:{x}, y:{y}, z:{z})");
            }
#endif
            return Layers[y].GetBlock(x, z);
        }
    }
}
