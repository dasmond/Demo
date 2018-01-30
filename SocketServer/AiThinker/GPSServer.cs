using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

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
        /// 服务构造
        /// <para>自定义命令行协议 GPSRequestInfoParser</para>
        /// </summary>
        public GPSServer() : base(new CommandLineReceiveFilterFactory(Encoding.Default, new GPSRequestInfoParser()))
        {
            //GPSServer() : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(",", ",")))
            //内置的协议
            //CommandLineReceiveFilterFactory - 命令行协议
            //TerminatorReceiveFilter - 结束符协议
            //CountSpliterReceiveFilter - 固定数量分隔符协议
            //FixedSizeReceiveFilter - 固定请求大小的协议
            //BeginEndMarkReceiveFilter - 带起止符的协议
            //FixedHeaderReceiveFilter - 头部格式固定并且包含内容长度的协议
            

        }

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

        /// <summary>
        /// 服务开始
        /// </summary>
        protected override void OnStarted()
        {
            base.OnStarted();
        }

        /// <summary>
        /// 服务停止
        /// </summary>
        protected override void OnStopped()
        {
            base.OnStopped();
        }
    }
}
