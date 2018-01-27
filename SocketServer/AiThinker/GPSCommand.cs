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
    //GPSCommand 命令

    /// <summary>
    /// 请求命令：ECHO
    /// </summary>
    public class ECHO : CommandBase<GPSSession, StringRequestInfo>
    {
        /// <summary>
        /// 命令处理类
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(GPSSession session, StringRequestInfo requestInfo)
        {
            var key = requestInfo.Key;
            var param = requestInfo.Parameters;
            var body = requestInfo.Body;
                        
            //("服务器收到命令：ECHO ", JsonHelper.SerializeObject(requestInfo));
            session.Send("服务器收到命令：ECHO");
        }

    }
}
