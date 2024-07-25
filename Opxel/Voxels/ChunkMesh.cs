using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using Opxel.Content;
using Opxel.Graphics;
using Opxel.Debug;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Opxel.Voxels
{
    internal class ChunkMesh : IDisposable
    {
        public readonly Chunk Chunk;

        private readonly ShaderProgram ChunkBlockShaderProgram;
        private readonly Texture2D BlockTexture;
        private VertexArray VertexArray;
        private GraphicBuffer VertexBuffer;
        private GraphicBuffer IndexBuffer;


        private bool disposed;

        private unsafe VertexArray CreateVertexArray()
        {
            VertexAttribute posAttrib = new VertexAttribute(VertexBuffer, 0, 3, typeof(float), sizeof(float), 0, 8 * sizeof(float));
            VertexAttribute uvAttrib = new VertexAttribute(VertexBuffer, 1, 2, typeof(float), sizeof(float), 3 * sizeof(float), 8 * sizeof(float));
            VertexAttribute normalAttrib = new VertexAttribute(VertexBuffer, 2, 3, typeof(float), sizeof(float), 5 * sizeof(float), 8 * sizeof(float));
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
            ChunkBlockShaderProgram.SetUniform("uCameraPosition", Chunk.World.Player.Camera.Transform.Position);
            GL.DrawElements(PrimitiveType.Triangles, IndexBuffer.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void GenerateMesh()
        {
            BlockPalette blockPalette = Chunk.World.BlockPalette;
            ChunkManager chunkManager = Chunk.World.ChunkManager;
            ChunkMeshBuilder meshBuilder = new ChunkMeshBuilder(blockPalette);
            ChunkLayer[] layers = Chunk.BlockData.Layers;

            Chunk.World.WorldDataLoader.LoadNeighbourData(Chunk.Position,
                out ChunkBlockData xPosNeighbourData,
                out ChunkBlockData xNegNeighbourData,
                out ChunkBlockData zPosNeighbourData,
                out ChunkBlockData zNegNeighbourData);



            for(int y = 0;y < Chunk.SizeY;y++)
            {
                if(layers[y].IsEmpty) continue;

                ChunkLayer layer = layers[y];
                ChunkLayer buttomLayer = y > 0 ? layers[y - 1] : ChunkLayer.Empty;
                ChunkLayer topLayer = y < Chunk.SizeY - 1 ? layers[y + 1] : ChunkLayer.Empty;

                for(int x = 0;x < Chunk.SizeX;x++)
                {
                    for(int z = 0;z < Chunk.SizeZ;z++)
                    {
                        int block = layer[x, z];

                        //Wenn unsichtbar skippe
                        if(blockPalette.HasBlockTag(block, BlockTags.NoMesh))
                            continue;

                        int neighbourBlock;

                        //XPositive
                        if(x < Chunk.SizeX - 1)
                        {
                            neighbourBlock = layer[x + 1, z];
                        }
                        else
                        {
                            neighbourBlock = xPosNeighbourData.GetBlock(0, y, z);
                        }

                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.XPositive, block);
                        }

                        //XNegative
                        if(x > 0)
                        {
                            neighbourBlock = layer[x - 1, z];
                        }
                        else
                        {
                            neighbourBlock = xNegNeighbourData.GetBlock(Chunk.SizeX - 1, y, z);
                        }

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
                        if(z != Chunk.SizeZ - 1)
                        {
                            neighbourBlock = layer[x, z + 1];
                        }
                        else
                        {
                            neighbourBlock = zPosNeighbourData.GetBlock(x, y, 0);
                        }

                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.ZPositive, block);
                        }


                        //ZNegative
                        if(z > 0)
                        {
                            neighbourBlock = layer[x, z - 1];
                        }
                        else
                        {
                            neighbourBlock = zNegNeighbourData.GetBlock(x, y, Chunk.SizeZ - 1);
                        }

                        if(blockPalette.HasBlockTag(neighbourBlock, BlockTags.Transparent))
                        {
                            meshBuilder.AddBlockFace(new Vector3i(x, y, z), FaceDirection.ZNegative, block);
                        }

                    }
                }
            }

            // 122 us
            VertexBuffer.SetData(meshBuilder.VerticesList);
            IndexBuffer.SetData(meshBuilder.IndicesList);
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
