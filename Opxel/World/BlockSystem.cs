using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.World
{
    internal class BlockSystem
    {
        public readonly ChunkManager ChunkManager;

        public BlockSystem(OpxelWorld world)
        {
            this.ChunkManager = world.ChunkManager;
        }

        public bool Raycast(Vector3 worldStart, Vector3 normDirection, float length, out Vector3 hitWorldPos, out Vector3i hitWorldBeforeBlockPos)
        {
            const float raycastStepSize = 0.09f;
            Vector3 step = raycastStepSize * normDirection;

            for(Vector3 currentPos = worldStart;(currentPos - worldStart).Length <= length;currentPos += step)
            {
                if (ChunkManager.GetBlock((Vector3i)currentPos) != 0)
                {
                    hitWorldPos = currentPos;
                    hitWorldBeforeBlockPos = (Vector3i)currentPos;
                    return true;
                }
            }

            hitWorldPos = new Vector3(float.NaN);
            hitWorldBeforeBlockPos = new Vector3i(int.MaxValue - 1);
            return false;
        }
    }
}
