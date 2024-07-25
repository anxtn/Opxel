using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using Opxel.Debug;
using Opxel.Graphics;
using Opxel.Mathematics;
using Opxel.Input;
using Opxel.Content;
using Opxel.Voxels;
using System.ComponentModel;

//TODO:
//      Profiler for better performence (in vs)
//      CubeMap class
//      FrameBuffer class

namespace Opxel.Application
{
    internal class OpxelInstance : GameWindow
    {
        private AssetManager assetManager;
        private OpxelWorld world;

        private static Vector2i resolution = new Vector2i(1920 ,1080 );

        public OpxelInstance() :
        base(GameWindowSettings.Default, new NativeWindowSettings() {ClientSize = resolution, StartVisible = false, Title = "Opxel" })
        {
            WindowState = WindowState.Fullscreen;
        }

        protected override void OnLoad()
        {
            
            CenterWindow();

            string iconPath = "../../../Resources/Icons/OpxcelIconPNG.png";
            Icon = ImageLoader.CreateWindowIcon(iconPath);

            IsVisible = true;

            assetManager = new AssetManager();
            assetManager.PreLoadAll();

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

#if DEBUG
            Debugger.SetupOpenGLDebugging();
#endif
            world = new OpxelWorld(BlockPalette.Default,
               assetManager.Load<ShaderProgram>("Shaders/ChunkShader.shader.glsl"),
               assetManager.Load<PixelTexture>("Textures/BlockTextures.png"));
            world.Player.Speed *= 4;

            int h = world.BlockTexture.Handle;

            world.Player.Transform.Position = new Vector3(0, 20, 0);

            base.OnLoad();
        }
        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            float deltaTime = (float)frameEventArgs.Time;

            OpxelInput.Update(KeyboardState, MouseState);

            if(OpxelInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                Close();

            world.Player.Update(deltaTime);
            Mesh.DefaultShaderProgram.Use();
            Mesh.DefaultShaderProgram.SetUniform("uViewProjection", world.Player.Camera.ViewProjectionMatrix, true);
            world.BlockShaderProgram.Use();
            world.BlockShaderProgram.SetUniform("uViewProjection", world.Player.Camera.ViewProjectionMatrix, true);
            world.ChunkManager.Update();

            base.OnUpdateFrame(frameEventArgs);
        }

        protected override void OnRenderFrame(FrameEventArgs frameEventArgs)
        {
            base.OnRenderFrame(frameEventArgs);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            world.ChunkManager.RenderChunks();

            Context.SwapBuffers();
            base.OnRenderFrame(frameEventArgs);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            if(world != null)
                world.Player.Camera.AspectRation = e.Width / (float)e.Height;
            base.OnResize(e);   
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Console.WriteLine("\n\n");
            base.OnClosing(e);
        }
    }
}
