using OpenTK.Graphics.ES11;
using Opxel.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Opxel.Content
{
    internal class AssetManager
    {
        public string Directory { get; set; } = @"../../../Resources/";
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
            {".md2", typeof(Mesh )}
        };

        public T Load<T>(string assetPath) where T : IAssetLoadable
        {

            string absolutePath = GetAbsolutePathFromAssetPath(Directory + assetPath);

            if(IsAssetLoaded(absolutePath)) {
                return (T)LoadedAssets[absolutePath].Value;
            }
            else
            {
                Asset asset = new Asset(absolutePath, T.Load<T>(Directory + assetPath), typeof(T));
                LoadedAssets.Add(absolutePath, asset);
                return (T)asset.Value;
            }
            
        }

        public bool IsAssetLoaded(string absolutePath) {
            return LoadedAssets.ContainsKey(absolutePath);
        }

        private string GetAbsolutePathFromAssetPath(string relativePath) { 
            return Path.GetFullPath(relativePath);
        }

        public bool TryGetAssetTypeFromFile(string path, out Type? type)
        {
            string extention = Path.GetExtension(path);
            return AssetFileTypes.TryGetValue(extention, out type);
        }
    }
}
