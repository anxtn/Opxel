using OpenTK.Graphics.OpenGL;
using Opxel.Content;
using StbImageSharp;

namespace Opxel.Graphics
{

    internal class Texture2D : IDisposable, IAssetLoadable
    {
        private bool disposed;

        public readonly int Width;
        public readonly int Height;
        public readonly int Handle;

        public TextureMagFilter MagFilter;
        public TextureMinFilter MinFilter;

        public PixelInternalFormat Format;

        public Texture2D(int width, int height, byte[] data)
        {
            this.Width = width;
            this.Height = height;
            this.Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, this.Handle);
            GL.TexImage2D<byte>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            //with NearestMipmapLinear there is no dark arthefakt in far 
           
            GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
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

        public static IAssetLoadable Load(string path)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult imageResult = new ImageResult();
            using(FileStream stream = File.OpenRead(path))
            {
                imageResult = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            }
            Texture2D texture = new Texture2D(imageResult.Width, imageResult.Height, imageResult.Data);
            return texture;
        }
    }
}
