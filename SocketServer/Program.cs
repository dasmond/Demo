using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Logging;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请按任意键启动服务...");

            Console.ReadKey();
            Console.WriteLine();

            var appServer = new AppServer();

            //设置 AppServer服务
            if (!appServer.Setup(9091)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //注册 新建会话事件
            appServer.NewSessionConnected += appServer_NewSessionConnected;

            //注册 关闭会话事件
            appServer.SessionClosed += appServer_SessionClosed;

            //注册 收到请求事件
            appServer.NewRequestReceived += appServer_NewRequestReceived; 
            
            //启动 AppServer服务
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("服务启动成功, 按‘q’键停止服务...");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //停止 AppServer服务
            appServer.Stop();

            Console.WriteLine("服务已停止！");
            Console.ReadKey();
            
            LogHelper.Debug("日志：{0}", DateTime.Now.ToFileTime());

            //DemoDelegate.Act1 = DemoDelegate.Method3;
            //DemoDelegate.Method3_Call("外部调用"); 

            //Action<string> act = (x) => { x += "x"; Console.Write(x); };
            //act("test");

            //AppServer  承载TCP连接的服务器实例。通过AppServer实例获取客户端连接，服务器级别的操作和逻辑。
            //AppSession 客户端逻辑连接，通过AppSession实例处理基于连接的操作。发送数据到客户端，接收客户端发送的数据或者关闭连接。
             
        }


        /// <summary>
        /// 新建会话事件处理方法
        /// </summary>
        /// <param name="session"></param>
        private static void appServer_NewSessionConnected(AppSession session)
        {
            string msg = string.Format("新会话... SessionID={0}", session.SessionID);
            session.Send(msg);
            session.Logger.Debug(msg);
        }

        /// <summary>
        /// 关闭会话事件处理方法
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private static void appServer_SessionClosed(AppSession session, CloseReason value)
        { 
            string msg = string.Format("会话关闭！ SessionID={0}", session.SessionID);
            session.Send(msg);
            session.Logger.Debug(msg);
        }

        /// <summary>
        /// 收到请求事件处理方法（(SuperSocket 默认协议使用换行符，信息必须带换行符！)）
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        private static void appServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        { 
            switch (requestInfo.Key.ToUpper())
            {
                case ("ECHO"):
                    session.Send(requestInfo.Body);
                    break;

                case ("ADD"):
                    session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
                    break;

                case ("MULT"):

                    var result = 1;

                    foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
                    {
                        result *= factor;
                    }

                    session.Send(result.ToString());
                    break;
            } 
        }



    }
}
