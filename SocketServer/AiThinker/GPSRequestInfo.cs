using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.AiThinker
{
    /// <summary>
    /// 实体类：客户端请求
    /// </summary>
    public class GPSRequestInfo : IRequestInfo
    {
        /// <summary>
        /// 接口实现：命令字符（）
        /// </summary>
        public string Key { get; set; }

        public int DeviceId { get; set; }


    }


}
