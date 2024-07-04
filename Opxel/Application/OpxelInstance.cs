﻿using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using Opxel.Debug;
using Opxel.Graphics;
using Opxel.Mathematics;
using Opxel.Input;
using Opxel.Content;
using Opxel.Voxels;
using Opxel.Helpers.Extentions;
using System.Runtime.InteropServices;

//TODO: Cubemap class

namespace Opxel.Application
{
    internal class OpxelInstance : GameWindow
    {
        private AssetManager assetManager;
        private FlyingCamera camera;
        private Chunk chunk;
        private Chunk chunk2;
        private float viewport;
        private OpxelWorld world;

        public OpxelInstance() :
        base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new Vector2i(1600, 900), StartVisible = false, Title="Opxel"})
        {

        }

        protected override void OnLoad()
        {
            int major = OpenGLParameter.MajorVersion;
            int minor = OpenGLParameter.MinorVersion;
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

            world = new OpxelWorld(BlockPalette.Default,
                assetManager.Load<ShaderProgram>("Shaders/ChunkShader.shader.glsl"),
                assetManager.Load<PixelTexture>("Textures/BlockTextures.png"));

            Mesh.DefaultShaderProgram.Use();
            Mesh.DefaultShaderProgram.SetUniform("uViewport", viewport);
            world.BlockShaderProgram.Use();
            world.BlockShaderProgram.SetUniform("uViewport", viewport);

            
            chunk = new Chunk(world,new Vector3i(0));
            chunk2 = new Chunk(world,new Vector3i(Chunk.Size,0,0));

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
            base.OnUpdateFrame(frameEventArgs);
        }

        protected override void OnRenderFrame(FrameEventArgs frameEventArgs)
        {
            base.OnRenderFrame(frameEventArgs);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            chunk.ChunkMesh.Render();
            chunk2.ChunkMesh.Render();

            Context.SwapBuffers();
            base.OnRenderFrame(frameEventArgs);
        }
    }
}
