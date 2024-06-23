using OpenTK.Graphics.OpenGL;
using Opxel.Graphics;
using StbImageSharp;

namespace Opxel.Content
{
    internal class ImageLoader
    {
        public static Texture2D LoadFromFile(string path)
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
