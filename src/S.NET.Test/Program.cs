using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S.NET.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = File.ReadAllText("settings.lisp");
            var settings = SConvert.DeserializeObject<Settings>(str);
        }
    }
}
