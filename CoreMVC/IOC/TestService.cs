
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac.Extras.DynamicProxy;

namespace CoreMVC.IOC
{
    [Intercept(typeof(AOPTest))]
    public class TestService : ITestService
    {
        public TestService()
        {
            MyProperty = Guid.NewGuid();
        }
        public Guid MyProperty { get; set; }
        public List<string> GetList(string a)
        {
            return new List<string>() { "LiLei", "ZhangSan", "LiSi" };
        }
    }

    public class TestService2 : ITestService2
    {
        public TestService2()
        {
            MyProperty = Guid.NewGuid();
        }
        public Guid MyProperty { get; set; }
        public List<string> GetList()
        {
            return new List<string>() { "LiLei", "ZhangSan", "LiSi" };
        }
    }

    public class TestService3 : ITestService3
    {

        public TestService3()
        {
            MyProperty = Guid.NewGuid();
        }
        public Guid MyProperty { get; set; }
        public List<string> GetList()
        {
            return new List<string>() { "LiLei", "ZhangSan", "LiSi" };
        }
    }
}
