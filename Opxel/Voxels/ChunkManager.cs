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

        public readonly Dictionary<Vector3i, Chunk> ActiveChunks;
        public readonly Queue<Chunk> DeleteQueue;
        public readonly Queue<Vector3i> LoadQueue;
        public int chunkLoadDistance { get; set; } = Chunk.SizeX * 7 + 8;
        private Vector3i[] ChunkLoadOffsets;

        private bool deleteFlag;

        public ChunkManager(OpxelWorld world)
        {
            this.World = world;
            ActiveChunks = new Dictionary<Vector3i, Chunk>();
            DeleteQueue = new Queue<Chunk>();
            LoadQueue = new Queue<Vector3i>();
            deleteFlag = false;
            ChunkLoadOffsets = CalcChunkLoadOffsets(chunkLoadDistance);
        }

        private static Vector3i[] CalcChunkLoadOffsets(int radius)
        {
            List<Vector3i> offsets = new List<Vector3i>();
            for(int dx = -radius;dx <= radius;dx += Chunk.SizeX)
            {
                for(int dz = -radius;dz <= radius;dz += Chunk.SizeZ)
                {
                    if(Vector2.Distance(Vector2.Zero, new Vector2(dx,dz)) <= radius)
                    {
                        offsets.Add(new Vector3i(dx,0,dz));
                    }
                }
            }

            return offsets.ToArray();
        }

        // per update only one chunk is loaded or deleted 
        public void Update()
        {
            Vector3 playerPosition = World.Player.Transform.Position;
            Vector3i chunkPosition = GetPositionOfChunk((Vector3i)playerPosition);

            foreach(Vector3i chunkOffset in ChunkLoadOffsets)
            {
                Vector3i iterPos = chunkPosition + chunkOffset;
                if(!IsChunkLoaded(iterPos) && !LoadQueue.Contains(iterPos))
                {
                    LoadQueue.Enqueue(iterPos);
                }
            }

            foreach(KeyValuePair<Vector3i, Chunk> chunk in ActiveChunks.Where((posChunk) => Vector2.Distance(posChunk.Key.Xz, playerPosition.Xz) > (chunkLoadDistance + 1) && !DeleteQueue.Contains(posChunk.Value)))
            {
                DeleteQueue.Enqueue(chunk.Value);
            }

            deleteFlag = !deleteFlag;

            if(deleteFlag && DeleteQueue.Count > 0)
            {
                Chunk chunk = DeleteQueue.Dequeue();
                ActiveChunks.Remove(chunk.Position);
                chunk.Dispose();
            }

            //chunk loading
            if(!deleteFlag && LoadQueue.Count > 0)
            {
                Vector3i position = LoadQueue.Dequeue();
                Chunk newChunk = new Chunk(World, position);
                ActiveChunks.Add(position, newChunk);

                if(TryGetNeighbour(position, FaceDirection.ZPositive, out var neighbourZPos))
                {
                    neighbourZPos!.ChunkMesh.GenerateSideFacesZNegative(newChunk.BlockData);
                    newChunk.ChunkMesh.GenerateSideFacesZPositive(neighbourZPos.BlockData);
                }

            }
        }

        public Vector3i GetPositionOfChunk(Vector3i worldPosition)
        {
            return new Vector3i((int)(worldPosition.X - (worldPosition.X % Chunk.SizeX)),
                    0,
                    (int)(worldPosition.Z - (worldPosition.Z % Chunk.SizeZ)));
        }

        public bool IsChunkLoaded(Vector3i worldPosition)
        {
            return ActiveChunks.ContainsKey(worldPosition);
        }

        public void RenderChunks()
        {
            foreach(Chunk chunk in ActiveChunks.Values)
            {
                chunk.ChunkMesh.Render();
            }
        }

        public Chunk GetNeighbour(Vector3i worldPosition, FaceDirection direction)
        {
            Vector3i offset = direction switch
            {
                FaceDirection.XPositive => new Vector3i(Chunk.SizeX, 0, 0),
                FaceDirection.XNegative => new Vector3i(-Chunk.SizeX, 0, 0),
                FaceDirection.ZPositive => new Vector3i(0, 0, Chunk.SizeZ),
                FaceDirection.ZNegative => new Vector3i(0, 0, -Chunk.SizeZ),
                _ => throw new NotImplementedException()
            };

            return ActiveChunks[worldPosition + offset];
        }

        public bool TryGetNeighbour(Vector3i worldPosition, FaceDirection direction, out Chunk? neighbourChunk)
        {
            Vector3i offset = direction switch
            {
                FaceDirection.XPositive => new Vector3i(Chunk.SizeX, 0, 0),
                FaceDirection.XNegative => new Vector3i(-Chunk.SizeX, 0, 0),
                FaceDirection.ZPositive => new Vector3i(0, 0, Chunk.SizeZ),
                FaceDirection.ZNegative => new Vector3i(0, 0, -Chunk.SizeZ),
                _ => throw new NotImplementedException()
            };

            return ActiveChunks.TryGetValue(worldPosition + offset, out neighbourChunk);
        }
    }
}
