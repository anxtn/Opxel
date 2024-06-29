using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using Opxel.Content;

namespace Opxel.Graphics
{
    internal class ChunkMesh
    {
        public readonly Vector3i Position;

        private GraphicBuffer VertexBuffer;
        private GraphicBuffer IndexBuffer;

        [StaticPreLoad("Shaders/ChunkShader.shader.glsl")]
        public static ShaderProgram ChunkShaderProgram = null!;

        public ChunkMesh(Vector3i position)
        {
            this.Position = position;
            this.VertexBuffer = new GraphicBuffer(BufferTarget.ArrayBuffer);
            this.IndexBuffer = new GraphicBuffer(BufferTarget.ElementArrayBuffer);
        }
    }
}
