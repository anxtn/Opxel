﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    internal class ChunkLayer
    {
        public bool IsEmpty {  get;private set; }
        public int[]? Blocks;
        public readonly int YPosition;

        public ChunkLayer(int yPosition)
        {
            IsEmpty = true; 
            this.YPosition = yPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBlock(int x, int z, int block)
        {

            if(IsEmpty)
            {
                Blocks = new int[Chunk.LayerSize];
                IsEmpty = false;
            }

            Blocks[z * Chunk.SizeX + x] = block;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetBlock(int x, int z)
        {
            if(IsEmpty)
            {
                return 0; // 0 = Air
            }
            return Blocks![z * Chunk.SizeX + x];
        }

        public int this[int x, int z] => GetBlock(x, z);
    }
}
