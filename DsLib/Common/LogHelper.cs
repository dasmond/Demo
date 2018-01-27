using System;
using System.IO; 
using log4net;
using log4net.Config;

namespace DsLib.Common
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class LogHelper
    {

        /// <summary>
        /// 静态构造
        /// </summary>
        static LogHelper()
        {
            //读取配置文件：log4net.config
            //var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Config/log4net.config");
            var logCfg = new FileInfo(Utils.GetMapPath("~/Config/log4net.config"));
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
        
        /// <summary>
        /// ILog对象
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 定义常规应用程序中未处理的异常信息记录方式
        /// </summary>
        public static void LoadUnhandledException()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, e) =>
            {
                logger.Fatal("未处理的异常", e.ExceptionObject as Exception);
            });
        }

        /// <summary>
        /// 调试日志记录
        /// </summary>
        public static void Debug(string msg, params Object[] args)
        {
            if (args.Length > 0)
                msg = string.Format(msg, args);
            logger.Debug(msg);
        }

        /// <summary>
        /// 信息日志记录
        /// </summary>
        public static void Info(string msg, params Object[] args)
        {
            if (args.Length > 0)
                msg = string.Format(msg, args);
            logger.Info(msg);
        }

        /// <summary>
        /// 警告日志记录
        /// </summary>
        public static void Warn(string msg, params Object[] args)
        {
            if (args.Length > 0)
                msg = string.Format(msg, args);
            logger.Warn(msg);
        }

        /// <summary>
        /// 错误日志记录
        /// </summary>
        public static void Error(string msg, params Object[] args)
        {
            if (args.Length > 0)
                msg = string.Format(msg, args);
            logger.Error(msg);
        }

        /// <summary>
        /// 严重错误日志记录
        /// </summary>
        public static void Fatal(string msg, params Object[] args)
        {
            if (args.Length > 0)
                msg = string.Format(msg, args);
            logger.Error(msg);
        }

         

    }
}
