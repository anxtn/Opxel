using Opxel.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Reflection.PortableExecutable;
using Opxel.Content;
using Opxel.AssetParsing;

namespace Opxel.Graphics
{
    internal class Mesh : IAssetLoadable, IDisposable
    {
        public Transform Transform;

        public GraphicBuffer PositionBuffer;
        public GraphicBuffer ColorBuffer;
        public GraphicBuffer IndexBuffer;
        public GraphicBuffer NormalBuffer;

        

        [StaticPreLoad("Shaders/SimpleShader.shader.glsl")]
        public static ShaderProgram DefaultShaderProgram = null!; //Nullable c# Gedöhns ╰( ° ʖ ° )つ──☆*

        public VertexArray VertexArray;

        public ShaderProgram ShaderProgram;

        private bool disposed; 

        public Vector3[] Positions {
            get
            {
                return PositionBuffer.ReadData<Vector3>();
            }
            set
            {
                 PositionBuffer.SetData<Vector3>(value);
            }
        }

        public Vector3[] Normals
        {
            get
            {
                return NormalBuffer.ReadData<Vector3>();
            }
            set
            {
                NormalBuffer.SetData<Vector3>(value);
            }
        }

        public Color4[] Colors
        {
            get
            {
                return ColorBuffer.ReadData<Color4>();
            }
            set
            {
                ColorBuffer.SetData<Color4>(value);
            }
        }

        public uint[] Indices
        {
            get
            {
                return IndexBuffer.ReadData<uint>();
            }
            set
            {
                IndexBuffer.SetData<uint>(value);
            }
        }

        public Mesh()
        {
            disposed = false;
            Transform = new Transform();
            PositionBuffer = new GraphicBuffer(BufferTarget.ArrayBuffer);
            NormalBuffer = new GraphicBuffer(BufferTarget.ArrayBuffer);
            ColorBuffer = new GraphicBuffer(BufferTarget.ArrayBuffer);
            IndexBuffer = new GraphicBuffer(BufferTarget.ElementArrayBuffer);
            ShaderProgram = DefaultShaderProgram;

            VertexArray = new VertexArray(
                new VertexAttribute(PositionBuffer, 0, 3, typeof(float), sizeof(float)),
                new VertexAttribute(ColorBuffer, 1, 4, typeof(float), sizeof(float)),
                new VertexAttribute(NormalBuffer, 2, 3, typeof(float), sizeof(float)));

            Positions = new Vector3[0];
            Normals = new Vector3[0];
            Colors = new Color4[0]; 
            Indices = new uint[0];
        }

        public void Render()
        {
            ShaderProgram.Use();
            ShaderProgram.SetUniform("uModel", Transform.ModelMatrix, true);
            VertexArray.Bind();
            IndexBuffer.Bind();
            GL.DrawElements(PrimitiveType.Triangles, IndexBuffer.Length, DrawElementsType.UnsignedInt, 0);
        }

        //Code From Sebastian Legue
        public void CalculateNormals()
        {
            Vector3[] normals = new Vector3[PositionBuffer.Length];
            int triangleCount = IndexBuffer.Length / 3;

            uint[] indices = Indices;
            Vector3[] positions = Positions;  

            for(int i = 0;i < triangleCount;i++)
            {
                int normalTriangleIndex = i * 3;
                uint vertexIndex1 = indices[normalTriangleIndex];
                uint vertexIndex2 = indices[normalTriangleIndex + 1];
                uint vertexIndex3 = indices[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndex1, vertexIndex2, vertexIndex3);
                normals[vertexIndex1] += triangleNormal;
                normals[vertexIndex2] += triangleNormal;
                normals[vertexIndex3] += triangleNormal;
            }

            for(int i = 0;i < normals.Length;i++)
            {
                normals[i].Normalize();
            }

            this.Normals = normals;

            Vector3 SurfaceNormalFromIndices(uint index1, uint index2, uint index3)
            {
                Vector3 pointA = positions[index1];
                Vector3 pointB = positions[index2];
                Vector3 pointC = positions[index3];

                Vector3 sideAB = pointB - pointA;
                Vector3 sideAC = pointC - pointA;

                return Vector3.Cross(sideAB, sideAC).Normalized();
            }
        }

        public void SetAllVertexColors(Color4 color)
        {
            Color4[] colors = new Color4[PositionBuffer.Length];
            for(int i = 0;i < colors.Length;i++)
            {
                colors[i] = color;
            }
            this.Colors = colors;
        }

        public static IAssetLoadable Load(string path)
        {
            return MD2Parser.ParseFile(path).ToMesh();
        }

        public void Dispose()
        {
            if(disposed) 
                return;

            PositionBuffer.Dispose();   
            IndexBuffer.Dispose();
            ColorBuffer.Dispose();
            NormalBuffer.Dispose();
            VertexArray.Dispose();

            disposed = true;
        }

        ~Mesh()
        {
            Dispose();
        }

    }
}
