using OpenTK.Mathematics;
using Opxel.Debug;
using Opxel.Generation;
using Opxel.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.World
{

    internal class ChunkManager
    {
        public readonly OpxelWorld World;
        public readonly WorldGenerator WorldGenerator;
        public readonly Dictionary<Vector3i, ChunkData> LoadedChunkData;
        public readonly Dictionary<Vector3i, Chunk> LoadedChunks;
        public readonly int ChunkLoadDistance = Chunk.SizeX * 16 + 8;

        private Vector3i[] chunkLoadOffsets;

        public ChunkManager(OpxelWorld world)
        {
            World = world;
            WorldGenerator = new WorldGenerator(World);
            LoadedChunkData = new Dictionary<Vector3i, ChunkData>();
            LoadedChunks = new Dictionary<Vector3i, Chunk>();
            chunkLoadOffsets = CalcChunkLoadOffsets(ChunkLoadDistance);
            world.BlockShaderProgram.Use();
            world.BlockShaderProgram.SetUniform("uRenderDistance", (float)(ChunkLoadDistance - 10));
        }

        private static Vector3i[] CalcChunkLoadOffsets(int radius)
        {
            List<Vector3i> offsets = new List<Vector3i>();
            for (int dx = -radius; dx <= radius; dx += Chunk.SizeX)
            {
                for (int dz = -radius; dz <= radius; dz += Chunk.SizeZ)
                {
                    if (Vector2.Distance(Vector2.Zero, new Vector2(dx, dz)) <= radius)
                    {
                        Vector3i worldChunkPos = new Vector3i(dx - dx % Chunk.SizeX, 0, dz - dz % Chunk.SizeZ);
                        offsets.Add(worldChunkPos);
                    }
                }
            }

            Vector3i[] result = [.. offsets.OrderBy((pos) => pos.EuclideanLength)];
            return result;
        }

        public ChunkData GetOrLoadChunkBlockData(Vector3i chunkPosition)
        {
            if(IsChunkDataLoaded(chunkPosition))
            {
                return LoadedChunkData[chunkPosition];
            }

            ChunkData data = WorldGenerator.GenerateChunkData(chunkPosition);
            LoadedChunkData.Add(chunkPosition, data);
            return data;
        }

        public void GetOrLoadNeighbourData(Vector3i chunkPosition, out ChunkData xPosData, out ChunkData xNegData, out ChunkData zPosData, out ChunkData zNegData)
        {
            Vector3i XPosNeightbour = chunkPosition + Vector3i.UnitX * Chunk.SizeX;
            Vector3i XNegNeightbour = chunkPosition - Vector3i.UnitX * Chunk.SizeX;
            Vector3i ZPosNeightbour = chunkPosition + Vector3i.UnitZ * Chunk.SizeZ;
            Vector3i ZNegNeightbour = chunkPosition - Vector3i.UnitZ * Chunk.SizeZ;

            xPosData = GetOrLoadChunkBlockData(XPosNeightbour);
            xNegData = GetOrLoadChunkBlockData(XNegNeightbour);
            zPosData = GetOrLoadChunkBlockData(ZPosNeightbour);
            zNegData = GetOrLoadChunkBlockData(ZNegNeightbour);
        }

        public void Update()
        {
            Vector3 playerPosition = World.Player.Transform.Position;
            Vector3i chunkPosition = World.Player.GetChunkPosition();

            //Chunk deleting
            foreach ((Vector3i position, Chunk chunk) in LoadedChunks)
            {
                if (Vector2.Distance(position.Xz, chunkPosition.Xz) > ChunkLoadDistance + Chunk.SizeX / 2)
                {
                    LoadedChunks.Remove(position);
                    chunk.Dispose();
                    break;
                }
            }

            //Chunk loading
            foreach (Vector3i chunkOffset in chunkLoadOffsets)
            {
                Vector3i iterPos = chunkPosition + chunkOffset;
                if (!IsChunkLoaded(iterPos))
                {
                    Chunk newChunk = new Chunk(World, iterPos);
                    LoadedChunks.Add(iterPos, newChunk);
                    break;
                }
            }
        }

        public bool IsChunkLoaded(Vector3i chunkWorldPosition)
        {
            return LoadedChunks.ContainsKey(chunkWorldPosition);
        }

        public bool IsChunkDataLoaded(Vector3i chunkPosition)
        {
            return LoadedChunkData.ContainsKey(chunkPosition);
        }

        public bool IsBlockLoaded(Vector3i blockWorldPosition)
        {
            Vector3i innerChunkPos = new Vector3i(
                blockWorldPosition.X % Chunk.SizeX,
                blockWorldPosition.Y,
                blockWorldPosition.Z % Chunk.SizeZ
                );

            Vector3i worldChunkPos = new Vector3i(
                    blockWorldPosition.X - innerChunkPos.X,
                    0,
                    blockWorldPosition.Z - innerChunkPos.Z
                );

            return IsChunkLoaded(worldChunkPos);
        }

        public int GetBlock(Vector3i worldBlockPosition)
        {
            Vector3i innerChunkPos = new Vector3i(
               worldBlockPosition.X % Chunk.SizeX,
               worldBlockPosition.Y,
               worldBlockPosition.Z % Chunk.SizeZ
               );

            Vector3i worldChunkPos = new Vector3i(
                    worldBlockPosition.X - innerChunkPos.X,
                    0,
                    worldBlockPosition.Z - innerChunkPos.Z
                );

            Console.WriteLine(worldChunkPos);

            return LoadedChunkData[worldChunkPos].GetBlock(innerChunkPos);
        }

        public bool TryGetBlock(Vector3i worldBlockPosition, out int blockId)
        {
            Vector3i innerChunkPos = new Vector3i(
                worldBlockPosition.X % Chunk.SizeX,
                worldBlockPosition.Y,
                worldBlockPosition.Z % Chunk.SizeZ
                );

            Vector3i worldChunkPos = new Vector3i(
                    worldBlockPosition.X - innerChunkPos.X,
                    0,
                    worldBlockPosition.Z - innerChunkPos.Z
                );

            if(LoadedChunkData.TryGetValue(worldChunkPos, out ChunkData? chunkData))
            {
                blockId = chunkData!.GetBlock(innerChunkPos);
                return true;
            }
            else
            {
                blockId = -1;
                return false;
            }
        }

        public void SetBlock(Vector3i worldBlockPosition, int blockId)
        {
            Vector3i innerChunkPos = new Vector3i(
               worldBlockPosition.X % Chunk.SizeX,
               worldBlockPosition.Y,
               worldBlockPosition.Z % Chunk.SizeZ
               );

            Vector3i worldChunkPos = new Vector3i(
                    worldBlockPosition.X - innerChunkPos.X,
                    0,
                    worldBlockPosition.Z - innerChunkPos.Z
                );

            LoadedChunkData[worldChunkPos].SetBlock(innerChunkPos,blockId);
        }

        public void UnloadChunkBlockData(Vector3i chunkPosition)
        {
            LoadedChunkData.Remove(chunkPosition);
        }

        public void UnloadUnusedBlockData(int count)
        {
            List<Vector3i> positionsToDelete = new List<Vector3i>();
            foreach((Vector3i position, ChunkData data) in LoadedChunkData)
            {
                if(Vector2.Distance(World.Player.Transform.Position.Xz, position.Xz) > World.ChunkManager.ChunkLoadDistance + Chunk.SizeX * 1.5f)
                {
                    positionsToDelete.Add(position);
                    count--;
                    if(count < 1)
                    {
                        break;
                    }
                }
            }

            foreach(Vector3i position in positionsToDelete)
            {
                UnloadChunkBlockData(position);
            }
        }

        public void RenderChunks()
        {
            foreach (Chunk chunk in LoadedChunks.Values)
            {
                chunk.ChunkMesh.Render();
            }
        }
    }
}
