using OpenTK.Graphics.OpenGL;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace Opxel.Graphics
{
    internal struct VertexAttribute
    {
        public readonly GraphicBuffer Buffer;
        public readonly int ComponentCount;
        public readonly int SizeInBytes;
        public readonly int Index;
        public readonly VertexAttribPointerType Type;
        public readonly bool Normalized;
        public readonly VertexAttribPointerMethodType MethodType;
        public readonly VertexAttribIntegerType VertexAttribIntegerType;
        public readonly VertexAttribDoubleType VertexAttribDoubleType;
        public readonly string Name;

        public VertexAttribute(GraphicBuffer buffer, int index, int componentCount, VertexAttribPointerType type, int componentSize, bool normalized = false,
            VertexAttribPointerMethodType methodType = VertexAttribPointerMethodType.Default, VertexAttribIntegerType vertexAttribIntegerType = (VertexAttribIntegerType) (-1),
            VertexAttribDoubleType vertexAttribDoubleType = (VertexAttribDoubleType)(-1), string name = "")
        { 
            if((index < 0) || (index >= OpenGLParameter.MaxVertexAttribs))
                throw new ArgumentOutOfRangeException(nameof(index));

            if(SizeInBytes < 0)
                throw new ArgumentOutOfRangeException(nameof(SizeInBytes));

            if(componentCount < 0)
                throw new ArgumentOutOfRangeException(nameof(componentCount));

            this.SizeInBytes = componentCount * componentSize;
            this.Buffer = buffer;
            this.ComponentCount = componentCount;
            this.Index = index;
            this.Normalized = normalized;
            this.Type = type;
            this.MethodType = methodType;
            this.VertexAttribIntegerType = vertexAttribIntegerType;
            this.VertexAttribDoubleType = vertexAttribDoubleType;
            this.Name = name;
        }
        public VertexAttribute(GraphicBuffer buffer, int index, int componentCount, Type type, int componentSize, bool normalized = false)
            : this(buffer, index, componentCount, TypeToVertexAttribPointerType(type), componentSize, normalized)
        { }


        public static VertexAttribPointerType TypeToVertexAttribPointerType(Type type)
        {
            switch(type.Name)
            {
                case "Single":
                    return VertexAttribPointerType.Float;
                case "Int32":
                    return VertexAttribPointerType.Int;
                case "Double":
                    return VertexAttribPointerType.Double;
                case "Half":
                    return VertexAttribPointerType.HalfFloat;
                case "Byte":
                    return VertexAttribPointerType.UnsignedByte;
                case "SByte":
                    return VertexAttribPointerType.Byte;
                case "Int16":
                    return VertexAttribPointerType.Short;
                case "UInt32":
                    return VertexAttribPointerType.UnsignedInt;
                case "UInt16":
                    return VertexAttribPointerType.UnsignedShort;
                default:
                    return (VertexAttribPointerType)(-1);
            }
        }
    }

    //to distinguish between the diffrent VertexAttribPointer methods
    internal enum VertexAttribPointerMethodType { 
        Default,
        Integer,
        Double
    };
}
