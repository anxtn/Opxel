using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;

namespace Opxel.Debug
{
    internal static class Debugger
    {
        public static ConsoleColor DefaultColor = ConsoleColor.White;
        public static ConsoleColor ErrorColor = ConsoleColor.Red;
        public static ConsoleColor WarningColor = ConsoleColor.Yellow;

        public static void LogColor(object message, ConsoleColor color)
        {
            ConsoleColor preColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = preColor;
        }

        public static void Log(object message)
        {
            LogColor(message, DefaultColor);
        }
        public static void LogError(object message)
        {
            LogColor(message, ErrorColor);
        }

        public static void LogWarning(object message)
        {
            LogColor(message, WarningColor);
        }

        public static void CheckGLError([CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerMember = "", [CallerFilePath] string filePath = "")
        {
            ErrorCode error = GL.GetError();
            if(error != ErrorCode.NoError)
            {
                string errorStr = Enum.GetName<ErrorCode>(error) ?? $"Error enum Code {(int)error}";
                string fileName = Path.GetFileName(filePath);
                string message = $"Open GL Error in {fileName} ({callerMember}) before called Line {lineNumber}: {errorStr}";
                LogError(message);
            }
        }
    }
}
