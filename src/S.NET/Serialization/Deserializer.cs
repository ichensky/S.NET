using S.NET.STree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace S.NET.Serialization
{
    internal class Deserializer
    {
        public T DeserializeObject<T>(ref string st)
        {
            var se = new STreeConvert();

            var snode = se.Deserialize(st).Items.FirstOrDefault() as SNodeFull;

            var type = typeof(T);
            var instance = (T)DeserializeValue(snode, type, false);

            return instance;
        }

        private object DeserializeValue(SNodeFull item, Type type, bool skip = true)
        {
            object value = null;
            if (type.IsPrimitive())
            {
                string str = null;
                if (item.IsLeaf)
                {
                    str = item.Name;
                }
                else
                {
                    if (item.Items.Count == 1)
                    {
                        str = item.Items.First().Name;
                    }
                    else if (item.Items.Count > 1)
                    {
                        str = item.Items.Skip(1).First().Name;
                    }
                }

                if (!type.IsString() && string.IsNullOrWhiteSpace(str))
                {
                    throw new Exception($"No content found for nullable type: '{type.Name}'.");
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
            else if (type.IsIEnumerable())
            {
                var listType = type.GenericTypeArguments.FirstOrDefault();
                var listSNodes = item.Items.AsEnumerable();
                if (skip)
                {
                    listSNodes = listSNodes.Skip(1);
                }

                var nextSkip = !(type.GenericTypeArguments.Length == 1 && listType.IsIEnumerable());

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

        private object Deserialize(SNode snode, Type type)
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

                    var value = DeserializeValue(item as SNodeFull, prop.PropertyType);
                    prop.SetValue(instance, value, null);
                }
            }

            return instance;
        }

    }
}
