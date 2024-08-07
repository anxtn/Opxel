﻿using OpenTK.Mathematics;
using Opxel.Voxels;

namespace Opxel.World
{
    internal class ChunkMeshBuilder
    {
        public readonly BlockPalette BlockPalette;
        public List<BlockVertex> VerticesList { get; set; }
        public List<uint> IndicesList { get; set; }

        public ChunkMeshBuilder(BlockPalette blockPalette)
        {
            BlockPalette = blockPalette;
            VerticesList = new List<BlockVertex>();
            IndicesList = new List<uint>();
        }

        public void AddBlockFace(Vector3i blockPosition, FaceDirection faceDirection, int block)
        {
            BlockVertex[] faceVertexData = new BlockVertex[4];
            uint[] faceIndices = new uint[4];

            switch (faceDirection)
            {
                case FaceDirection.XPositive:

                    faceIndices = new uint[]
                    {
                        0u + (uint)VerticesList.Count,1u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,
                        0u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,3u + (uint)VerticesList.Count
                    };

                    faceVertexData = new BlockVertex[]{
                        new BlockVertex(new Vector3i(1,1,0) + blockPosition,BlockPalette.Blocks[block].UVXPositive[0],FaceDirection.XPositive),
                        new BlockVertex(new Vector3i(1,1,1) + blockPosition,BlockPalette.Blocks[block].UVXPositive[1],FaceDirection.XPositive),
                        new BlockVertex(new Vector3i(1,0,1) + blockPosition,BlockPalette.Blocks[block].UVXPositive[2],FaceDirection.XPositive),
                        new BlockVertex(new Vector3i(1,0,0) + blockPosition,BlockPalette.Blocks[block].UVXPositive[3],FaceDirection.XPositive)
                    };

                    break;

                case FaceDirection.XNegative:

                    faceVertexData = new BlockVertex[]{
                        new BlockVertex(new Vector3i(0,1,0)+ blockPosition,BlockPalette.Blocks[block].UVXNegative[0],FaceDirection.XNegative),
                        new BlockVertex(new Vector3i(0,1,1)+ blockPosition,BlockPalette.Blocks[block].UVXNegative[1],FaceDirection.XNegative),
                        new BlockVertex(new Vector3i(0,0,1)+ blockPosition,BlockPalette.Blocks[block].UVXNegative[2],FaceDirection.XNegative),
                        new BlockVertex(new Vector3i(0,0,0)+ blockPosition,BlockPalette.Blocks[block].UVXNegative[3],FaceDirection.XNegative)
                    };

                    faceIndices = new uint[]
                    {

                        0u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,1u + (uint)VerticesList.Count,
                        0u + (uint)VerticesList.Count,3u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count
                    };

                    break;

                case FaceDirection.YPositive:

                    faceIndices = new uint[]
                    {
                        0u + (uint)VerticesList.Count,1u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,
                        0u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,3u + (uint)VerticesList.Count
                    };

                    faceVertexData = new BlockVertex[]{
                        new BlockVertex(new Vector3i(0,1,1) + blockPosition,BlockPalette.Blocks[block].UVYPositive[0],FaceDirection.YPositive),
                        new BlockVertex(new Vector3i(1,1,1) + blockPosition,BlockPalette.Blocks[block].UVYPositive[1],FaceDirection.YPositive),
                        new BlockVertex(new Vector3i(1,1,0) + blockPosition,BlockPalette.Blocks[block].UVYPositive[2],FaceDirection.YPositive),
                        new BlockVertex(new Vector3i(0,1,0) + blockPosition,BlockPalette.Blocks[block].UVYPositive[3],FaceDirection.YPositive)
                    };

                    break;

                case FaceDirection.YNegative:

                    faceVertexData = new BlockVertex[]{
                        new BlockVertex(new Vector3i(0,0,1)+ blockPosition,BlockPalette.Blocks[block].UVYNegative[0], FaceDirection.YNegative),
                        new BlockVertex(new Vector3i(1,0,1)+ blockPosition,BlockPalette.Blocks[block].UVYNegative[1], FaceDirection.YNegative),
                        new BlockVertex(new Vector3i(1,0,0)+ blockPosition,BlockPalette.Blocks[block].UVYNegative[2], FaceDirection.YNegative),
                        new BlockVertex(new Vector3i(0,0,0)+ blockPosition,BlockPalette.Blocks[block].UVYNegative[3], FaceDirection.YNegative)
                    };

                    faceIndices = new uint[]
                    {

                        0u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,1u + (uint)VerticesList.Count,
                        0u + (uint)VerticesList.Count,3u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count
                    };

                    break;

                case FaceDirection.ZPositive:

                    faceIndices = new uint[]
                    {
                        0u + (uint)VerticesList.Count,1u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,
                        0u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,3u + (uint)VerticesList.Count
                    };

                    faceVertexData = new BlockVertex[]{
                        new BlockVertex(new Vector3i(1,1,1) + blockPosition,BlockPalette.Blocks[block].UVZPositive[0],FaceDirection.ZPositive),
                        new BlockVertex(new Vector3i(0,1,1) + blockPosition,BlockPalette.Blocks[block].UVZPositive[1],FaceDirection.ZPositive),
                        new BlockVertex(new Vector3i(0,0,1) + blockPosition,BlockPalette.Blocks[block].UVZPositive[2],FaceDirection.ZPositive),
                        new BlockVertex(new Vector3i(1,0,1) + blockPosition,BlockPalette.Blocks[block].UVZPositive[3],FaceDirection.ZPositive)
                    };

                    break;

                case FaceDirection.ZNegative:

                    faceVertexData = new BlockVertex[]{
                        new BlockVertex(new Vector3i(1,1,0)+ blockPosition,BlockPalette.Blocks[block].UVZNegative[0],FaceDirection.ZNegative),
                        new BlockVertex(new Vector3i(0,1,0)+ blockPosition,BlockPalette.Blocks[block].UVZNegative[1],FaceDirection.ZNegative),
                        new BlockVertex(new Vector3i(0,0,0)+ blockPosition,BlockPalette.Blocks[block].UVZNegative[2],FaceDirection.ZNegative),
                        new BlockVertex(new Vector3i(1,0,0)+ blockPosition,BlockPalette.Blocks[block].UVZNegative[3],FaceDirection.ZNegative)
                    };

                    faceIndices = new uint[]
                    {

                        0u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,1u + (uint)VerticesList.Count,
                        0u + (uint)VerticesList.Count,3u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count
                    };

                    break;
                default: throw new System.ComponentModel.InvalidEnumArgumentException(nameof(faceDirection), (int)faceDirection, typeof(FaceDirection));
            }


            VerticesList.AddRange(faceVertexData);
            IndicesList.AddRange(faceIndices);
        }
    }

    internal enum FaceDirection : byte
    {
        XPositive,
        XNegative,
        YPositive,
        YNegative,
        ZPositive,
        ZNegative
    }
}
