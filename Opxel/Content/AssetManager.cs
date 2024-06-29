using OpenTK.Graphics.ES11;
using OpenTK.Graphics.OpenGL;
using Opxel.Debug;
using Opxel.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Debugger = Opxel.Debug.Debugger;

//Todo : Bake loader delegates

namespace Opxel.Content
{
    internal class AssetManager
    {
        public string RootDirectory { get; set; } = @"../../../Resources/";
        public Dictionary<string /* path */, Asset> LoadedAssets;

        private readonly MethodInfo generalGenericLoadMethod;

        public AssetManager()
        {
            this.LoadedAssets = new Dictionary<string, Asset>();

            foreach(MethodInfo method in GetType().GetMethods()) { 
                if(method.Name == nameof(Load) && method.IsGenericMethod)
                {
                    generalGenericLoadMethod = method;
                }
            }

            if(generalGenericLoadMethod == null)
            {
                throw new Exception("Internal error: Could not find the generic Load Method");
            }
        }

        public Dictionary<string /*extention*/, Type /*assetType*/> AssetFileTypes = new Dictionary<string, Type>() {
            //Texture2D
            {".png", typeof(Texture2D)},
            {".bmp", typeof(Texture2D)},
            {".jpeg", typeof(Texture2D)},

            //Mesh
            {".md2", typeof(Mesh )},

            //Shader
            {".glsl", typeof(ShaderProgram)},
            {".shader", typeof(ShaderProgram)},
        };

        public T Load<T>(string assetPath) where T : IAssetLoadable
        {

            string absolutePath = GetAbsolutePathFromAssetPath(assetPath);

            if(IsAssetLoaded(absolutePath))
            {
                string fileName = Path.GetFileName(absolutePath);
                Debugger.LogWarning($"Tried loading already loaded asset. (asset file: {fileName})");
                return (T)LoadedAssets[absolutePath].Value;
            }
            else
            {
                IAssetLoadable assetValue = T.Load(absolutePath);
                Asset asset = new Asset(absolutePath, assetValue, typeof(T));
                LoadedAssets.Add(absolutePath, asset);
                return (T)assetValue;
            }

        }

        public bool IsSupportedFile(string path)
        {
            string extention = Path.GetExtension(path);
            return AssetFileTypes.ContainsKey(extention);
        }

        public IAssetLoadable Load(string assetPath)
        {
            string absolutePath = GetAbsolutePathFromAssetPath(assetPath);
            string extention = Path.GetExtension(assetPath);

            if(!IsSupportedFile(absolutePath))
            {
                string fileName = Path.GetFileName(absolutePath);
                throw new ArgumentException($"Unnsupported fileformat (file: {fileName})");
            }
               

            Type assetType = AssetFileTypes[extention];

            if(IsAssetLoaded(absolutePath))
            {
                string fileName = Path.GetFileName(absolutePath);
                Debugger.LogWarning($"Tried loading already loaded asset. (asset file: {fileName})");
                return LoadedAssets[absolutePath].Value;
            }
            else
            {
                MethodInfo genericLoadMethod = generalGenericLoadMethod.MakeGenericMethod(assetType);
                return (IAssetLoadable)(genericLoadMethod.Invoke(this, new[] { absolutePath }) ?? throw new NullReferenceException());
            }
        }

        public void Unload<T>(T assetValue) where T : IAssetLoadable
        {
            for(int i = 0; i < LoadedAssets.Count;i++)
            {
                Asset asset = LoadedAssets.Values.ElementAt(i);

                if(asset.Value == (object)assetValue)
                {
                    LoadedAssets.Remove(asset.SourcePath);
                    assetValue.Unload();
                    return;
                }
            }

            Debugger.LogWarning("Tried to unload asset, which hasnt been loaded.");
            
        }

        public bool Unload(string assetPath)
        {
            if(LoadedAssets.TryGetValue(assetPath, out Asset? assetValue))
            {
                ((IAssetLoadable)assetValue.Value).Unload();
                LoadedAssets.Remove(assetPath);
                return true;
            }
            else
            {
                Debugger.LogWarning($"Tried to unload asset, which hasnt been loaded. (assetPath:{assetPath})");
                return false;
            }
        }

        public void UnloadAll()
        {
            foreach(Asset asset in LoadedAssets.Values)
            {
                ((IAssetLoadable)asset).Unload();
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
        
        public void LoadAll()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            string[] paths = System.IO.Directory.GetFiles(Path.GetFullPath(RootDirectory), "*.*", SearchOption.AllDirectories);

            foreach(string path in paths)
            {
                if(IsSupportedFile(path))
                {
                    if(!IsAssetLoaded(path))
                        Load(path);
                }
            }

            stopwatch.Stop();
            Opxel.Debug.Debugger.Log($"Loaded all found assets in {stopwatch.ElapsedMilliseconds} ms.");
        }

        public void PreLoadAll()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            FieldInfo[] fieldsToPreload = StaticPreLoadAttribute.GetAllPreloadField();

            foreach(var field in fieldsToPreload)
            {
                if(field.IsStatic && field.IsDefined(typeof(StaticPreLoadAttribute), true))
                {
                    StaticPreLoadAttribute attrib = field.GetCustomAttribute<StaticPreLoadAttribute>() ?? null!;
                    object loadedValue = Load(attrib.AssetPath);
                    field.SetValue(null, loadedValue);
                }
            }

            stopwatch.Stop();
            Opxel.Debug.Debugger.Log($"Loaded all preload assets in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}
