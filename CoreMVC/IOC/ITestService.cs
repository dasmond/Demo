using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.IOC
{
    public interface ITestService
    {
        Guid MyProperty { get; }
        List<string> GetList(string a);
    }
    public interface ITestService2
    {
        Guid MyProperty { get; }
        List<string> GetList();
    }
    public interface ITestService3
    {
        Guid MyProperty { get; }
        List<string> GetList();
    }
}
