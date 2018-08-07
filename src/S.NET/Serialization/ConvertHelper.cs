using System;
using System.Collections.Generic;
using System.Text;

namespace S.NET.Serialization
{
    internal static class ConvertHelper
    {
        public static bool IsDecimal(this Type type) { return type == typeof(decimal); }
        public static bool IsString(this Type type) { return type == typeof(string); }
        public static bool IsPrimitive(this Type type) { return type.IsPrimitive || type.IsString() || type.IsEnum || type.IsDecimal(); }
        public static bool IsIEnumerable(this Type type) { return type.GetInterface("System.Collections.IEnumerable") != null; }
    }
}
