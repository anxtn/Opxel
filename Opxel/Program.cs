using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Opxel;
using Opxel.AssetParsing;
using System.Runtime.InteropServices;

//foreach(string name in Enum.GetNames<PixelFormat>())
//{
//    Console.WriteLine($"case: PixelFormat.{name}:\nreturn PixelInternalFormat.{name};");
//}

using (OpxelGame opxelInstance = new OpxelGame())
{
    opxelInstance.Run();
}
