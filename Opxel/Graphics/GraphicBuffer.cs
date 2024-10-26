﻿using OpenTK.Graphics.OpenGL;
using Opxel.Debug;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace Opxel.Graphics
{
    internal class GraphicBuffer : IDisposable
    {
        public int Handle {  get; private set; }
        public readonly BufferUsageHint Usage;
        public readonly BufferTarget Target;
        public int Length { get; private set; }
        public int ByteLength { get; private set; }

        private bool disposed;
        public GraphicBuffer(BufferTarget type, BufferUsageHint usage)
        {
            disposed = false;
            Handle = GL.GenBuffer();
            Length = -1;
            ByteLength = -1;
            this.Target = type;
            this.Usage = usage;
        }

        public GraphicBuffer(BufferTarget type)
            : this(type, BufferUsageHint.StaticDraw)
        { }

        public void SetData<T>(T[] data) where T : unmanaged
        {
            Length = data.Length;
            ByteLength = Length * Marshal.SizeOf<T>();
            Bind();
            GL.BufferData(Target, ByteLength, data, Usage);
        }

        public void SetData<T>(List<T> data) where T : unmanaged
        {
            Span<T> dataSpan = CollectionsMarshal.AsSpan(data);
            SetData(dataSpan);
        }

        public void SetData<T>(Span<T> data) where T : unmanaged
        {
            Length = data.Length;
            ByteLength = Length * Marshal.SizeOf<T>();
            Bind();
            ref T refData = ref data.GetPinnableReference();
            GL.BufferData(Target, ByteLength,ref refData, Usage);
        }

        public T[] ReadData<T>() where T : unmanaged 
        {
            T[] data = new T[Length];
            Bind();
            GL.GetBufferSubData(Target, IntPtr.Zero, ByteLength, data);
            return data;
        }

        public int GetParameter(BufferParameterName parameterName)
        {
            int value = -1;
            GL.GetBufferParameter(Target,parameterName, out value); 
            return value;
        }

        public void Bind()
        {
            GL.BindBuffer(Target, Handle);
        }

        public void Dispose()
        {
            if(disposed) return;
            disposed = true;
            GL.DeleteBuffer(Handle);
            GC.SuppressFinalize(this);
        }

        ~GraphicBuffer()
        {
            Dispose();
        }
    }
}
