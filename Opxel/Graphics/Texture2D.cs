using OpenTK.Graphics.OpenGL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Opxel.Graphics
{

    internal class Texture2D : IDisposable
    {
        private bool disposed;

        public readonly int Width;
        public readonly int Height;
        public readonly int Handle;

        public TextureMagFilter MagFilter;
        public TextureMagFilter MinFilter;

        public PixelInternalFormat Format;

        public Texture2D(int width, int height, byte[] data)
        {
            this.Width = width;
            this.Height = height;
            this.Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, this.Handle);
            GL.TexImage2D<byte>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            GL.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            disposed = false;
        }

        public void Bind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, this.Handle);
        }

        public void Dispose()
        {
            if(disposed) return;
            disposed = true;
            GC.SuppressFinalize(this);
        }

        ~Texture2D()
        {
            Dispose();
        }
    }
}
