using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreMVC.Controllers
{
    public class DITestController : Controller
    {
        private readonly IOC.ITestService _testService;
        private readonly IOC.ITestService2 _testService2;
        private readonly IOC.ITestService3 _testService3;
        public DITestController(IOC.ITestService testService, IOC.ITestService2 testService2, IOC.ITestService3 testService3)
        {
            _testService = testService;
            _testService2 = testService2;
            _testService3 = testService3;
        }

        //这里采用了Action注入的方法，,新注入了一个ITestService2 ,来保证2个ITestService2 在同一个作用域.
        public IActionResult Index([FromServices]IOC.ITestService testService11, [FromServices]IOC.ITestService2 testService22)
        {
            ViewBag.date = _testService.GetList("");

            ViewBag.guid = _testService.MyProperty;
            ViewBag.guid11 = testService11.MyProperty;

            ViewBag.guid2 = _testService2.MyProperty;
            ViewBag.guid22 = testService22.MyProperty;

            ViewBag.guid3 = _testService3.MyProperty;

            return View();
        }
    }
}