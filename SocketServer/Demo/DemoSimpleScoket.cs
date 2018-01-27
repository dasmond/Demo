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

using SocketServer.AiThinker;
using SuperSocket.SocketBase.Config;

namespace SocketServer.Demo
{
    /// <summary>
    /// SuperSocket Server 示例
    /// </summary>
    public static class DemoSimpleScoket
    {
        #region - MainDemoA 简单示例 -
        /// <summary>
        /// SuperSocket 简单示例
        /// </summary>
        /// <param name="args"></param>
        public static void MainDemoA(string[] args)
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
                default:
                    break;
            }

            session.Send(JsonHelper.SerializeObject(requestInfo));
            LogHelper.Debug("收到消息：" + JsonHelper.SerializeObject(requestInfo));
        }
        #endregion

        #region - MainDemoB 示例【AppServer,AppSession】  -
        /// <summary>
        /// SuperSocket 示例【AppServer,AppSession】 
        /// </summary>
        public static void MainDemoB()
        {
            LogHelper.Info("服务启动...");

            var server = new GPSServer();

            //设置服务
            if (!server.Setup(9091)) //Setup with listening port
            {
                LogHelper.Fatal("服务配置失败！");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //启动服务
            if (!server.Start())
            {
                LogHelper.Fatal("启动失败！");
                Console.ReadKey();
                return;
            }

            LogHelper.Info("服务启动成功, 按‘q’键停止服务...");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //停止服务
            server.Stop();

            LogHelper.Debug("服务已停止：{0}", DateTime.Now.ToFileTime());
            Console.ReadKey();

        }

        /// <summary>
        /// 服务器实例
        /// <para>AppServer  承载TCP连接的服务器实例。通过AppServer实例获取客户端连接，服务器级别的操作和逻辑。</para>
        /// <para>AppSession 客户端逻辑连接，通过AppSession实例处理基于连接的操作。发送数据到客户端，接收客户端发送的数据或者关闭连接。</para>
        /// </summary>
        private class TelnetServer : AppServer<TelnetSession>
        {
            /// <summary>
            /// 服务配置 
            /// http://docs.supersocket.net/v1-6/zh-CN/SuperSocket-Basic-Configuration
            /// </summary>
            /// <param name="rootConfig">根配置</param>
            /// <param name="config">服务器实例配置</param>
            /// <returns></returns>
            protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
            {
                return base.Setup(rootConfig, config);
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

        /// <summary>
        /// 客户端逻辑连接/会话
        /// <para>AppServer  承载TCP连接的服务器实例。通过AppServer实例获取客户端连接，服务器级别的操作和逻辑。</para>
        /// <para>AppSession 客户端逻辑连接，通过AppSession实例处理基于连接的操作。发送数据到客户端，接收客户端发送的数据或者关闭连接。</para>
        /// </summary>
        private class TelnetSession : AppSession<TelnetSession>
        {
            public int MacId { get; internal set; }

            /// <summary>
            /// 会话开始
            /// </summary>
            protected override void OnSessionStarted()
            {
                LogHelper.Debug("客户端接入... IP={0},Port={1},SessionID={2}", this.LocalEndPoint.Address, this.LocalEndPoint.Port, this.SessionID);
                this.Send("Welcome to GPS Server");
            }

            /// <summary>
            /// 会话关闭：关闭后执行
            /// </summary>
            /// <param name="reason"></param>
            protected override void OnSessionClosed(CloseReason reason)
            {
                LogHelper.Debug("客户端关闭... SessionID={0}", this.SessionID);
                base.OnSessionClosed(reason);
            }

            /// <summary>
            /// 未知命令请求
            /// </summary>
            /// <param name="requestInfo"></param>
            protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
            {
                LogHelper.Debug("未知请求：{0}", JsonHelper.SerializeObject(requestInfo));
                base.HandleUnknownRequest(requestInfo);
                this.Send("Unknow request");
            }

            /// <summary>
            /// 异常
            /// </summary>
            /// <param name="e"></param>
            protected override void HandleException(Exception e)
            {
                LogHelper.Error("异常输入：{0}", JsonHelper.SerializeObject(e));
                this.Send("Application error: {0}", e.Message);
            }

        }
        #endregion

    }

}
