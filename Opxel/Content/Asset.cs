using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Content
{
    internal class Asset
    {
        public string SourcePath;
        public object Value;
        public Type Type;

        public Asset(string sourcePath, object value, Type type)
        {
            this.SourcePath = sourcePath;
            this.Value = value;
            this.Type = type;
        }

    }

    //internal class Asset<T> where T : IAssetLoadable<T>
    //{
    //    public string Path;
    //    public bool Loaded;
    //    private object _value => Value;
    //    public T Value;

    //    public Asset(string path)
    //    {
    //         this.Path = path;
    //         Loaded = false;
    //    }
    //}
}
