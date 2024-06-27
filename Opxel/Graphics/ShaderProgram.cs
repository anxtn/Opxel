using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System.Text;
using Opxel.Content;

namespace Opxel.Graphics
{
    public class ShaderProgram : IDisposable, IAssetLoadable
    {
        public readonly int Handle;
        public readonly Dictionary<ShaderType, string> ShaderPaths;
        private readonly Dictionary<string, int> uniformLocations;
        private bool disposed;

        public static ShaderProgram activeShaderProgram;

        public unsafe ShaderProgram(params Shader[] shaders)
        {
            this.Handle = GL.CreateProgram();
            disposed = false;
            uniformLocations = new Dictionary<string, int>();
            ShaderPaths = new Dictionary<ShaderType, string>();

            foreach(Shader shader in shaders)
            {
                AttachShader(shader);
            }
            GL.LinkProgram(this.Handle);
            foreach(Shader shader in shaders)
            {
                GL.DetachShader(this.Handle, shader.Handle);
            }
        }

        private void AttachShader(Shader shader)
        {
            if(!shader.Compiled)
            {
                throw new NotCompiledShaderException("Cann not attach a Shader, which is not compiled to an Shaderprogram ");
            }

            if(!this.ShaderPaths.TryAdd(shader.Type, shader.Path))
            {
                throw new ArgumentException($"Cannot Attach a Shader with the same ShaderType ({Enum.GetName(shader.Type)}) twice to the same Shader Program.", nameof(shader));
            }

            GL.AttachShader(this.Handle, shader.Handle);
        }

        public int GetUniformLocation(string name)
        {
            if(uniformLocations.TryGetValue(name, out int location))
            {
                return location;
            }
            else
            {
                int newLocation = GL.GetUniformLocation(this.Handle, name);
                if(newLocation == -1) throw new ArgumentException($"Couldn't find Uniform with name \"{name}\" in the shaderprogram.");
                uniformLocations.Add(name, newLocation);
                return newLocation;
            }
        }

        public void SetUniform(string name, float value)
        {
            GL.Uniform1(GetUniformLocation(name), value);
        }

        public void SetUniform(string name, Vector3 value)
        {
            GL.Uniform3(GetUniformLocation(name), value);
        }



        public void SetUniform(string name, int value)
        {
            GL.Uniform1(GetUniformLocation(name), value);
        }

        public unsafe void SetUniform(string name, ref Matrix3 matrix3, bool transpose = true)
        {
            GL.UniformMatrix3(GetUniformLocation(name), transpose, ref matrix3);
        }

        public unsafe void SetUniform(string name, Matrix3 matrix3, bool transpose = true)
        {
            GL.UniformMatrix3(GetUniformLocation(name), transpose, ref matrix3);
        }

        public unsafe void SetUniform(string name, ref Matrix4 matrix4, bool transpose = true)
        {
            GL.UniformMatrix4(GetUniformLocation(name), transpose, ref matrix4);
        }

        public unsafe void SetUniform(string name, Matrix4 matrix4, bool transpose = true)
        {
            GL.UniformMatrix4(GetUniformLocation(name), transpose, ref matrix4);
        }

        public void Use()
        {
            if(activeShaderProgram != this)
                GL.UseProgram(this.Handle);
        }

        public unsafe T ReadUniform<T>(string name) where T : unmanaged
        {
            T uniformData;
            GL.GetUniform(this.Handle, GetUniformLocation(name), (float*)&uniformData);
            return uniformData;
        }

        public void Dispose()
        {
            if(!disposed)
            {
                disposed = true;
                GL.UseProgram(0);
                GL.DeleteProgram(Handle);
            }
        }

        ~ShaderProgram()
        {
            Dispose();
        }

        //interface

        public static object Load(string path)
        {
            return ShaderProgram.FromShaderFormatFile(path);
        }

        //statics
        public static ShaderProgram FromCodes(params (string code, ShaderType shaderType)[] shaderCodes)
        {
            Shader[] shaders = new Shader[shaderCodes.Length];
            for(int i = 0;i < shaders.Length;i++)
            {
                shaders[i] = Shader.CompileFromSourceCode(shaderCodes[i].code, shaderCodes[i].shaderType);
            }

            ShaderProgram shaderProgram = new ShaderProgram(shaders);
            foreach(Shader shader in shaders)
            {
                shader.Delete();
            }
            return shaderProgram;
        }

        public static ShaderProgram FromFiles(params (string fileName, ShaderType shaderType)[] shaderCodeFiles)
        {
            (string code, ShaderType shaderType)[] codes = new (string, ShaderType)[shaderCodeFiles.Length];

            for(int i = 0;i < codes.Length;i++)
            {

                codes[i].code = File.ReadAllText(shaderCodeFiles[i].fileName);
                codes[i].shaderType = shaderCodeFiles[i].shaderType;
            }
            return FromCodes(codes);
        }

        public static ShaderProgram FromVertexAndFragmentShaderFile(string vertexShaderPath, string fragmentShaderPath)
        {
            return ShaderProgram.FromFiles((vertexShaderPath, ShaderType.VertexShader), (fragmentShaderPath, ShaderType.FragmentShader));
        }

        public static ShaderProgram FromVertexAndFragmentShaderCode(string vertexShaderCode, string fragmentShaderCode)
        {
            return ShaderProgram.FromCodes((vertexShaderCode, ShaderType.VertexShader), (fragmentShaderCode, ShaderType.FragmentShader));
        }

        public static ShaderProgram FromShaderFormatFile(string path)
        {
            return ShaderFileParser.ParseCode(File.ReadAllText(path));
        }
    }


    [Serializable]
    public class NotCompiledShaderException : Exception
    {
        public NotCompiledShaderException() { }
        public NotCompiledShaderException(string message) : base(message) { }
        public NotCompiledShaderException(string message, Exception inner) : base(message, inner) { }
        protected NotCompiledShaderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
