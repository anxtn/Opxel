using OpenTK.Graphics.OpenGL;
using Opxel.Graphics;

namespace Opxel.Graphics
{
    internal class VertexArray : IDisposable
    {
        public readonly int Handle;
        public readonly VertexAttribute[] Attributes;
        private bool disposed;

        public VertexArray(params VertexAttribute[] attributes)
        {
            this.Handle = GL.GenVertexArray();
            disposed = false;
            this.Attributes = attributes;
            GL.BindVertexArray(this.Handle);
            foreach(VertexAttribute attrib in attributes)
            {
                attrib.Buffer.Bind();

                switch(attrib.MethodType)
                {
                    case VertexAttribPointerMethodType.Default:
                        GL.VertexAttribPointer(attrib.Index, attrib.ComponentCount, attrib.Type, attrib.Normalized, attrib.SizeInBytes, 0);
                        break;
                    case VertexAttribPointerMethodType.Integer:
                        GL.VertexAttribIPointer(attrib.Index, attrib.ComponentCount, attrib.VertexAttribIntegerType, attrib.SizeInBytes, IntPtr.Zero);
                        break;
                    case VertexAttribPointerMethodType.Double:
                        GL.VertexAttribLPointer(attrib.Index, attrib.ComponentCount, attrib.VertexAttribDoubleType , attrib.SizeInBytes, IntPtr.Zero);
                        break;
                }

                GL.EnableVertexAttribArray(attrib.Index);
            }
            GL.BindVertexArray(0);
        }

        public void Bind()
        {
            GL.BindVertexArray(this.Handle);
        }

        public void Dispose()
        {
            if(!disposed)
            {
                disposed = true;
                GL.BindVertexArray(0);
                GL.DeleteVertexArray(this.Handle);
            }
        }

        ~VertexArray()
        {
            Dispose();
        }
    }
}
