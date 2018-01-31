using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;
using SocketServer.AiThinker;
using SuperSocket.SocketEngine;
using SuperSocket.SocketBase;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(System.Guid.NewGuid());

            Console.WriteLine("服务启动...");
            
            var bootstrap = BootstrapFactory.CreateBootstrap();
            
            if (!bootstrap.Initialize())
            {
                Console.WriteLine("服务初始化失败，请检查配置文件！");
                Console.ReadKey();
                return;
            }

            //启动服务
            var result = bootstrap.Start();

            Console.WriteLine("启动结果: {0}!", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("启动失败！");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("服务启动成功, 按‘q’键停止服务...");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //停止服务
            bootstrap.Stop();

            Console.WriteLine("服务已停止：{0}", DateTime.Now.ToFileTime());
            Console.ReadKey();


        }

         



    }
}
