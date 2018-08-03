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
    class Cap {
        public string Name { get; set; }
        public int Years { get; set; }
    }
    class Kk {
       // public int MyProperty { get; set; }
        public List<Cap> Vv { get; set; }
    }
    class Program
    {
      
        static void Main(string[] args)
        {
            var str = File.ReadAllText("settings.clj");
            //var settings = SConvert.DeserializeObject<Settings>(str);


            //var settings = SConvert.DeserializeObject<List<int>>("(1 2 3)");

            //var dict = new Dictionary<string, Kk>();
            //dict.Add("keyyy", new Kk { MyProperty = 24 });


            

            var rr = SConvert.SerializeObject( new Kk() { Vv = new List<Cap> {
                new Cap{ Name="Bob" },
                new Cap{ Name="Carl" }
            } });
        }
    }
}
