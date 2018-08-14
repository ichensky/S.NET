using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace S.NET.Serialization
{
    internal class Serializer
    {
        private SerializerSettings serializerSettings;

        public Serializer(SerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings;
        }

        public string SerializeObject(object obj)
        {
            var sb = new StringBuilder();

            sb.Append("(");
            if (serializerSettings.Formatting == Formatting.None)
            {
                sb.Append(SerializeObjectValue(obj));
            }
            else
            {
                sb.Append(SerializeObjectValue(obj));
            }
            sb.Append(")");

            return sb.ToString();
        }

        private string SerializeObjectValue(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            var type = obj.GetType();

            if (type.IsPrimitive()||type.IsValueType)
            {
                sb.Append($"{SerializePrimitivValue(obj, type)}");
            }
            else if (type.IsIEnumerable())
            {
                var values = (IEnumerable)obj;

                var listType = type.GenericTypeArguments.FirstOrDefault();
                var flag = type.GenericTypeArguments.Length == 1 && listType.IsPrimitive();

                if (flag)
                {
                    sb.Append($"{string.Join(" ", values.Cast<object>().Select(x => SerializePrimitivValue(x, listType)))}");
                }
                else
                {
                    foreach (var value in values)
                    {
                        var val = SerializeObjectValue(value);
                        sb.Append($"({val})");
                    }
                }
            }
            else
            {
                var props = type.GetRuntimeProperties();
                foreach (var prop in props)
                {
                    var val = prop.GetValue(obj);
                    var valStr = SerializeObjectValue(val);

                    // TODO: check NullValueHandling attribute
                    //if (valStr.Length > 0)
                    {
                        sb.Append($"({prop.Name} ");
                        sb.Append(valStr);
                        sb.AppendLine($")");
                    }
                }
            }
            return sb.ToString();
        }



        private string SerializePrimitivValue(object val, Type type)
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
    }
}
