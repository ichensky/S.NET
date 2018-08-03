using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using S.NET.STree;

namespace S.NET
{
    public static class SConvert
    {
        #region Deserialize
        public static T DeserializeObject<T>(string st)
        {
            var se = new STreeConvert();

            var snode = se.Deserialize(st).Items.FirstOrDefault() as SNodeFull;

            var type = typeof(T);
            var instance = (T)DeserializeValue(snode, type, false);

            return instance;
        }

        private static object DeserializeValue(SNodeFull item, Type type, bool skip = true)
        {

            object value = null;
            if (IsPrimitive(type))
            {
                string str = null;
                if (item.IsLeaf)
                {
                    str = item.Name;
                }
                else
                {
                    if (item.Items.Count==1)
                    {
                        str = item.Items.First().Name;
                    }
                    else
                    {
                        str = item.Items.Skip(1).First().Name;
                    }
                }

                if (type.IsEnum)
                {
                    value = Enum.Parse(type, str);
                }
                else
                {
                    value = Convert.ChangeType(str, type);
                }
            }
            else if (IsIEnumerable(type))
            {
                var listType = type.GenericTypeArguments.FirstOrDefault();
                var listSNodes = item.Items.AsEnumerable();
                if (skip)
                {
                    listSNodes = listSNodes.Skip(1);
                }

                var nextSkip = !(type.GenericTypeArguments.Length==1 && IsIEnumerable(listType));

                value = Activator.CreateInstance(type);
                foreach (var lsnode in listSNodes)
                {
                    type.GetMethod("Add").Invoke(value, new[] { DeserializeValue(lsnode as SNodeFull, listType, nextSkip) });
                }
            }
            else
            {
                value = Deserialize(item, type);
            }
            return value;
        }

        private static object Deserialize(SNode snode, Type type)
        {
            var instance = Activator.CreateInstance(type);
            var props = type.GetRuntimeProperties();

            var items = snode.Items.Where(x => x.Items.Count > 1).ToList();
            foreach (var prop in props)
            {
                var item = items.FirstOrDefault(x => x.Items.First().Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
                if (item != null)
                {
                    var name = prop.Name;

                    var value = DeserializeValue( item as SNodeFull, prop.PropertyType);
                    prop.SetValue(instance, value, null);
                }
            }

            return instance;
        }

        #endregion Deserialize

        #region Serialize

        private static bool IsPrimitive(Type type) { return type.IsPrimitive || type == typeof(string) || type.IsEnum; }
        private static bool IsIEnumerable(Type type) { return type.GetInterface("System.Collections.IEnumerable") != null; }
        private static string SerializePrimitivValue(object val, Type type)
        {

            if (type.IsEnum)
            {
                val = Convert.ChangeType(val, Enum.GetUnderlyingType(type));
            }
            if (type == typeof(string))
            {
                return $"\"{val}\"";
            }
            else
            {
                return val.ToString();
            }
        }
        public static string SerializeObject(object obj)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"({SerializeObjectValue(obj)})");

            return sb.ToString();
        }


        public static string SerializeObjectValue(object obj)
        {
            var sb = new StringBuilder();
            var type = obj.GetType();

            if (IsPrimitive(type))
            {
                sb.Append($"{SerializePrimitivValue(obj, type)}");
            }
            else if (IsIEnumerable(type))
            {
                var values = (IEnumerable)obj;

                    var listType = type.GenericTypeArguments.FirstOrDefault();
                var flag = type.GenericTypeArguments.Length == 1 && IsPrimitive(listType);

                if (flag)
                {
                        sb.Append($"{string.Join(" ", values.Cast<object>().Select(x => SerializePrimitivValue(x, listType)))}");
                }
                else
                {
                    foreach (var value in values)
                    {
                        var val = SerializeObjectValue(value);
                        sb.AppendLine($"({val})");
                    }
                }
            }
            else
            {
                var props = type.GetRuntimeProperties();
                foreach (var prop in props)
                {
                    sb.Append($"({prop.Name} ");
                    var val = prop.GetValue(obj);
                    sb.Append(SerializeObjectValue(val));
                    sb.AppendLine($")");
                }
            }
            return sb.ToString();
        }


        #endregion Serialize
    }
}
