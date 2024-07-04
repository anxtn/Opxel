using OpenTK.Graphics.OpenGL;
using Opxel.Helpers.Extentions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Opxel.Debug
{
    internal static class Debugger
    {
        public static ConsoleColor DefaultColor = ConsoleColor.White;
        public static ConsoleColor ErrorColor = ConsoleColor.Red;
        public static ConsoleColor WarningColor = ConsoleColor.Yellow;

        //Idk aber vielleicht könnte es unter bestimmten Bedingungen Probleme mit dem GC geben wegen dem DebugProc
        public static void SetupOpenGLDebugging()
        {
            DebugProc DebugProc = (source, type, id, severity, length, message, userParam) =>
            {
                string messageString = Marshal.PtrToStringAnsi(message, length);
                switch(type)
                {
                    case DebugType.DebugTypeError:
                        LogError($"OpenGL Error: ({((ErrorCode)id).GetEnumName()}): {messageString}");
                        break;
                    case DebugType.DebugTypePerformance:
                        LogWarning($"OpenGL Warning: ({((ErrorCode)id).GetEnumName()}): {messageString}");
                        break;
                    default:
                        ; //Den Debug speichern zum Debuggen
                        break;
                }
            };
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback(DebugProc, 0);
        }

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
            LogColor($"Error: {message}", ErrorColor);
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
