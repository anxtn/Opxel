using Opxel.Application;
using Opxel.Generation;
using Opxel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    internal class OpxelWorld
    {
        public readonly BlockPalette BlockPalette;
        public readonly ShaderProgram BlockShaderProgram;
        public readonly PixelTexture BlockTexture;
        public readonly WorldDataLoader WorldDataLoader;
        public readonly ChunkManager ChunkManager;
        public readonly OpxelPlayer Player;

        public OpxelWorld(BlockPalette blockPalette, ShaderProgram blockShaderProgram, PixelTexture blockTexture)
        {
            this.BlockPalette = blockPalette;
            this.BlockShaderProgram = blockShaderProgram;
            this.BlockTexture = blockTexture;

            WorldDataLoader = new WorldDataLoader(this);
            ChunkManager = new ChunkManager(this);
            Player = new OpxelPlayer();
        }
    }
}
