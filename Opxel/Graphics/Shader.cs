using OpenTK.Graphics.OpenGL;
using Opxel.Debug;

namespace Opxel.Graphics
{
    public class Shader
    {
        public readonly int Handle;
        public readonly ShaderType Type;
        public string? Path { get; private set; }
        public bool Compiled => GetParameter(ShaderParameter.CompileStatus) == 1;

        public Shader(ShaderType shaderType)
        {
            this.Type = shaderType;
            this.Handle = GL.CreateShader(shaderType);
        }

        public void Delete()
        {
            GL.DeleteShader(Handle);
        }

        public void Attach(int program)
        {
            GL.AttachShader(program, Handle);
        }

        public void CompileFromSourceCode(string sourceCode)
        {
            if(Path is null)
                Path = "string sourcecode";
            GL.ShaderSource(Handle, sourceCode);
            GL.CompileShader(this.Handle);

            string err = GetShaderInfoLog();
            if(err != string.Empty)
            {
                Debugger.LogError(err);
            }
        }

        public void CompileFromFile(string path)
        {
            Path = path;
            CompileFromSourceCode(File.ReadAllText(path));
        }

        public int GetParameter(ShaderParameter parameter)
        {
            int value = -1;
            GL.GetShader(Handle, parameter, out value);
            return value;
        }

        public string GetShaderSourceCode()
        {
            int sourceLength = GetParameter(ShaderParameter.ShaderSourceLength);
            string sourceCode = new string(' ', sourceLength);
            GL.GetShaderSource(Handle, sourceLength, out sourceLength, out sourceCode);
            return sourceCode;
        }

        public string GetShaderInfoLog()
        {
            return GL.GetShaderInfoLog(Handle);
        }

        //static

        public static Shader CompileFromFile(string path, ShaderType shaderType)
        {
            Shader shader = new Shader(shaderType);
            shader.CompileFromFile(path);
            return shader;
        }

        public static Shader CompileFromSourceCode(string soureCode, ShaderType shaderType)
        {
            Shader shader = new Shader(shaderType);
            shader.CompileFromSourceCode(soureCode);
            return shader;
        }
    }
}
