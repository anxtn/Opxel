using OpenTK.Mathematics;
using Opxel.Debug;
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
        public readonly int ChunkLoadDistance = Chunk.SizeX * 16 + 8;
        private Vector3i[] ChunkLoadOffsets;

        public ChunkManager(OpxelWorld world)
        {
            this.World = world;
            ActiveChunks = new Dictionary<Vector3i, Chunk>();
            ChunkLoadOffsets = CalcChunkLoadOffsets(ChunkLoadDistance);
            world.BlockShaderProgram.Use();
            world.BlockShaderProgram.SetUniform("uRenderDistance", (float)(ChunkLoadDistance-10));
        }

        private static Vector3i[] CalcChunkLoadOffsets(int radius)
        {
            List<Vector3i> offsets = new List<Vector3i>();
            for(int dx = -radius;dx <= radius;dx += Chunk.SizeX)
            {
                for(int dz = -radius;dz <= radius;dz += Chunk.SizeZ)
                {
                    if(Vector2.Distance(Vector2.Zero, new Vector2(dx, dz)) <= radius)
                    {
                        offsets.Add(new Vector3i(dx, 0, dz));
                    }
                }
            }

            Vector3i[] result = [.. offsets.OrderBy((pos) => pos.EuclideanLength)];
            return result;
        }

        public void Update()
        {
            Vector3 playerPosition = World.Player.Transform.Position;
            Vector3i chunkPosition = World.Player.GetChunkPosition();

            //Chunk deleting
            foreach((Vector3i position, Chunk chunk) in ActiveChunks)
            {
                if((Vector2.Distance(position.Xz, chunkPosition.Xz) > (ChunkLoadDistance + Chunk.SizeX / 2)))
                {
                    ActiveChunks.Remove(position);
                    chunk.Dispose();
                    break;
                }
            }

            //Chunk loading
            foreach(Vector3i chunkOffset in ChunkLoadOffsets)
            {
                Vector3i iterPos = chunkPosition + chunkOffset;
                if(!IsChunkLoaded(iterPos))
                {
                    Chunk newChunk = new Chunk(World, iterPos);
                    ActiveChunks.Add(iterPos, newChunk);
                    break;
                }
            }
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
