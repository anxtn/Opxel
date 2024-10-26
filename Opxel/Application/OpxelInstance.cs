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
using Opxel.World;

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

        private static Vector2i resolution = new Vector2i((int)(1920.0/1.33) ,(int)(1080.0/1.33) );

        public OpxelInstance() :
        base(GameWindowSettings.Default, new NativeWindowSettings() {ClientSize = resolution, StartVisible = false, Title = "Opxel" })
        {
            WindowState = WindowState.Normal;
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
               assetManager.Load<ShaderProgram>("Shaders/BlockShader.glsl"),
               assetManager.Load<Texture2D>("Textures/BlockTextures.png"));
            world.Player.Speed *= 4;

            int h = world.BlockTexture.Handle;

            world.Player.Transform.Position = new Vector3(0, 30, 0);

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
            Mesh.DefaultShaderProgram.SetUniform("uViewProjection", world.Player.Camera.ViewProjectionMatrix);
            world.BlockShaderProgram.Use();
            world.BlockShaderProgram.SetUniform("uViewProjection", world.Player.Camera.ViewProjectionMatrix);
            world.ChunkManager.Update();
            world.ChunkManager.UnloadUnusedBlockData(10);
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
