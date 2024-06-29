using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Content
{
    internal class Asset
    {
        public readonly string SourcePath;
        public readonly IAssetLoadable Value;
        public readonly Type Type;
        public readonly AssetManager AssetManager;

        public Asset(string sourcePath, IAssetLoadable value, Type type, AssetManager assetManager)
        {
            this.SourcePath = sourcePath;
            this.Value = value;
            this.Type = type;
            this.AssetManager = assetManager;
        }
    }
}
