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
  
    
    class Program
    {
        public enum TestEnum
        {
            None = 0,
            Time = 1,
            Frame = 2
        }


        static void Main(string[] args)
        {
            var xxx = SConvert.DeserializeObject<TestEnum>("(1)");
            //var xx = SConvert.DeserializeObject<DateTime>("(\"8/14/2018 6:27:22 AM\")");
            //var x1 = SConvert.DeserializeObject<byte>("(5)");
            //var x1 = SConvert.DeserializeObject<decimal>($"(\"\\\"\")");

            // var settings = SConvert.DeserializeObject<string>("(5)");
            //var settings = SConvert.DeserializeObject<string>($"({" "})");

            //var settings = SConvert.DeserializeObject<Settings>(")");
            //var settings = SConvert.DeserializeObject<Settings>("(5))");


            //var str = File.ReadAllText("settings.clj");
            //var settings = SConvert.DeserializeObject<Settings>(str);


            //var settings = SConvert.DeserializeObject<List<int>>("(1 2 3)");

            //var dict = new Dictionary<string, Kk>();
            //dict.Add("keyyy", new Kk { MyProperty = 24 });


            //var ms = new Asa{ Name="asa1", BoaProp=new Boa { Goa="boa1", AsaProp=new Asa { Name="asa2" } }  };


            var rr = SConvert.SerializeObject(new { x=DateTimeOffset.UtcNow }, Formatting.Indented);

            //var rr = SConvert.SerializeObject(ms, Formatting.Indented);
        }
    }
}
