using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SocketServer.AiThinker
{
    /// <summary>
    /// GPS客户端逻辑连接/会话
    /// <para>AppServer  承载TCP连接的服务器实例。通过AppServer实例获取客户端连接，服务器级别的操作和逻辑。</para>
    /// <para>AppSession 客户端逻辑连接，通过AppSession实例处理基于连接的操作。发送数据到客户端，接收客户端发送的数据或者关闭连接。</para>
    /// </summary>
    public class GPSSession : AppSession<GPSSession>
    {
        public int MacId { get; internal set; }
                
        /// <summary>
        /// 会话开始
        /// </summary>
        protected override void OnSessionStarted()
        {
            //Items
            //Config
            //LocalEndPoint
            //LastActiveTime
            //StartTime
            //Connected
            //Charset
            //PrevCommand
            //CurrentCommand
            Logger.DebugFormat("客户端接入... IP={0},Port={1},Charest={2},SessionID={3}", this.LocalEndPoint.Address, this.LocalEndPoint.Port, this.Charset, this.SessionID);
            this.Send("Welcome to GPS Server");
        }

        /// <summary>
        /// 会话关闭：关闭后执行
        /// </summary>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(CloseReason reason)
        {
            Logger.DebugFormat("客户端关闭... SessionID={0}", this.SessionID);
            base.OnSessionClosed(reason);
        }

        /// <summary>
        /// 未知命令请求
        /// </summary>
        /// <param name="requestInfo"></param>
        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            Logger.DebugFormat("未知请求：{0}", JsonHelper.SerializeObject(requestInfo));
            base.HandleUnknownRequest(requestInfo);
            this.Send("Unknow request");
        }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="e"></param>
        protected override void HandleException(Exception e)
        {
            Logger.ErrorFormat("异常输入：{0}",JsonHelper.SerializeObject(e));
            this.Send("Application error: {0}", e.Message);
        }

    }
}
