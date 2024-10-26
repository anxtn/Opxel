using Opxel.Application;
using Opxel.Generation;
using Opxel.Graphics;
using Opxel.Voxels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.World
{
    internal class OpxelWorld
    {
        public readonly BlockPalette BlockPalette;
        public readonly ShaderProgram BlockShaderProgram;
        public readonly Texture2D BlockTexture;
        public readonly ChunkManager ChunkManager;
        public readonly OpxelPlayer Player;
        public readonly BlockSystem BlockSystem;

        public OpxelWorld(BlockPalette blockPalette, ShaderProgram blockShaderProgram, Texture2D blockTexture)
        {
            BlockPalette = blockPalette;
            BlockShaderProgram = blockShaderProgram;
            BlockTexture = blockTexture;
            ChunkManager = new ChunkManager(this);
            Player = new OpxelPlayer(this);
            BlockSystem = new BlockSystem(this);
        }
    }
}
