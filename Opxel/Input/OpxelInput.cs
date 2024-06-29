using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Opxel.Input
{
    internal static class OpxelInput
    {
        public static KeyboardState KeyboardState { get; set; }
        public static MouseState MouseState { get; set; }
        public static Vector2 MouseDelta => MouseState.Delta;
        public static Vector2 MousePosition => MouseState.Position;
        public static Vector2 ScrollDelta => MouseState.ScrollDelta;
        public static Vector2 Scoll => MouseState.Scroll;

        public static bool IsKeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public static bool IsMouseButtonDown(MouseButton button)
        {
            return MouseState.IsButtonDown(button);
        }

        public static void Update(KeyboardState keyboardState, MouseState mouseState)
        {
            KeyboardState = keyboardState;
            MouseState = mouseState;
        }
    }
}
