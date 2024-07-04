using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    [Flags]
    internal enum BlockTags
    {
        None = 0,
        NoMesh = 1,
        Transparent = 2,
    }
}
