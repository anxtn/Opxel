using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Helpers.Extentions
{
    internal static class EnumExtention
    {
        public static string GetEnumName(this Enum @enum)
        {
            return Convert.ToString(@enum) ?? "undefined Enum";
        }

    }
}
