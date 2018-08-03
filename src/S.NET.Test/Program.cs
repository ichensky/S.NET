using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace S.NET.Test
{
    class Kk {
        public int MyProperty { get; set; }
       // public List<List<int>> Vv { get; set; }
    }
    class Program
    {
      
        static void Main(string[] args)
        {
            var str = File.ReadAllText("settings.clj");
            var settings = SConvert.DeserializeObject<Settings>(str);


            //var settings = SConvert.DeserializeObject<List<int>>("(1 2 3)");

            var rr = SConvert.SerializeObject(new Kk { MyProperty=324 });
        }
    }
}
