using OpenTK.Graphics.OpenGL;
using Opxel.Graphics;

namespace Opxel.Graphics
{
    internal class UniformBuffer : IDisposable
    {
        public readonly int BlockIndex;
        public readonly ShaderProgram ShaderProgram;
        public readonly GraphicBuffer Buffer;
        public readonly string BlockName;
        public readonly int BindingPoint;

        private bool disposed;

        public UniformBuffer(ShaderProgram shaderProgram, string blockName, int bindingPoint)
        {
            this.ShaderProgram = shaderProgram;
            this.BlockName = blockName;
            this.BindingPoint = bindingPoint;
            this.Buffer = new GraphicBuffer(BufferTarget.UniformBuffer, BufferUsageHint.StreamDraw);
            BlockIndex = GL.GetUniformBlockIndex(ShaderProgram.Handle, BlockName);
            GL.UniformBlockBinding(ShaderProgram.Handle, BlockIndex, bindingPoint);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, BindingPoint, Buffer.Handle);
            disposed = false;
        }


        public void BindBufferBase()
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, BindingPoint, Buffer.Handle);
        }

        public unsafe void SetData<T>(T[] blockData) where T : unmanaged
        {
            Buffer.SetData<T>(blockData);
        }

        public void SetData<T>(T blockData) where T : unmanaged
        {
            Buffer.SetData<T>(new T[] { blockData });
        }

        public void Dispose()
        {
            if(disposed) return;
            Buffer.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }

        ~UniformBuffer()
        {
            Dispose();
        }
    }
}
