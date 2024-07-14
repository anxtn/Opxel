using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Generation
{
    internal class WorldGenerator
    { 

        public WorldGenerator()
        {
            
        }

        public float GetHeight(int x, int y)
        {
            float ax = x / 16f;
            float ay = y / 16f;
            return MathF.Abs((float)((MathF.Sin(ax) + Math.Sin(ay)) / 2f + (MathF.Sin(ax/2) + Math.Sin(ay/2))));
        }
    }
}
