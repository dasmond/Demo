using System.ComponentModel;

namespace MyCube.Admin.Controllers
{
    /// <summary>系统设置控制器</summary>
    [DisplayName("平台设置")]
    public class CubeController : ConfigController<Setting>
    {
        static CubeController()
        {
            MenuOrder = 34;
        }
    }
}