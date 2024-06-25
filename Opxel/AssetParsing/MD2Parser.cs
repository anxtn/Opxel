using OpenTK.Graphics.ES20;
using OpenTK.Mathematics;
using Opxel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//Md2 parser oriented to http://tfc.duke.free.fr/coding/md2-specs-en.html

namespace Opxel.AssetParsing
{
    internal class MD2Parser
    {

        public static readonly Vector3[] NormalCollection = new Vector3[] {
        new Vector3( -0.525731f,  0.000000f,  0.850651f ),
new Vector3( -0.442863f,  0.238856f,  0.864188f ),
new Vector3( -0.295242f,  0.000000f,  0.955423f ),
new Vector3( -0.309017f,  0.500000f,  0.809017f ),
new Vector3( -0.162460f,  0.262866f,  0.951056f ),
new Vector3(  0.000000f,  0.000000f,  1.000000f ),
new Vector3(  0.000000f,  0.850651f,  0.525731f ),
new Vector3( -0.147621f,  0.716567f,  0.681718f ),
new Vector3(  0.147621f,  0.716567f,  0.681718f ),
new Vector3(  0.000000f,  0.525731f,  0.850651f ),
new Vector3(  0.309017f,  0.500000f,  0.809017f ),
new Vector3(  0.525731f,  0.000000f,  0.850651f ),
new Vector3(  0.295242f,  0.000000f,  0.955423f ),
new Vector3(  0.442863f,  0.238856f,  0.864188f ),
new Vector3(  0.162460f,  0.262866f,  0.951056f ),
new Vector3( -0.681718f,  0.147621f,  0.716567f ),
new Vector3( -0.809017f,  0.309017f,  0.500000f ),
new Vector3( -0.587785f,  0.425325f,  0.688191f ),
new Vector3( -0.850651f,  0.525731f,  0.000000f ),
new Vector3( -0.864188f,  0.442863f,  0.238856f ),
new Vector3( -0.716567f,  0.681718f,  0.147621f ),
new Vector3( -0.688191f,  0.587785f,  0.425325f ),
new Vector3( -0.500000f,  0.809017f,  0.309017f ),
new Vector3( -0.238856f,  0.864188f,  0.442863f ),
new Vector3( -0.425325f,  0.688191f,  0.587785f ),
new Vector3( -0.716567f,  0.681718f, -0.147621f ),
new Vector3( -0.500000f,  0.809017f, -0.309017f ),
new Vector3( -0.525731f,  0.850651f,  0.000000f ),
new Vector3(  0.000000f,  0.850651f, -0.525731f ),
new Vector3( -0.238856f,  0.864188f, -0.442863f ),
new Vector3(  0.000000f,  0.955423f, -0.295242f ),
new Vector3( -0.262866f,  0.951056f, -0.162460f ),
new Vector3(  0.000000f,  1.000000f,  0.000000f ),
new Vector3(  0.000000f,  0.955423f,  0.295242f ),
new Vector3( -0.262866f,  0.951056f,  0.162460f ),
new Vector3(  0.238856f,  0.864188f,  0.442863f ),
new Vector3(  0.262866f,  0.951056f,  0.162460f ),
new Vector3(  0.500000f,  0.809017f,  0.309017f ),
new Vector3(  0.238856f,  0.864188f, -0.442863f ),
new Vector3(  0.262866f,  0.951056f, -0.162460f ),
new Vector3(  0.500000f,  0.809017f, -0.309017f ),
new Vector3(  0.850651f,  0.525731f,  0.000000f ),
new Vector3(  0.716567f,  0.681718f,  0.147621f ),
new Vector3(  0.716567f,  0.681718f, -0.147621f ),
new Vector3(  0.525731f,  0.850651f,  0.000000f ),
new Vector3(  0.425325f,  0.688191f,  0.587785f ),
new Vector3(  0.864188f,  0.442863f,  0.238856f ),
new Vector3(  0.688191f,  0.587785f,  0.425325f ),
new Vector3(  0.809017f,  0.309017f,  0.500000f ),
new Vector3(  0.681718f,  0.147621f,  0.716567f ),
new Vector3(  0.587785f,  0.425325f,  0.688191f ),
new Vector3(  0.955423f,  0.295242f,  0.000000f ),
new Vector3(  1.000000f,  0.000000f,  0.000000f ),
new Vector3(  0.951056f,  0.162460f,  0.262866f ),
new Vector3(  0.850651f, -0.525731f,  0.000000f ),
new Vector3(  0.955423f, -0.295242f,  0.000000f ),
new Vector3(  0.864188f, -0.442863f,  0.238856f ),
new Vector3(  0.951056f, -0.162460f,  0.262866f ),
new Vector3(  0.809017f, -0.309017f,  0.500000f ),
new Vector3(  0.681718f, -0.147621f,  0.716567f ),
new Vector3(  0.850651f,  0.000000f,  0.525731f ),
new Vector3(  0.864188f,  0.442863f, -0.238856f ),
new Vector3(  0.809017f,  0.309017f, -0.500000f ),
new Vector3(  0.951056f,  0.162460f, -0.262866f ),
new Vector3(  0.525731f,  0.000000f, -0.850651f ),
new Vector3(  0.681718f,  0.147621f, -0.716567f ),
new Vector3(  0.681718f, -0.147621f, -0.716567f ),
new Vector3(  0.850651f,  0.000000f, -0.525731f ),
new Vector3(  0.809017f, -0.309017f, -0.500000f ),
new Vector3(  0.864188f, -0.442863f, -0.238856f ),
new Vector3(  0.951056f, -0.162460f, -0.262866f ),
new Vector3(  0.147621f,  0.716567f, -0.681718f ),
new Vector3(  0.309017f,  0.500000f, -0.809017f ),
new Vector3(  0.425325f,  0.688191f, -0.587785f ),
new Vector3(  0.442863f,  0.238856f, -0.864188f ),
new Vector3(  0.587785f,  0.425325f, -0.688191f ),
new Vector3(  0.688191f,  0.587785f, -0.425325f ),
new Vector3( -0.147621f,  0.716567f, -0.681718f ),
new Vector3( -0.309017f,  0.500000f, -0.809017f ),
new Vector3(  0.000000f,  0.525731f, -0.850651f ),
new Vector3( -0.525731f,  0.000000f, -0.850651f ),
new Vector3( -0.442863f,  0.238856f, -0.864188f ),
new Vector3( -0.295242f,  0.000000f, -0.955423f ),
new Vector3( -0.162460f,  0.262866f, -0.951056f ),
new Vector3(  0.000000f,  0.000000f, -1.000000f ),
new Vector3(  0.295242f,  0.000000f, -0.955423f ),
new Vector3(  0.162460f,  0.262866f, -0.951056f ),
new Vector3( -0.442863f, -0.238856f, -0.864188f ),
new Vector3( -0.309017f, -0.500000f, -0.809017f ),
new Vector3( -0.162460f, -0.262866f, -0.951056f ),
new Vector3(  0.000000f, -0.850651f, -0.525731f ),
new Vector3( -0.147621f, -0.716567f, -0.681718f ),
new Vector3(  0.147621f, -0.716567f, -0.681718f ),
new Vector3(  0.000000f, -0.525731f, -0.850651f ),
new Vector3(  0.309017f, -0.500000f, -0.809017f ),
new Vector3(  0.442863f, -0.238856f, -0.864188f ),
new Vector3(  0.162460f, -0.262866f, -0.951056f ),
new Vector3(  0.238856f, -0.864188f, -0.442863f ),
new Vector3(  0.500000f, -0.809017f, -0.309017f ),
new Vector3(  0.425325f, -0.688191f, -0.587785f ),
new Vector3(  0.716567f, -0.681718f, -0.147621f ),
new Vector3(  0.688191f, -0.587785f, -0.425325f ),
new Vector3(  0.587785f, -0.425325f, -0.688191f ),
new Vector3(  0.000000f, -0.955423f, -0.295242f ),
new Vector3(  0.000000f, -1.000000f,  0.000000f ),
new Vector3(  0.262866f, -0.951056f, -0.162460f ),
new Vector3(  0.000000f, -0.850651f,  0.525731f ),
new Vector3(  0.000000f, -0.955423f,  0.295242f ),
new Vector3(  0.238856f, -0.864188f,  0.442863f ),
new Vector3(  0.262866f, -0.951056f,  0.162460f ),
new Vector3(  0.500000f, -0.809017f,  0.309017f ),
new Vector3(  0.716567f, -0.681718f,  0.147621f ),
new Vector3(  0.525731f, -0.850651f,  0.000000f ),
new Vector3( -0.238856f, -0.864188f, -0.442863f ),
new Vector3( -0.500000f, -0.809017f, -0.309017f ),
new Vector3( -0.262866f, -0.951056f, -0.162460f ),
new Vector3( -0.850651f, -0.525731f,  0.000000f ),
new Vector3( -0.716567f, -0.681718f, -0.147621f ),
new Vector3( -0.716567f, -0.681718f,  0.147621f ),
new Vector3( -0.525731f, -0.850651f,  0.000000f ),
new Vector3( -0.500000f, -0.809017f,  0.309017f ),
new Vector3( -0.238856f, -0.864188f,  0.442863f ),
new Vector3( -0.262866f, -0.951056f,  0.162460f ),
new Vector3( -0.864188f, -0.442863f,  0.238856f ),
new Vector3( -0.809017f, -0.309017f,  0.500000f ),
new Vector3( -0.688191f, -0.587785f,  0.425325f ),
new Vector3( -0.681718f, -0.147621f,  0.716567f ),
new Vector3( -0.442863f, -0.238856f,  0.864188f ),
new Vector3( -0.587785f, -0.425325f,  0.688191f ),
new Vector3( -0.309017f, -0.500000f,  0.809017f ),
new Vector3( -0.147621f, -0.716567f,  0.681718f ),
new Vector3( -0.425325f, -0.688191f,  0.587785f ),
new Vector3( -0.162460f, -0.262866f,  0.951056f ),
new Vector3(  0.442863f, -0.238856f,  0.864188f ),
new Vector3(  0.162460f, -0.262866f,  0.951056f ),
new Vector3(  0.309017f, -0.500000f,  0.809017f ),
new Vector3(  0.147621f, -0.716567f,  0.681718f ),
new Vector3(  0.000000f, -0.525731f,  0.850651f ),
new Vector3(  0.425325f, -0.688191f,  0.587785f ),
new Vector3(  0.587785f, -0.425325f,  0.688191f ),
new Vector3(  0.688191f, -0.587785f,  0.425325f ),
new Vector3( -0.955423f,  0.295242f,  0.000000f ),
new Vector3( -0.951056f,  0.162460f,  0.262866f ),
new Vector3( -1.000000f,  0.000000f,  0.000000f ),
new Vector3( -0.850651f,  0.000000f,  0.525731f ),
new Vector3( -0.955423f, -0.295242f,  0.000000f ),
new Vector3( -0.951056f, -0.162460f,  0.262866f ),
new Vector3( -0.864188f,  0.442863f, -0.238856f ),
new Vector3( -0.951056f,  0.162460f, -0.262866f ),
new Vector3( -0.809017f,  0.309017f, -0.500000f ),
new Vector3( -0.864188f, -0.442863f, -0.238856f ),
new Vector3( -0.951056f, -0.162460f, -0.262866f ),
new Vector3( -0.809017f, -0.309017f, -0.500000f ),
new Vector3( -0.681718f,  0.147621f, -0.716567f ),
new Vector3( -0.681718f, -0.147621f, -0.716567f ),
new Vector3( -0.850651f,  0.000000f, -0.525731f ),
new Vector3( -0.688191f,  0.587785f, -0.425325f ),
new Vector3( -0.587785f,  0.425325f, -0.688191f ),
new Vector3( -0.425325f,  0.688191f, -0.587785f ),
new Vector3( -0.425325f, -0.688191f, -0.587785f ),
new Vector3( -0.587785f, -0.425325f, -0.688191f ),
new Vector3( -0.688191f, -0.587785f, -0.425325f )
        };

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

                if(result.Header.Version != 8)
                {
                    throw new Md2ParsingException($"Version of File muste be 8, but is {result.Header.Version}");
                }

                //Skins
                byte[] skinData = br.ReadBytes(Marshal.SizeOf<Md2Skin>() * result.Header.SkinCount);
                result.Skins = ParsingHelper.GetObjectsFromBytes<Md2Skin>(skinData, result.Header.SkinCount);

                //Texture Coords
                byte[] texCoordsData = br.ReadBytes(Marshal.SizeOf<Md2TexCoord>() * result.Header.TexCoordCount);
                result.TexCoords = ParsingHelper.GetObjectsFromBytes<Md2TexCoord>(texCoordsData, result.Header.TexCoordCount);

                //Triangles
                byte[] trianglesData = br.ReadBytes(Marshal.SizeOf<Md2Triangle>() * result.Header.TriangleCount);
                result.Triangles = ParsingHelper.GetObjectsFromBytes<Md2Triangle>(trianglesData, result.Header.TriangleCount);

                //Frames
                result.Frames = new Md2Frame[result.Header.FrameCount];

                for(int i = 0;i < result.Header.FrameCount;i++)
                {
                    //Scale
                    byte[] scaleData = br.ReadBytes(Marshal.SizeOf<Md2Vector3>());
                    result.Frames[i].Scale = ParsingHelper.GetObjectFromBytes<Md2Vector3>(scaleData);

                    //Translate
                    byte[] translateData = br.ReadBytes(Marshal.SizeOf<Md2Vector3>());
                    result.Frames[i].Translate = ParsingHelper.GetObjectFromBytes<Md2Vector3>(translateData);

                    //Name 
                    byte[] nameData = br.ReadBytes(16);
                    result.Frames[i].Name = Encoding.ASCII.GetString(nameData).TrimEnd('\0');

                    //Vertices
                    byte[] vertexData = br.ReadBytes(Marshal.SizeOf<Md2Vertex>() * result.Header.VertexCount);
                    result.Frames[i].Vertices = ParsingHelper.GetObjectsFromBytes<Md2Vertex>(vertexData, result.Header.VertexCount);
                }

                //Gl Commands
                byte[] glCommandsData = br.ReadBytes(Marshal.SizeOf<int>() * result.Header.CommandsCount);
                result.GlCommands = ParsingHelper.GetObjectsFromBytes<int>(glCommandsData, result.Header.CommandsCount);
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
            mesh.Normals = GetNormals(0);
            mesh.SetAllVertexColors(Color4.Gray);

            return mesh;
        }

        public Vector3[] CalculateVertices(int frameIndex)
        {
            Md2Frame frame = Frames[frameIndex];
            Vector3[] vertices = new Vector3[frame.Vertices.Length];

            for (int i = 0;i < frame.Vertices.Length;i++)
            {
                Md2Vertex vertex = frame.Vertices[i];
                vertices[i] = new Vector3(
                    vertex.X * frame.Scale.X + frame.Translate.X,
                    vertex.Y * frame.Scale.Y + frame.Translate.Y,
                    vertex.Z * frame.Scale.Z + frame.Translate.Z);

                (vertices[i].Y, vertices[i].Z) = (vertices[i].Z, vertices[i].Y);

                Console.WriteLine(vertices[i]);
            }

            return vertices;
        }

        public uint[] GetIndices()
        {
            uint[] indices = new uint[Header.TriangleCount * 3];
            for(int i = 0;i < Header.TriangleCount;i++)
            {
                indices[i*3] = Triangles[i].VertexIndex1;
                indices[i*3 + 1] = Triangles[i].VertexIndex2;
                indices[i*3 + 2] = Triangles[i].VertexIndex3;
            }

            return indices;
        }

        public Vector3[] GetNormals(int frameIndex)
        {
            Md2Frame frame = Frames[frameIndex];
            Vector3[] normals = new Vector3[Header.VertexCount];

            for(int i = 0;i < Header.VertexCount;i++)
            {
                normals[i] = MD2Parser.NormalCollection[frame.Vertices[i].NormalIndex];
                (normals[i].Y, normals[i].Z) = (normals[i].Z, normals[i].Y);
            }

            return normals;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2Header
    {
        public int ident;                  /* magic number: "IDP2" */
        public int Version;                /* version: must be 8 */

        public int SkinWidth;              /* texture width */
        public int SkinHeight;             /* texture height */

        public int FrameSize;              /* size in bytes of a frame */

        public int SkinCount;              /* number of skins */
        public int VertexCount;           /* number of vertices per frame */
        public int TexCoordCount;                 /* number of texture coordinates */
        public int TriangleCount;               /* number of triangles */
        public int CommandsCount;             /* number of opengl commands */
        public int FrameCount;             /* number of frames */

        public int OffsetSkins;           /* offset skin data */
        public int OffsetTexCoords;              /* offset texture coordinate data */
        public int OffsetTriangles;            /* offset triangle data */
        public int OffsetFrames;          /* offset frame data */
        public int OffsetCommands;          /* offset OpenGL command data */
        public int OffsetEnd;             /* offset end of file */
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3 ToOpenTKVector3()
        {
            return new Vector3(X, Z,Y);
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
        public ushort VertexIndex1;
        public ushort VertexIndex2;
        public ushort VertexIndex3;

        public ushort TexCoordIndex1;
        public ushort TexCoordIndex2;
        public ushort TexCoordIndex3;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2TexCoord
    {
        public short S;
        public short T;
    };

    //Compressed Vertex!
    [StructLayout(LayoutKind.Sequential)]
    internal struct Md2Vertex
    {
        public byte X;
        public byte Y;
        public byte Z;
        public byte NormalIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Md2Frame
    {
        public Md2Vector3 Scale;               /* scale factor */
        public Md2Vector3 Translate;           /* translation vector */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;             /* frame name */
        public Md2Vertex[] Vertices; /* list of frame's vertices */
    };

    [StructLayout(LayoutKind.Sequential)]
    struct Md2GlCommand
    {
        public float S;                    /* s texture coord. */
        public float T;                    /* t texture coord. */
        public int Index;                  /* vertex index */
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
