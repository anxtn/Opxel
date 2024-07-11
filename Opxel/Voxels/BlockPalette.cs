using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    internal class BlockPalette
    {
        public BlockProperty[] Blocks;

        public BlockPalette(BlockProperty[] blocks)
        {
            this.Blocks = blocks;
        }

        public bool HasBlockTag(int block, BlockTags blockTag)
        {
            return Blocks[(int)block].Tags.HasFlag(blockTag);
        }

        public static BlockPalette Default = new BlockPalette(new[]{
            //id: 0 Air
            new BlockProperty("Air",Vector2i.Zero,Vector2i.Zero,Vector2i.Zero,BlockTags.NoMesh | BlockTags.Transparent),

            //id: 1 Grass
            new BlockProperty("Grass",new Vector2i(0,0),new Vector2i(1,0),new Vector2i(2,0)),

            //id:2 Stone
            new BlockProperty("Stone", new Vector2i(3,0),new Vector2i(3,0),new Vector2i(3,0))
        });
    }
}
