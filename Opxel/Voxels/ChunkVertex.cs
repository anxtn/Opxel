using OpenTK.Mathematics;
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
//pack last 2 values to shot or smth

namespace Opxel.Voxels
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ChunkVertex
    {
        public float X;
        public float Y;
        public float Z;

        public float S;
        public float T;

        public ChunkVertex(float x, float y, float z)
        {
            this.X = x; 
            this.Y = y;
            this.Z = z;
        }

        public ChunkVertex(Vector3i integerPosition, Vector2i integerUv)
        {
            this.X = integerPosition.X;
            this.Y = integerPosition.Y;
            this.Z = integerPosition.Z;

            this.S = integerUv.X / 16f;
            this.T = integerUv.Y / 16f;
        }

        public ChunkVertex(Vector3i integerPosition)
        {
            this.X = integerPosition.X;
            this.Y = integerPosition.Y;
            this.Z = integerPosition.Z;
        }
    }
}
