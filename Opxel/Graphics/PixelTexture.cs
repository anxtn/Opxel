using Opxel.Content;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace Opxel.Graphics
{
    internal class PixelTexture : Texture2D, IAssetLoadable
    {
        public PixelTexture(int width, int height, byte[] data) : base(width, height, data)
        {
            this.MagFilter = TextureMagFilter.Nearest;
            this.MinFilter = TextureMinFilter.Nearest;
        }

        public static new IAssetLoadable Load(string path)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult imageResult = new ImageResult();
            using(FileStream stream = File.OpenRead(path))
            {
                imageResult = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            }
            PixelTexture texture = new PixelTexture(imageResult.Width, imageResult.Height, imageResult.Data);
            return texture;
        }
    }
}
