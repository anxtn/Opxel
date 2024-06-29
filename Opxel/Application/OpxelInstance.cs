using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using Opxel.Debug;
using Opxel.Graphics;
using Opxel.Mathematics;
using Opxel.Input;
using Opxel.Content;

//TODO: Cubemap class

namespace Opxel.Application
{
    internal class OpxelInstance : GameWindow
    {
        private AssetManager assetManager;
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
            assetManager.PreLoadAll();
            assetManager.LoadAll();

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);

            

            viewport = (float)Size.X / Size.Y;
            camera = new FlyingCamera();

            Mesh.DefaultShaderProgram.Use();
            Mesh.DefaultShaderProgram.SetUniform("uViewport", viewport);

            

            mesh = assetManager.Load<Mesh>("Models/Monkey.md2");

            base.OnLoad();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            float deltaTime = (float)e.Time;

            OpxelInput.Update(KeyboardState, MouseState);
            camera.Update(deltaTime);
            Mesh.DefaultShaderProgram.SetUniform("uViewProjection", camera.ViewProjectionMatrix, true);

            rotY += deltaTime;

            mesh.Transform.RotateByRadiants(new Vector3(0, rotY, 0));

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
