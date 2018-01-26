using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Demo
{
    /// <summary>
    /// 委托示例
    /// </summary>
    public static class DemoDelegate
    {
        //https://www.cnblogs.com/zhangchenliang/p/4968779.html

        //------------------------------------------------------
        // Action：是无返回值的泛型委托，可带0~16个参数
        //------------------------------------------------------

        /// <summary>
        /// 定义委托：带一个参数
        /// </summary>
        public static Action<string> Act1;

        //------------------------------------------------------
        //示例A   
        //DemoDelegate.Act1 = DemoDelegate.Method1;
        //DemoDelegate.Act1("字符串参数");
        //DemoDelegate.Act1 = DemoDelegate.Method2;
        //DemoDelegate.Act1("字符串参数");

        /// <summary>  
        /// 提供给委托调用方法1
        /// </summary>  
        public static void Method1(string arg)
        { 
            Console.WriteLine("--- 提供给委托调用方法1 参数 arg=" + arg + " ---");
        }
        
        /// <summary>  
        /// 提供给委托调用方法2
        /// </summary>  
        public static void Method2(string arg)
        {
            Console.WriteLine("--- 提供给委托调用方法2 参数 arg=" + arg + " ---");
        }

        //------------------------------------------------------
        //示例B
        //DemoDelegate.Act1 = DemoDelegate.Method3;
        //DemoDelegate.Method3_Call("外部调用");
        //Method3_Call -> Method3_Delegate -> Method3

        /// <summary>
        /// 示例B--1：作为参数传入委托
        /// </summary>
        /// <param name="arg"></param>
        public static void Method3(string arg)
        {
            arg += "+1";
            Console.WriteLine(string.Format("Method3：【{0}】", arg));
        }

        /// <summary>
        /// 示例B--2：委托的实现
        /// </summary>
        /// <param name="str"></param>
        private static void Method3_Delegate(string arg)
        {
            if (Act1 != null)
            {
                arg += "+2";
                Console.WriteLine(string.Format("Method3_Delegate：[{0}]", arg));

                Act1(arg);

                //Act1 = (x) => { Console.WriteLine(string.Format("Invoke:{0}", x)); };
            }
        }

        /// <summary>
        /// 示例B--3：供外部调用的方法
        /// </summary>
        /// <param name="arg"></param>
        public static void Method3_Call(string arg)
        {
            arg += "+3";
            Console.WriteLine(string.Format("Method3_Call：（{0}）", arg));

            Method3_Delegate(arg);//调用委托
        }


        //------------------------------------------------------
        //示例C 
        //EvnetAct1 += DemoDelegate_EvnetAct1;//可以在其他类中定义并绑定
        //EvnetAct1("触发事件");              //只能在定义类中使用

        /// <summary>
        /// 声明事件委托：带一个参数
        /// </summary>
        public static event Action<string> EvnetAct1;
        /// <summary>
        /// 定义事件委托：带一个参数
        /// </summary>
        /// <param name="arg"></param>
        public static void DemoDelegate_EvnetAct1(string arg)
        {
            Console.WriteLine("--- 事件定义（DemoDelegate_EvnetAct1） 参数 arg=" + arg + " ---");
        }
        /// <summary>
        ///  提供给事件委托调用方法4
        /// </summary>
        /// <param name="arg"></param>
        public static void Method4(string arg)
        {
            Console.WriteLine("--- 提供给事件委托调用方法3 参数 arg=" + arg + " ---");
            EvnetAct1 += DemoDelegate_EvnetAct1; //可以在其他类中定义并绑定
            EvnetAct1("事件定义传入的参数");     //只能在定义类中使用
        }


        //------------------------------------------------------
        //示例E
        //Action<string> methodCall = (x) => { x += "测试"; Console.WriteLine(x); };
        //public event Action<string> BoilerEventLog;
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    BoilerEventLog += new Action<string>(methodCall);
        //    BoilerEventLog("123");
        //}

    }
}

/*


        //声明委托
        public static Action<string> OutputMessage;

        #region 调用委托
        private static void HandMessage(object Msg)
        {
            OutputMessage?.Invoke(Msg.ToString());
        }
        private static void HandMessage(object Msg, Exception ex)
        {
            OutputMessage?.Invoke(string.Format("{0} --输出：{1}", Msg.ToString(), ex.ToString()));
        }
        private static void HandMessage(string format, params object[] args)
        {
            OutputMessage?.Invoke(string.Format(format, args));
        }
        #endregion

        #region 封装Log4net
        public static void Debug(object message)
        {
            HandMessage(message);
            if (log.IsDebugEnabled)
                log.Debug(message);
        }
        public static void Debug(object message, Exception ex)
        {
            HandMessage(message, ex);
            if (log.IsDebugEnabled)
                log.Debug(message, ex);
        }
        public static void DebugFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsDebugEnabled)
                log.DebugFormat(format, args);
        }
        public static void Error(object message)
        {
            HandMessage(message);
            if (log.IsErrorEnabled)
                log.Error(message);
        }
        public static void Error(object message, Exception ex)
        {
            HandMessage(message, ex);
            if (log.IsErrorEnabled)
                log.Error(message, ex);
        }
        public static void ErrorFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsErrorEnabled)
                log.ErrorFormat(format, args);
        }
        public static void Fatal(object message)
        {
            HandMessage(message);
            if (log.IsFatalEnabled)
                log.Fatal(message);
        }
        public static void Fatal(object message, Exception ex)
        {
            HandMessage(message, ex);
            if (log.IsFatalEnabled)
                log.Fatal(message, ex);
        }
        public static void FatalFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsFatalEnabled)
                log.FatalFormat(format, args);
        }
        public static void Info(object message)
        {
            HandMessage(message);
            if (log.IsInfoEnabled)
                log.Info(message);
        }
        public static void Info(object message, Exception ex)
        {
            HandMessage(message, ex);
            if (log.IsInfoEnabled)
                log.Info(message, ex);
        }
        public static void InfoFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsInfoEnabled)
                log.InfoFormat(format, args);
        }
        public static void Warn(object message)
        {
            HandMessage(message);
            if (log.IsWarnEnabled)
                log.Warn(message);
        }
        public static void Warn(object message, Exception ex)
        {
            HandMessage(message, ex);
            if (log.IsWarnEnabled)
                log.Warn(message, ex);
        }
        public static void WarnFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsWarnEnabled)
                log.WarnFormat(format, args);
        }
        #endregion
 
     
*/
