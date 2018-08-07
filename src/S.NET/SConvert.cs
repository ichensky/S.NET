using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using S.NET.Serialization;
using S.NET.STree;

namespace S.NET
{
    public static class SConvert
    {
        public static T DeserializeObject<T>(string st)
        {
            return new Deserializer().DeserializeObject<T>(ref st);
        }
        public static string SerializeObject(object obj, SerializerSettings serializerSettings)
        {
            return new Serializer(new SerializerSettings()).SerializeObject(obj);
        }
        public static string SerializeObject(object obj, Formatting formatting=Formatting.None)
        {
            return SerializeObject(obj, new SerializerSettings { Formatting=formatting });
        }      
    }
}
