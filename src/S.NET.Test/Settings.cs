using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S.NET.Test
{
    public class User
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Height { get; set; }
    }

    public class Driver
    {
        public string Name { get; set; }
        public string Years { get; set; }
        public string LiveIn { get; set; }
    }

    public class Car
    {
        public string Model { get; set; }
        public string Name { get; set; }
        public List<int> Wheels { get; set; }
        public string Vin { get; set; }
        public Driver Driver { get; set; }
    }

    public enum Version
    {
        Major = 0,
        Minor = 1,
        Other = 2
    }
    public class Meta
    {
        public List<Driver> Drivers { get; set; }
        public Version Version { get; set; }
    }


    public class Settings
    {
        public string App { get; set; }
        public List<string> Tags { get; set; }

        public User User { get; set; }
        public Car Car { get; set; }

        public Meta Meta { get; set; }

        public List<List<int>> Matrix { get; set; }

    }
}
