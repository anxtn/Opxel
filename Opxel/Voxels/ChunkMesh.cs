using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using Opxel.Content;
using Opxel.Graphics;
using Opxel.Debug;
using System.Drawing;

namespace Opxel.Voxels
{
    internal class ChunkMesh : IDisposable
    {
        public readonly Chunk Chunk;

        private readonly ShaderProgram ChunkBlockShaderProgram;
        private readonly PixelTexture BlockTexture;
        private VertexArray VertexArray;
        private GraphicBuffer VertexBuffer;
        private GraphicBuffer IndexBuffer;
        

        private bool disposed;

        public void SetVertices(ChunkVertex[] vertices)
        {
            VertexBuffer.SetData(vertices);
        }

        public void SetIndices(uint[] indices)
        {
            IndexBuffer.SetData(indices);
        }

        private unsafe VertexArray CreateVertexArray()
        {
            VertexAttribute posAttrib = new VertexAttribute(VertexBuffer,0,3,typeof(float),sizeof(float),0,8 * sizeof(float));
            VertexAttribute uvAttrib = new VertexAttribute(VertexBuffer,1,2,typeof(float),sizeof(float),3 * sizeof(float), 8 * sizeof(float));
            VertexAttribute normalAttrib = new VertexAttribute(VertexBuffer,2,3,typeof(float),sizeof(float),5 * sizeof(float), 8 * sizeof(float));
            VertexArray vao = new VertexArray(posAttrib, uvAttrib, normalAttrib);
            return vao;
        }

        public ChunkMesh(Chunk chunk)
        {
            this.Chunk = chunk;
            VertexBuffer = new GraphicBuffer(BufferTarget.ArrayBuffer);
            IndexBuffer = new GraphicBuffer(BufferTarget.ElementArrayBuffer);
            VertexArray = CreateVertexArray();
            this.ChunkBlockShaderProgram = chunk.World.BlockShaderProgram;
            this.BlockTexture = chunk.World.BlockTexture;
            disposed = false;
        }

        public void Render()
        {
            VertexArray.Bind();
            IndexBuffer.Bind();
            BlockTexture.Bind();
            Chunk.World.BlockShaderProgram.Use();
            ChunkBlockShaderProgram.SetUniform("uChunkPosition", Chunk.Position);
            ChunkBlockShaderProgram.SetUniform("uTexture", 0);
            GL.DrawElements(PrimitiveType.Triangles, IndexBuffer.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void GenerateMesh()
        {
            BlockPalette blockPalette = Chunk.World.BlockPalette;
            ChunkMeshBuilder meshBuilder = new ChunkMeshBuilder(blockPalette);
            ChunkLayer[] layers = Chunk.BlockData.Layers;
            

            for(int y = 1;y < Chunk.Size - 1;y++)
            {
                if(layers[y].IsEmpty) continue;

                ChunkLayer layer = layers[y];
                ChunkLayer topLayer = layers[y + 1];
                ChunkLayer buttomLayer = layers[y - 1];

                for(int x = 1;x < Chunk.Size - 1;x++)
                {
                    for(int z = 1;z < Chunk.Size - 1;z++)
                    {
                        int block = layer[x, z];

                        //Wenn unsichbar skippe
                        if(blockPalette.HasBlockTag(block, BlockTags.NoMesh))
                            continue;

                        int neighbourBlock;

                        //XPositive
                        neighbourBlock = layer[x + 1, z];
                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.XPositive, block);
                        }

                        //XNegative
                        neighbourBlock = layer[x - 1, z];
                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.XNegative, block);
                        }


                        //YPositive
                        neighbourBlock = topLayer[x, z];
                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.YPositive, block);
                        }

                        //YNegative
                        neighbourBlock = buttomLayer[x, z];
                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.YNegative, block);
                        }

                        //ZPositive
                        neighbourBlock = layer[x, z + 1];
                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.ZPositive, block);
                        }

                        //ZNegative
                        neighbourBlock = layer[x, z - 1];
                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.ZNegative, block);
                        }
                    }
                }
            }

            this.SetVertices(meshBuilder.VerticesList.ToArray());
            this.SetIndices(meshBuilder.IndicesList.ToArray());
        }

        public void Dispose()
        {
            if(disposed) return;

            disposed = true;
            IndexBuffer.Dispose();
            VertexBuffer.Dispose();
            VertexArray.Dispose();

            GC.SuppressFinalize(this);
        }

        ~ChunkMesh()
        {
            Dispose();
        }
    }
}
