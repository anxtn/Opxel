using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//TODO:
// -GetObjectFromBytes Method should use Spans as Parameter 

namespace Opxel.AssetParsing
{
    internal class ParsingHelper
    {
        public static T GetObjectFromBytes<T>(byte[] buffer) where T : struct
        {
            T? obj = null;
            if((buffer != null) && (buffer.Length > 0))
            {
                IntPtr ptrObj = IntPtr.Zero;
                try
                {
                    int objSize = Marshal.SizeOf<T>();
                    if(objSize > 0)
                    {
                        if(buffer.Length < objSize)
                            throw new Exception(String.Format("Buffer smaller than needed for creation of object of type {0}", typeof(T).Name));
                        ptrObj = Marshal.AllocHGlobal(objSize);
                        if(ptrObj != IntPtr.Zero)
                        {
                            Marshal.Copy(buffer, 0, ptrObj, objSize);
                            obj = Marshal.PtrToStructure<T>(ptrObj);
                        }
                        else
                            throw new Exception(String.Format("Couldn't allocate memory to create object of type {0}", typeof(T).Name));
                    }
                }
                finally
                {
                    if(ptrObj != IntPtr.Zero)
                        Marshal.FreeHGlobal(ptrObj);
                }
            }
            if(obj != null)
                return obj.Value;
            else
                throw new Exception("Struct Parsing Error");
        }

        public static T[] GetObjectsFromBytes<T>(byte[] buffer, int count) where T : struct
        {
            T[] objs = new T[count];
            int structSize = Marshal.SizeOf<T>();

            for(int i = 0;i < count;i++)
            {
                objs[i] = GetObjectFromBytes<T>(buffer[(i * structSize)..((i+1) * structSize)]);
            }

            return objs;
        }

        public static string StructToString<T>(T obj) where T : struct
        {
            StringBuilder sb = new StringBuilder();
            string typeName = typeof(T).Name;
            FieldInfo[] fields = typeof(T).GetFields();

            sb.AppendLine($"{typeName} :");
            sb.AppendLine(new string('-', typeName.Length + 2));



            foreach(FieldInfo field in fields)
            {
                string strValue = field.GetValue(obj)?.ToString() ?? "null";
                sb.AppendLine($"{field.Name} = {strValue}");
            }


            sb.AppendLine(new string('-', typeName.Length + 2));
            return sb.ToString();
        }
    }
}
