using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Opxel.Graphics
{
    internal class GraphicBuffer : IDisposable
    {
        public readonly int Handle;
        public readonly BufferUsageHint Usage;
        public readonly BufferTarget Target;
        public int Length { get; private set; }
        public int ByteSize { get; private set; }

        private bool disposed;
        public GraphicBuffer(BufferTarget type, BufferUsageHint usage)
        {
            disposed = false;
            Handle = GL.GenBuffer();
            Length = -1;
            ByteSize = -1;
            this.Target = type;
            this.Usage = usage;
        }

        public GraphicBuffer(BufferTarget type)
            : this(type, BufferUsageHint.StaticDraw)
        { }

        public unsafe void SetData<T>(T[] data) where T : unmanaged
        {
            Bind();
            GL.BufferData(Target, sizeof(T) * data.Length, data, Usage);
            Length = data.Length;
            ByteSize = Length * sizeof(T);
        }

        public unsafe T[] ReadData<T>() where T : unmanaged
        {
            T[] data = new T[Length];
            Bind();
            GL.GetBufferSubData(Target, IntPtr.Zero, sizeof(T) * Length, data);
            return data;
        }

        public int GetParameter(BufferParameterName parameterName)
        {
            int value = -1;
            GL.GetBufferParameter(Target,parameterName, out value); 
            return value;
        }

        public void Bind()
        {
            GL.BindBuffer(Target, Handle);
        }

        public void Dispose()
        {
            if(disposed) return;
            disposed = true;
            GL.DeleteBuffer(Handle);
            GC.SuppressFinalize(this);
        }

        ~GraphicBuffer()
        {
            Dispose();
        }
    }
}
