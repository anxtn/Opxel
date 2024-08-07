﻿using System.Reflection;

namespace Opxel.Content
{

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    internal sealed class StaticPreLoadAttribute : Attribute
    {
        private readonly string _assetPath;
        public string AssetPath => _assetPath;
        public Type? SpecificType { get; set; } //AssetManager class sets this prop in PreLoadAll() method


        public StaticPreLoadAttribute(string assetPath)
        {
            this._assetPath = assetPath;
        }

        private static bool ValidateField(FieldInfo field)
        {
            if(!field.IsDefined(typeof(StaticPreLoadAttribute), true))
                return false;

            if(!field.IsStatic)
                throw new Exception($"The StaticPreLoadAttribute can only be used for static members. (field: {field.Name} from {field.DeclaringType?.FullName})");

            return true;
        }

        //very slow
        public static FieldInfo[] GetAllPreloadField()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] allTypes = assembly.GetTypes();

            List<FieldInfo> attribFields = new List<FieldInfo>();

            foreach(Type type in allTypes)
            {
                foreach(FieldInfo field in type.GetFields())
                {
                    if(ValidateField(field))
                    {
                        attribFields.Add(field);
                    }
                }
            }

            return attribFields.ToArray();
        }
    }
}
