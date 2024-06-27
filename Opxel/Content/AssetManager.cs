using OpenTK.Graphics.ES11;
using Opxel.Debug;
using Opxel.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Todo : Bake loader delegates

namespace Opxel.Content
{
    internal class AssetManager
    {
        public string RootDirectory { get; set; } = @"../../../Resources/";
        public Dictionary<string /* path */, Asset> LoadedAssets;

        public AssetManager()
        {
            this.LoadedAssets = new Dictionary<string, Asset>();
        }

        public Dictionary<string /*extention*/, Type /*assetType*/> AssetFileTypes = new Dictionary<string, Type>() {
            //Texture2D
            {".png", typeof(Texture2D)},
            {".bmp", typeof(Texture2D)},
            {".jpeg", typeof(Texture2D)},

            //Mesh
            {".md2", typeof(Mesh )},

            //Shader
            {".glsl", typeof(ShaderProgram)}
        };

        public T Load<T>(string assetPath) where T : IAssetLoadable
        {

            string absolutePath = GetAbsolutePathFromAssetPath(assetPath);

            if(IsAssetLoaded(absolutePath))
            {
                Debugger.LogWarning("Loade same Asset twice.");
                return (T)LoadedAssets[absolutePath].Value;
            }
            else
            {
                Asset asset = new Asset(absolutePath, T.Load(absolutePath), typeof(T));
                LoadedAssets.Add(absolutePath, asset);
                return (T)asset.Value;
            }

        }

        public bool IsSupportedFile(string path)
        {
            string extention = Path.GetExtension(path);
            return AssetFileTypes.ContainsKey(extention);
        }

        public object Load(string assetPath)
        {
            string absolutePath = GetAbsolutePathFromAssetPath(assetPath);
            string extention = Path.GetExtension(assetPath);
            if(!IsSupportedFile(absolutePath))
                throw new ArgumentException($"Unnsupported fileformat (\"{extention}\")");

            Type assetType = AssetFileTypes[extention];



            if(IsAssetLoaded(absolutePath))
            {
                Debugger.LogWarning("Loade same Asset twice.");
                return LoadedAssets[absolutePath].Value;
            }
            else
            {
                Func<string, object> loaderMethod = assetType.GetMethod("Load")?.CreateDelegate<Func<string, object>>()
                    ?? throw new Exception("Couldnt access load method from the given type.");
                object assetValue = loaderMethod(absolutePath); 
                Asset asset = new Asset(absolutePath, assetValue, assetType);
                LoadedAssets.Add(absolutePath, asset);
                return assetValue;
            }
        }

        public bool IsAssetLoaded(string absolutePath)
        {
            return LoadedAssets.ContainsKey(absolutePath);
        }

        private string GetAbsolutePathFromAssetPath(string assetPath)
        {
            return Path.GetFullPath(Path.Combine(RootDirectory, assetPath));
        }

        public bool TryGetAssetTypeFromFile(string path, out Type? type)
        {
            string extention = Path.GetExtension(path);
            return AssetFileTypes.TryGetValue(extention, out type);
        }

        public void PreLoadAll()
        {
            string[] paths = System.IO.Directory.GetFiles(Path.GetFullPath(RootDirectory), "*.*", SearchOption.AllDirectories);

            foreach(string path in paths) {
                if(IsSupportedFile(path))
                {
                    Load(path);
                }
            }
        }
    }
}
