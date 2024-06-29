using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//######## Vertexdata Concept #########
//
// 1   ubyte X FinalCoord =  (X / 16)
// 1   ubyte Y
// 1   ubyte Z
// 1   ubyte s
// 1   ubyte t
// 1.5 ubyte sunLight (4 Bit R, 4 Bit G, 4 Bit B)
// 1.5 ubyte environmentLight (4 Bit R, 4 Bit G, 4 Bit B) 

namespace Opxel.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ChunkVertex
    {
        public ulong packedValue;

        public ChunkVertex(uint x, uint y, uint z, uint s, uint t, uint sunLight /*Range 0 - 15  */, uint dirLight)
        {
            Pack(x,y,z,s,t,sunLight, dirLight);
        }

        public void Pack(uint x, uint y, uint z, uint s, uint t, uint sunLight /*Range 0 - 15  */, uint Light)
        {
            //packedValue = x | (y << 8) | (z << 16) | (s << 24) | (t << 28) | (sunLight << 36) | (dirLight << 40);
            packedValue = x | y; 
        }
    }
}
