using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using Opxel.Debug;
using Opxel.Graphics;
using Opxel.Mathematics;
using Opxel.Input;
using Opxel.AssetParsing;

namespace Opxel.Application
{
    internal class OpxelInstance : GameWindow
    {

        private VertexArray vertexArray;

        private ShaderProgram shaderProgram;

        private FlyingCamera camera;

        private Mesh mesh;
        private float rotY = 0;

        private float viewport;

        public OpxelInstance() :
        base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new Vector2i(1600, 900) })
        {

        }

        protected override void OnLoad()
        {
            int major = OpenGLParameter.MajorVersion;
            int minor = GL.GetInteger(GetPName.MinorVersion);
            CenterWindow();

            Console.WriteLine($"Using Open GL Version {major}");
            Console.WriteLine(BitConverter.IsLittleEndian ? "Using Little Endian" : "Using Big Endian");


            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);

            Vector3[] verticesV3 =
           {
                new Vector3(-0.5f, 0.5f, 0f), //TopLeft
               new Vector3( 0.5f, 0.5f, 0f),  //TopRight 
               new Vector3( 0.5f, -0.5f, 0f), //BottomRight
                new Vector3(-0.5f, -0.5f, 0f) //BottomLeft
            };

            Color4[] colorsC4 =
           {
                new Color4( 1f,0f,0f,1f),
                new Color4(1f,1f,0f,1f),
                new Color4(0f,0f,1f,1f),
                new Color4(0f,1f,0f,1f)
            };

            uint[] indices =
            {
                0,1,2,0,2,3
            };

            string shaderPrgPath = @"C:\Users\Anton Müller\Desktop\Opxel\Opxel\Resources\Shaders\SimpleShader.shader.glsl";
            shaderProgram = ShaderProgram.FromShaderFormatFile(shaderPrgPath);
            Mesh.DefaultShaderProgram = shaderProgram;

            viewport = (float)Size.X / Size.Y;
            camera = new FlyingCamera();

            shaderProgram.Use();
            shaderProgram.SetUniform("uViewport", viewport);

            //mesh = new Mesh();
            //mesh.Indices = indices;
            //mesh.Positions = verticesV3;
            //mesh.Colors = colorsC4;

            string path = "C:\\Users\\Anton Müller\\Downloads\\Monkey.md2";

            MD2Object obj = MD2Parser.ParseFile(path);
            mesh = obj.ToMesh();

            base.OnLoad();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            float deltaTime = (float)e.Time;

            OpxelInput.Update(KeyboardState, MouseState);
            camera.Update(deltaTime);
            shaderProgram.SetUniform("uViewProjection", camera.ViewProjectionMatrix, true);

            rotY += deltaTime;
            mesh.Transform.RotateByDegrees(new Vector3(0, 0, rotY));

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
