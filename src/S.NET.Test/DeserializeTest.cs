using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace S.NET.Test
{
    public class DeserializeTest
    {
        public enum TestEnum{
            None=0,
            Time=1,
            Frame=2
        }

        [Fact]
        public static void ExceptionArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => SConvert.DeserializeObject<object>(null));
        }
        [Fact]
        public static void ExceptionArgument()
        {
            Assert.Throws<ArgumentException>(() => SConvert.DeserializeObject<object>("("));
            Assert.Throws<ArgumentException>(() => SConvert.DeserializeObject<object>(")"));
            Assert.Throws<ArgumentException>(() => SConvert.DeserializeObject<int>("5"));
        }

        [Fact]
        public static void ExceptionFormat()
        {
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<int>("(()"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<int>("(5()"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<int>("((5)"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<int>("())"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<int>("(5))"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<int>("()5)"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<object>("(()(5)))"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<object>("(())(5))"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<object>("((()(5))"));
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<object>("(()()5))"));
        }

        public static void ExceptionInt()
        {
            Assert.Throws<Exception>(() => SConvert.DeserializeObject<int>("()"));
        }

        [Fact]
        public static void Int()
        {
            Assert.Equal(4, SConvert.DeserializeObject<int>("(4)"));
        }
        [Fact]
        public static void Double()
        {
            Assert.Equal(5.2, SConvert.DeserializeObject<double>("(5.2)"));
        }
        [Fact]
        public static void Decimal()
        {
            Assert.Equal(4.6m, SConvert.DeserializeObject<decimal>("(4.6)"));
        }

        [Fact]
        public static void Enum()
        {
            Assert.Equal(TestEnum.Time, SConvert.DeserializeObject<TestEnum>("(1)"));
        }

        [Fact]
        public static void String()
        {
            Assert.Null(SConvert.DeserializeObject<string>("()"));
            Assert.Null(SConvert.DeserializeObject<string>("(      )"));
            Assert.Equal(string.Empty,SConvert.DeserializeObject<string>($"(\"\")"));
            Assert.Equal(" ",SConvert.DeserializeObject<string>($"(\" \")"));
            Assert.Equal("hello",SConvert.DeserializeObject<string>($"(hello)"));
            Assert.Equal("hello world",SConvert.DeserializeObject<string>($"(\"hello world\")"));
            Assert.Equal("#$123*7^&ashfks", SConvert.DeserializeObject<string>($"(#$123*7^&ashfks)"));
            Assert.Equal("(",SConvert.DeserializeObject<string>($"(\"(\")"));
        }


        [Fact]
        public static void ListInt()
        {
            var list = SConvert.DeserializeObject<List<int>>("(1 2 3)");
            Assert.NotNull(list);
            Assert.Equal(6, list.Sum());
        }

        [Fact]
        public static void ListString()
        {
            var list = SConvert.DeserializeObject<List<string>>("(1 $#%^4 \"543\")");
            Assert.NotNull(list);
            Assert.Equal(3, list.Count());
        }

    }
}
