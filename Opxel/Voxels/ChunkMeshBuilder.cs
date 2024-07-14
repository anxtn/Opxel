using OpenTK.Mathematics;
using Opxel.Voxels;

namespace Opxel.Voxels
{
    internal class ChunkMeshBuilder
    {
        public readonly BlockPalette BlockPalette;
        public List<ChunkVertex> VerticesList { get; set; }
        public List<uint> IndicesList { get; set; }

        public ChunkMeshBuilder(BlockPalette blockPalette)
        {
            this.BlockPalette = blockPalette;
            VerticesList = new List<ChunkVertex>();
            IndicesList = new List<uint>();
        }

        public void AddBlockFace(Vector3i blockPosition, FaceDirection faceDirection, int block)
        {
            ChunkVertex[] faceVertexData = new ChunkVertex[4];
            uint[] faceIndices = new uint[4];

            switch(faceDirection)
            {
                case FaceDirection.XPositive:

                    faceIndices = new uint[]
                    {
                        0u + (uint)VerticesList.Count,1u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,
                        0u + (uint)VerticesList.Count,2u + (uint)VerticesList.Count,3u + (uint)VerticesList.Count
                    };

                    faceVertexData = new ChunkVertex[]{
                        new ChunkVertex(new Vector3i(1,1,0) + blockPosition,BlockPalette.Blocks[(int)block].UVXPositive[0],FaceDirection.XPositive),
                        new ChunkVertex(new Vector3i(1,1,1) + blockPosition,BlockPalette.Blocks[(int)block].UVXPositive[1],FaceDirection.XPositive),
                        new ChunkVertex(new Vector3i(1,0,1) + blockPosition,BlockPalette.Blocks[(int)block].UVXPositive[2],FaceDirection.XPositive),
                        new ChunkVertex(new Vector3i(1,0,0) + blockPosition,BlockPalette.Blocks[(int)block].UVXPositive[3],FaceDirection.XPositive)
                    };

                    break;

                case FaceDirection.XNegative:

                    faceVertexData = new ChunkVertex[]{
                        new ChunkVertex(new Vector3i(0,1,0)+ blockPosition,BlockPalette.Blocks[(int)block].UVXNegative[0],FaceDirection.XNegative),
                        new ChunkVertex(new Vector3i(0,1,1)+ blockPosition,BlockPalette.Blocks[(int)block].UVXNegative[1],FaceDirection.XNegative),
                        new ChunkVertex(new Vector3i(0,0,1)+ blockPosition,BlockPalette.Blocks[(int)block].UVXNegative[2],FaceDirection.XNegative),
                        new ChunkVertex(new Vector3i(0,0,0)+ blockPosition,BlockPalette.Blocks[(int)block].UVXNegative[3],FaceDirection.XNegative)
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

                    faceVertexData = new ChunkVertex[]{
                        new ChunkVertex(new Vector3i(0,1,1) + blockPosition,BlockPalette.Blocks[(int)block].UVYPositive[0],FaceDirection.YPositive),
                        new ChunkVertex(new Vector3i(1,1,1) + blockPosition,BlockPalette.Blocks[(int)block].UVYPositive[1],FaceDirection.YPositive),
                        new ChunkVertex(new Vector3i(1,1,0) + blockPosition,BlockPalette.Blocks[(int)block].UVYPositive[2],FaceDirection.YPositive),
                        new ChunkVertex(new Vector3i(0,1,0) + blockPosition,BlockPalette.Blocks[(int)block].UVYPositive[3],FaceDirection.YPositive)
                    };

                    break;

                case FaceDirection.YNegative:

                    faceVertexData = new ChunkVertex[]{
                        new ChunkVertex(new Vector3i(0,0,1)+ blockPosition,BlockPalette.Blocks[(int)block].UVYNegative[0], FaceDirection.YNegative),
                        new ChunkVertex(new Vector3i(1,0,1)+ blockPosition,BlockPalette.Blocks[(int)block].UVYNegative[1], FaceDirection.YNegative),
                        new ChunkVertex(new Vector3i(1,0,0)+ blockPosition,BlockPalette.Blocks[(int)block].UVYNegative[2], FaceDirection.YNegative),
                        new ChunkVertex(new Vector3i(0,0,0)+ blockPosition,BlockPalette.Blocks[(int)block].UVYNegative[3], FaceDirection.YNegative)
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

                    faceVertexData = new ChunkVertex[]{
                        new ChunkVertex(new Vector3i(1,1,1) + blockPosition,BlockPalette.Blocks[(int)block].UVZPositive[0],FaceDirection.ZPositive),
                        new ChunkVertex(new Vector3i(0,1,1) + blockPosition,BlockPalette.Blocks[(int)block].UVZPositive[1],FaceDirection.ZPositive),
                        new ChunkVertex(new Vector3i(0,0,1) + blockPosition,BlockPalette.Blocks[(int)block].UVZPositive[2],FaceDirection.ZPositive),
                        new ChunkVertex(new Vector3i(1,0,1) + blockPosition,BlockPalette.Blocks[(int)block].UVZPositive[3],FaceDirection.ZPositive)
                    };

                    break;

                case FaceDirection.ZNegative:

                    faceVertexData = new ChunkVertex[]{
                        new ChunkVertex(new Vector3i(1,1,0)+ blockPosition,BlockPalette.Blocks[(int)block].UVZNegative[0],FaceDirection.ZNegative),
                        new ChunkVertex(new Vector3i(0,1,0)+ blockPosition,BlockPalette.Blocks[(int)block].UVZNegative[1],FaceDirection.ZNegative),
                        new ChunkVertex(new Vector3i(0,0,0)+ blockPosition,BlockPalette.Blocks[(int)block].UVZNegative[2],FaceDirection.ZNegative),
                        new ChunkVertex(new Vector3i(1,0,0)+ blockPosition,BlockPalette.Blocks[(int)block].UVZNegative[3],FaceDirection.ZNegative)
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
