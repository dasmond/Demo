using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreMVC.Controllers
{
    public class AutoDIController : Controller
    {

        //采用构造注入
        private readonly IOC.ITestService _testService;
        public AutoDIController(IOC.ITestService testService)
        {
            _testService = testService;
        }

        //采用属性注入
        //public IOC.ITestService _testService { get; set; }

        // GET: AutoDI
        public ActionResult Index()
        {
            ViewBag.date = _testService.GetList("Name");
            return View();
        }
    }
}