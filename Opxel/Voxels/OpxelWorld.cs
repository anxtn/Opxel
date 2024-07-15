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
        public readonly WorldGenerator WorldGenerator;
        public readonly Camera Camera;

        public OpxelWorld(BlockPalette blockPalette, ShaderProgram blockShaderProgram, PixelTexture blockTexture, Camera camera)
        {
            this.BlockPalette = blockPalette;
            this.BlockShaderProgram = blockShaderProgram;
            this.BlockTexture = blockTexture;
            this.WorldGenerator = new WorldGenerator();
            this.Camera = camera;
        }
    }
}
