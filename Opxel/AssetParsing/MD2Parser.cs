using OpenTK.Graphics.ES20;
using OpenTK.Mathematics;
using Opxel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//TODO
//-naming
namespace Opxel.AssetParsing
{
    internal class MD2Parser
    {

        public static MD2Object ParseFile(string path)
        {
            MD2Object result = new MD2Object();

            using(BinaryReader br = new BinaryReader(File.OpenRead(path)))
            {
                //Header
                byte[] headerData = br.ReadBytes(Marshal.SizeOf<Md2Header>());
                result.Header = ParsingHelper.GetObjectFromBytes<Md2Header>(headerData);

                if(result.Header.ident != 844121161)
                {
                    throw new Md2ParsingException($"Ident in header is wrong. Ident must be 844121161, but the value is {result.Header.ident}.");
                }

                if(result.Header.version != 8)
                {
                    throw new Md2ParsingException($"Version of File muste be 8, but is {result.Header.version}");
                }

                //Skins
                byte[] skinData = br.ReadBytes(Marshal.SizeOf<Md2Skin>() * result.Header.num_skins);
                result.Skins = ParsingHelper.GetObjectsFromBytes<Md2Skin>(skinData, result.Header.num_skins);

                //Texture Coords
                byte[] texCoordsData = br.ReadBytes(Marshal.SizeOf<Md2TexCoord>() * result.Header.num_st);
                result.TexCoords = ParsingHelper.GetObjectsFromBytes<Md2TexCoord>(texCoordsData, result.Header.num_st);

                //Triangles
                byte[] trianglesData = br.ReadBytes(Marshal.SizeOf<Md2Triangle>() * result.Header.num_tris);
                result.Triangles = ParsingHelper.GetObjectsFromBytes<Md2Triangle>(trianglesData, result.Header.num_tris);

                //Frames
                result.Frames = new Md2Frame[result.Header.num_frames];

                for(int i = 0;i < result.Header.num_frames;i++)
                {
                    //Scale
                    byte[] scaleData = br.ReadBytes(Marshal.SizeOf<Md2Vector3>());
                    result.Frames[i].scale = ParsingHelper.GetObjectFromBytes<Md2Vector3>(scaleData);

                    //Translate
                    byte[] translateData = br.ReadBytes(Marshal.SizeOf<Md2Vector3>());
                    result.Frames[i].translate = ParsingHelper.GetObjectFromBytes<Md2Vector3>(translateData);

                    //Name 
                    byte[] nameData = br.ReadBytes(16);
                    result.Frames[i].Name = Encoding.ASCII.GetString(nameData).TrimEnd('\0');

                    //Vertices
                    byte[] vertexData = br.ReadBytes(Marshal.SizeOf<Md2Vertex>() * result.Header.num_vertices);
                    result.Frames[i].vertices = ParsingHelper.GetObjectsFromBytes<Md2Vertex>(vertexData, result.Header.num_vertices);
                }

                //Gl Commands
                byte[] glCommandsData = br.ReadBytes(Marshal.SizeOf<int>() * result.Header.num_glcmds);
                result.GlCommands = ParsingHelper.GetObjectsFromBytes<int>(glCommandsData, result.Header.num_glcmds);
            }

            return result;
        }
    }

    internal class MD2Object
    {
        public Md2Header Header;
        public Md2Skin[] Skins;
        public Md2TexCoord[] TexCoords;
        public Md2Triangle[] Triangles;
        public Md2Frame[] Frames;
        public int[] GlCommands;
        public int TexId;

        public Mesh ToMesh()
        {
            Mesh mesh = new Mesh();
            mesh.Positions = CalculateVertices(0);
            mesh.Indices = GetIndices();
            mesh.SetAllVertexColors(Color4.Red);
            mesh.CalculateNormals();

            return mesh;
        }

        public Vector3[] CalculateVertices(int frameIndex)
        {
            Md2Frame frame = Frames[frameIndex];
            Vector3[] vertices = new Vector3[frame.vertices.Length];

            Console.WriteLine(frame.Name);

            for (int i = 0;i < frame.vertices.Length;i++)
            {
                Md2Vertex vertex = frame.vertices[i];
                vertices[i] = new Vector3(
                    vertex.x * frame.scale.x + frame.translate.x,
                    vertex.y * frame.scale.z + frame.translate.y,
                    vertex.z * frame.scale.z + frame.translate.z);

                (vertices[i].Y, vertices[i].Z) = (vertices[i].Z, vertices[i].Y);

                Console.WriteLine(vertices[i]);
            }

            return vertices;
        }

        public uint[] GetIndices()
        {
            uint[] indices = new uint[Header.num_tris * 3];
            for(int i = 0;i < Header.num_tris;i++)
            {
                indices[i*3] = Triangles[i].vertexIndex1;
                indices[i*3 + 1] = Triangles[i].vertexIndex2;
                indices[i*3 + 2] = Triangles[i].vertexIndex3;
            }

            return indices;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2Header
    {
        public int ident;                  /* magic number: "IDP2" */
        public int version;                /* version: must be 8 */

        public int skinwidth;              /* texture width */
        public int skinheight;             /* texture height */

        public int framesize;              /* size in bytes of a frame */

        public int num_skins;              /* number of skins */
        public int num_vertices;           /* number of vertices per frame */
        public int num_st;                 /* number of texture coordinates */
        public int num_tris;               /* number of triangles */
        public int num_glcmds;             /* number of opengl commands */
        public int num_frames;             /* number of frames */

        public int offset_skins;           /* offset skin data */
        public int offset_st;              /* offset texture coordinate data */
        public int offset_tris;            /* offset triangle data */
        public int offset_frames;          /* offset frame data */
        public int offset_glcmds;          /* offset OpenGL command data */
        public int offset_end;             /* offset end of file */
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3 ToOpenTKVector3()
        {
            return new Vector3(x, z,y);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2Skin
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Name;    //64 bytes (texture file name)
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2Triangle
    {
        public ushort vertexIndex1;
        public ushort vertexIndex2;
        public ushort vertexIndex3;

        public ushort textureCoordIndex1;
        public ushort textureCoordIndex2;
        public ushort textureCoordIndex3;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2TexCoord
    {
        public short s;
        public short t;
    };

    //Compressed Vertex!
    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2Vertex
    {
        public byte x;
        public byte y;
        public byte z;
        public byte normalIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Md2Frame
    {
        public Md2Vector3 scale;               /* scale factor */
        public Md2Vector3 translate;           /* translation vector */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;             /* frame name */
        public Md2Vertex[] vertices; /* list of frame's vertices */
    };

    [StructLayout(LayoutKind.Sequential)]
    struct Md2GlCommand
    {
        public float s;                    /* s texture coord. */
        public float t;                    /* t texture coord. */
        public int index;                  /* vertex index */
    };


    [Serializable]
    public class Md2ParsingException : Exception
    {
        public Md2ParsingException() { }
        public Md2ParsingException(string message) : base(message) { }
        public Md2ParsingException(string message, Exception inner) : base(message, inner) { }
        protected Md2ParsingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
