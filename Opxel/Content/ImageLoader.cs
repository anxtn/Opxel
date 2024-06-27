using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Opxel.Graphics;
using StbImageSharp;
using System.Runtime.InteropServices;

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

        public static WindowIcon CreateWindowIcon(string path)
        {
            StbImage.stbi_set_flip_vertically_on_load(0);
            ImageResult imageResult = new ImageResult();
            using(FileStream stream = File.OpenRead(path))
            {
                imageResult = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            }
            OpenTK.Windowing.Common.Input.Image image = new OpenTK.Windowing.Common.Input.Image(imageResult.Width, imageResult.Height, imageResult.Data);
            WindowIcon icon = new WindowIcon(image);
            return icon;
        }
    }
}
