using Opxel.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Opxel.Content
{
    internal interface IAssetLoadable : IDisposable
    {
        public abstract static IAssetLoadable Load(string path);
        public static bool IsLoadableFile(string path)
        {
            return true;
        }

        public void Unload()
        {
            Dispose();
        }
    }
}
