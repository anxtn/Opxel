using Opxel.Content;
using Opxel.Voxels;
using Opxel.Graphics;
using System.Runtime.CompilerServices;

namespace Opxel.Graphics
{

	class VoxelMesh : IAssetLoadable, IDisposable
	{
		private readonly GraphicBuffer VertexBuffer;
		private readonly GraphicBuffer IndexBuffer;
		private readonly VertexArray VertexArray;

		private bool disposed;

		public VoxelMesh()
		{
			
			disposed = false;
		}

		public static IAssetLoadable Load(string path)
		{
			return new VoxelMesh();
		}

		public void Dispose()
		{
			if(disposed) return;
			disposed = true;
			GC.SuppressFinalize(this);
		}

		~VoxelMesh()
		{
			Dispose();
		}
	}
}
