using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{

    internal class ChunkManager
    {
        public readonly OpxelWorld World;

        public readonly List<Chunk> ActiveChunks;
        public readonly Queue<Chunk> DeleteQueue;
        public readonly Queue<Vector3i> LoadQueue;
        public int chunkLoadDistance = Chunk.Size * 3 + 16;

        private bool deleteFlag;

        public ChunkManager(OpxelWorld world)
        {
            this.World = world;
            ActiveChunks = new List<Chunk>();
            DeleteQueue = new Queue<Chunk>();  
            LoadQueue = new Queue<Vector3i>();
            deleteFlag = false;
        }

        public void Update(Vector3i playerPosition)
        {
            List<Chunk> standingChunks = new List<Chunk>();

            for(int dx = -chunkLoadDistance;dx <= chunkLoadDistance;dx += Chunk.Size)
            {
                for(int dz = -chunkLoadDistance;dz <= chunkLoadDistance;dz += Chunk.Size)
                {
                    Vector3i pos = GetPositionOfChunk(new Vector3i(playerPosition.X + dx, 0, playerPosition.Z + dz));
                   
                    if(Vector2.Distance(pos.Xz, playerPosition.Xz) <= chunkLoadDistance && !IsChunkLoaded(pos) && !LoadQueue.Contains(pos))
                    {
                        LoadQueue.Enqueue(pos);
                    }
                }
            }

            foreach(Chunk chunk in ActiveChunks.Where((c) => Vector2.Distance(c.Position.Xz, playerPosition.Xz) > chunkLoadDistance && !DeleteQueue.Contains(c)))
            {
                DeleteQueue.Enqueue(chunk);
            }

            deleteFlag = !deleteFlag;

            if(deleteFlag && DeleteQueue.Count > 0)
            {
                Chunk chunk = DeleteQueue.Dequeue();
                ActiveChunks.Remove(chunk);
                chunk.Dispose();
            }

            if(!deleteFlag && LoadQueue.Count > 0)
            {
                Chunk newChunk = new Chunk(World, LoadQueue.Dequeue());
                ActiveChunks.Add(newChunk);
            }
        }

        public Vector3i GetPositionOfChunk(Vector3i worldPosition)
        {
            return new Vector3i((int)(worldPosition.X - (worldPosition.X % Chunk.Size)),
                    (int)(worldPosition.Y - (worldPosition.Y % Chunk.Size)),
                    (int)(worldPosition.Z - (worldPosition.Z % Chunk.Size)));
        }

        public bool IsChunkLoaded(Vector3i worldPosition)
        {
            return ActiveChunks.Any((chunk) => chunk.IsPositionInside(worldPosition));
        }

        public void RenderChunks()
        {
            foreach(Chunk chunk in ActiveChunks)
            {
                chunk.ChunkMesh.Render();
            }
        }
    }
}
