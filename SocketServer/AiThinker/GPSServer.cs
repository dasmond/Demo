using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;

namespace SocketServer.AiThinker
{
    /// <summary>
    /// 服务器实例 
    /// <para>AppServer  承载TCP连接的服务器实例。通过AppServer实例获取客户端连接，服务器级别的操作和逻辑。</para>
    /// <para>AppSession 客户端逻辑连接，通过AppSession实例处理基于连接的操作。发送数据到客户端，接收客户端发送的数据或者关闭连接。</para>
    /// </summary>
    public class GPSServer : AppServer<GPSSession>
    {
        /// <summary>
        /// 服务配置
        /// <para>http://docs.supersocket.net/v1-6/zh-CN/SuperSocket-Basic-Configuration</para>
        /// </summary>
        /// <param name="rootConfig">根配置</param>
        /// <param name="serverConfig">服务器实例配置</param>
        /// <returns></returns>
        protected override bool Setup(IRootConfig rootConfig, IServerConfig serverConfig)
        {
            return base.Setup(rootConfig, serverConfig);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }
    }
}
