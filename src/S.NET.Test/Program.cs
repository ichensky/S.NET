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
      
        static void Main(string[] args)
        {
            //var x1 = SConvert.DeserializeObject<object>("((()(5))");
            var x1 = SConvert.DeserializeObject<decimal>("(4.3)");

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



            //var rr = SConvert.SerializeObject(ms, Formatting.Indented);
        }
    }
}
