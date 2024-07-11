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

//TODO: Cubemap class

namespace Opxel.Application
{
    internal class OpxelInstance : GameWindow
    {
        private AssetManager assetManager;
        private FlyingCamera camera;
        private float viewport;
        private OpxelWorld world;
        private ChunkManager chunkManager;

        public OpxelInstance() :
        base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new Vector2i(1600, 900), StartVisible = false, Title="Opxel"})
        {
            UpdateFrequency = 0;
        }

        protected override void OnLoad()
        {
            CenterWindow();

            string iconPath = "C:\\Users\\Anton Müller\\Desktop\\Opxel\\Opxel\\Resources\\Icons\\OpxcelIconPNG.png";
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

            viewport = (float)Size.X / Size.Y;
            camera = new FlyingCamera();
            camera.Speed *= 4;
            world = new OpxelWorld(BlockPalette.Default,
                assetManager.Load<ShaderProgram>("Shaders/ChunkShader.shader.glsl"),
                assetManager.Load<PixelTexture>("Textures/BlockTextures.png"));

            Mesh.DefaultShaderProgram.Use();
            Mesh.DefaultShaderProgram.SetUniform("uViewport", viewport);
            world.BlockShaderProgram.Use();
            world.BlockShaderProgram.SetUniform("uViewport", viewport);

            chunkManager = new ChunkManager(world);

            base.OnLoad();
        }
        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            float deltaTime = (float)frameEventArgs.Time;

            OpxelInput.Update(KeyboardState, MouseState);
            camera.Update(deltaTime);
            Mesh.DefaultShaderProgram.Use();
            Mesh.DefaultShaderProgram.SetUniform("uViewProjection", camera.ViewProjectionMatrix, true);
            world.BlockShaderProgram.Use();
            world.BlockShaderProgram.SetUniform("uViewProjection", camera.ViewProjectionMatrix, true);

            chunkManager.Update((Vector3i)camera.Position);

            base.OnUpdateFrame(frameEventArgs);
        }

        protected override void OnRenderFrame(FrameEventArgs frameEventArgs)
        {
            base.OnRenderFrame(frameEventArgs);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            chunkManager.RenderChunks();

            Context.SwapBuffers();
            base.OnRenderFrame(frameEventArgs);
        }
    }
}
