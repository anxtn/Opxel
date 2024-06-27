using Opxel.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Opxel.Content
{
    internal interface IAssetLoadable
    {
        public abstract static object Load(string path);
    }
}
