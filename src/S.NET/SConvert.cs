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
        public static T DeserializeObject<T>(string st)
        {
            var se = new STreeConvert();

            var snode = se.Deserialize(st);

            var type = typeof(T);
            var instance = (T)Deserialize(snode, type);

            return instance;
        }

        private static object Value(Type type, SNodeFull item, bool skip = true)
        {

            object value = null;
            if (IsPrimitive(type))
            {
                var str = item.IsLeaf ? item.Name : item.Items.Skip(1).First().Name;
                if (type.IsEnum)
                {
                    value = Enum.Parse(type, str);
                }
                else
                {
                    value = Convert.ChangeType(str, type);
                }
            }
            else if (type.GetInterface("System.Collections.IEnumerable") != null)
            {
                var listType = type.GenericTypeArguments.FirstOrDefault();
                var listSNodes = item.Items.AsEnumerable();
                if (skip)
                {
                    listSNodes = listSNodes.Skip(1);
                }

                var nextSkip = listType.GetInterface("System.Collections.IEnumerable") == null;

                value = Activator.CreateInstance(type);
                foreach (var lsnode in listSNodes)
                {
                    type.GetMethod("Add").Invoke(value, new[] { Value(listType, lsnode as SNodeFull, nextSkip) });
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

                    var value = Value(prop.PropertyType, item as SNodeFull);
                    prop.SetValue(instance, value, null);
                }
            }

            return instance;
        }

        public static string SerializeObject(object obj)
        {
            var sb = new StringBuilder();
            var type = obj.GetType();
            var props = type.GetRuntimeProperties();
            foreach (var prop in props)
            {
                sb.AppendLine(Value2222(obj, prop));
            }

            return sb.ToString();
        }

        private static bool IsPrimitive(Type type) { return type.IsPrimitive || type == typeof(string) || type.IsEnum; }

        private static string PrimitivValue(object val, Type type)
        {

            if (type.IsEnum)
            {
                val = Convert.ChangeType(val, Enum.GetUnderlyingType(type));
            }
            if (type == typeof(string))
            {
                return $"\"{val}\")";
            }
            else
            {
                return val.ToString();
            }
        }

        private static string Value2222(object obj, PropertyInfo property, bool addName = true)
        {
            var sb = new StringBuilder();
            var type = property.PropertyType;

            if (IsPrimitive(type))
            {
                var val = property.GetValue(obj, null);
                sb.Append($"({property.Name} {PrimitivValue(val, type)}");
            }

            else if (type.GetInterface("System.Collections.IEnumerable") != null)
            {
                var listType = type.GenericTypeArguments.FirstOrDefault();
                var values = (IEnumerable)(property.GetValue(obj, null));

                sb.Append($"(");
                if (addName)
                {
                    sb.Append($"{property.Name} ");
                }

                if (IsPrimitive(listType))
                {
                    sb.Append($"{string.Join(" ", values.Cast<object>().Select(x => PrimitivValue(x, listType)))}");
                }
                else
                {

                    //var newAddName = listType.GetInterface("System.Collections.IEnumerable") == null;

                    //var pp = values.GetType().GetRuntimeProperties().FirstOrDefault(x => x.PropertyType == listType);

                    //foreach (var value in values)
                    //{
                    //    var dfsa = Value2222(value, pp, newAddName);
                    //    sb.AppendLine($"({dfsa})");
                    //}
                }
                sb.Append($")");

            }
            else
            {
                var val = SerializeObject(property.GetValue(obj, null));
                sb.Append($"({property.Name} {val})");
            }
            return sb.ToString();
        }
    }
}
