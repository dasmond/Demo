
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;
using SuperSocket.SocketBase.Protocol;

namespace SocketServer.AiThinker
{
    /// <summary>
    /// 自定义命令行的解析协议
    /// </summary>
    public class GPSRequestInfoParser : IRequestInfoParser<StringRequestInfo>
    {
        #region ICommandParser Members

        public StringRequestInfo ParseRequestInfo(string source)
        {
            //$GNRMC,00039.262,V,2236.3748,N,11350.4114,E,0.000,0.00,060180,,,N*50

            if (!source.StartsWith("$"))
                return null;

            source = source.TrimStart('$');            
            string[] data = source.Split(',');
            string key = data[0];
            string body = source.Substring(source.IndexOf(',')+1);
            string[] parameters = body.Split(',');
            return new StringRequestInfo(key, body, parameters);
        }

        #endregion
    }
}
