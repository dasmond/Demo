using System.Web.Mvc;

namespace MyCube.Controllers
{
    /// <summary>主页面</summary>
    //[AllowAnonymous]
    public class CubeHomeController : Controller
    {
        /// <summary>主页面</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "主页面";

            return View();
        }
    }
}