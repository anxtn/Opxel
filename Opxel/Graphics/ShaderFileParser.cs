using OpenTK.Graphics.OpenGL;
using System;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Opxel.Graphics
{
    internal static class ShaderFileParser
    {
        private static readonly Dictionary<ShaderType, string> ShaderDefines = new Dictionary<ShaderType, string>
        {
            {ShaderType.VertexShader, "VERTEX"},
            { ShaderType.FragmentShader, "FRAGMENT"},
            {ShaderType.GeometryShader, "GEOMETRY" },
            {ShaderType.ComputeShader, "COMPUTE" },
            {ShaderType.TessControlShader, "TESSCONTROL" },
            {ShaderType.TessEvaluationShader, "TESSEVALUATION" },
        };

        private static readonly Dictionary<string, ShaderType> ShaderDefinesREV = new Dictionary<string, ShaderType>
        {
            {"VERTEX",ShaderType.VertexShader                   },
            {"FRAGMENT", ShaderType.FragmentShader              },
            {"GEOMETRY", ShaderType.GeometryShader              },
            {"COMPUTE", ShaderType.ComputeShader                },
            {"TESSCONTROL" , ShaderType.TessControlShader       },
            {"TESSEVALUATION", ShaderType.TessEvaluationShader  },
        };

        public static ShaderProgram ParseCode(string code)
        {
            code = CompressCode(code);

            ShaderType[] shaderTypes = GetShaderTypesInCode(code);
            List<Shader> shaders = new();

            List<string> lines = code.Split("\n").ToList();

            lines.RemoveAll((line) => string.IsNullOrWhiteSpace(line));

            for(int i = 0;i < lines.Count;i++)
            {
                lines[i] = lines[i].Substring(GetRelevantLineBegin(lines[i]));
                if(lines[i].StartsWith("//"))
                {
                    lines.RemoveAt(i);
                }
            }

            foreach(ShaderType shaderType in shaderTypes)
            {
                Shader shader = new Shader(shaderType);
                shaders.Add(shader);
                string modCode = AddDefineToCode(code, ShaderDefines[shaderType]);
                shader.CompileFromSourceCode(modCode);
            }

            return new ShaderProgram(shaders.ToArray());
        }

        //Puts define in 2nd line
        public static string AddDefineToCode(string code, string defineName)
        {
            int secondLineStart = code.IndexOf('\n');

            if (secondLineStart == -1) throw new ArgumentException("Invalid code (lines of code have to be more than 2)");

            return code.Insert(secondLineStart+1, $"#define {defineName}\n");
        }

        private static int GetRelevantLineBegin(string line)
        {
            for(int i = 0;i < line.Length;i++)
            {
                if(!char.IsWhiteSpace(line[i]))
                {
                    return i;
                }
            }
            return 0;
        }

        public static string CompressCode(string code)
        {
            List<string> lines = code.Split("\n").ToList();

            lines.RemoveAll((line) => string.IsNullOrWhiteSpace(line));

            for(int i = 0;i < lines.Count;i++)
            {
                lines[i] = lines[i].Substring(GetRelevantLineBegin(lines[i]));
                if(lines[i].StartsWith("//"))
                {
                    lines.RemoveAt(i);
                }
            }

            return string.Join('\n', lines);
        }

        //Code have to be compressed
        public static ShaderType[] GetShaderTypesInCode(string code)
        {
            List<ShaderType> shaderTypes = new List<ShaderType>();

            foreach(string line in code.Split("\n"))
            {

                if(line.StartsWith("#ifdef"))
                {
                    string defName =  line.Split(' ')[1];
                    defName = Regex.Replace(defName, @"\s+", "");
                    shaderTypes.Add(ShaderDefinesREV[defName]);
                }
            }

            return shaderTypes.ToArray();
        }
    }
}
