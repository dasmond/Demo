using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Command;

namespace SocketServer.AiThinker
{
    /// <summary>
    /// 
    /// </summary>
    public class GPSCommand : CommandBase<GPSSession, StringRequestInfo>
    {
        /// <summary>
        /// 命令处理类
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(GPSSession session, StringRequestInfo requestInfo)
        {
            //session.CustomID = new Random().Next(10000, 99999);
            //session.CustomName = "hello word";

            var key = requestInfo.Key;
            var param = requestInfo.Parameters;
            var body = requestInfo.Body;
            //返回随机数session.Send(session.CustomID.ToString());
            //返回
            session.Send("命令发送");
        }

    }
}
