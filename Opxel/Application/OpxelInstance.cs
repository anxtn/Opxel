using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using Opxel.Debug;
using Opxel.Graphics;
using Opxel.Mathematics;
using Opxel.Input;
using Opxel.AssetParsing;
using Opxel.Content;

//TODO: Cubemap class

namespace Opxel.Application
{
    internal class OpxelInstance : GameWindow
    {

        private VertexArray vertexArray;
        private AssetManager assetManager;

        private ShaderProgram shaderProgram;

        private FlyingCamera camera;

        private Mesh mesh;
        private float rotY = 0;

        private float viewport;

        public OpxelInstance() :
        base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new Vector2i(1600, 900), StartVisible = false, Title="Opxel"})
        {

        }

        protected override void OnLoad()
        {
            int major = OpenGLParameter.MajorVersion;
            int minor = GL.GetInteger(GetPName.MinorVersion);
            CenterWindow();

            string iconPath = "C:\\Users\\Anton Müller\\Desktop\\Opxel\\Opxel\\Resources\\Icons\\OpxcelIconPNG.png";
            Icon = ImageLoader.CreateWindowIcon(iconPath);

            IsVisible = true;   

            Console.WriteLine($"Using Open GL Version {major}");
            Console.WriteLine(BitConverter.IsLittleEndian ? "Using Little Endian" : "Using Big Endian");

            assetManager = new AssetManager();
            shaderProgram = assetManager.Load<ShaderProgram>("Shaders/SimpleShader.shader.glsl");
            Mesh.DefaultShaderProgram = shaderProgram;
            assetManager.PreLoadAll();

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);

            

            viewport = (float)Size.X / Size.Y;
            camera = new FlyingCamera();

            shaderProgram.Use();
            shaderProgram.SetUniform("uViewport", viewport);

            mesh = assetManager.Load<Mesh>("Models/Monkey.md2");

            Console.WriteLine("########## Assets ##############");
            foreach (string assetPath in assetManager.LoadedAssets.Keys)
            {
                Asset asset = assetManager.LoadedAssets[assetPath];
                string name = Path.GetFileName(assetPath);
                Console.WriteLine($"File: {name}; Type = {asset.Type}");
            }

            base.OnLoad();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            float deltaTime = (float)e.Time;

            OpxelInput.Update(KeyboardState, MouseState);
            camera.Update(deltaTime);
            shaderProgram.SetUniform("uViewProjection", camera.ViewProjectionMatrix, true);

            rotY += deltaTime;
            //mesh.Transform.RotateByDegrees(new Vector3(0, 0, rotY));

            Debugger.CheckGLError();
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            mesh.Render();

            SwapBuffers();
        }
    }
}
