using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            LogHelper.Debug("启动：{0}", DateTime.Now.ToFileTime()); 

            //DemoDelegate.Act1 = DemoDelegate.Method3;
            //DemoDelegate.Method3_Call("外部调用"); 

            //Action<string> act = (x) => { x += "x"; Console.Write(x); };
            //act("test");
            

            Console.ReadLine();


        }
         
    }
}
