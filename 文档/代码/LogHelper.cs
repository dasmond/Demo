using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DSNN.Core.Common
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public LogHelper()
        {
        }


        /// <summary>
        /// 写入一条信息
        /// </summary>
        /// <param name="text"></param>
        public static void Write(string text)
        {
            string logfile = WebHelper.GetMapPath("~/App_Data/Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            using (StreamWriter sw = new StreamWriter(logfile, true, Encoding.UTF8))
            {
                sw.WriteLine("---");
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
                sw.WriteLine("");
            }
        }

        /// <summary>
        /// 写入一条信息
        /// </summary>
        /// <param name="text"></param>
        public static void WriteWeixin(string text)
        {
            string logfile = WebHelper.GetMapPath("~/App_Data/Wxi_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            using (StreamWriter sw = new StreamWriter(logfile, true, Encoding.UTF8))
            {
                sw.WriteLine("---");
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
                sw.WriteLine("");
            }
        }

        /// <summary>
        /// 写入一条信息
        /// </summary>
        /// <param name="text"></param>
        public static void WriteSearch(string text)
        {
            string logfile = WebHelper.GetMapPath("~/App_Data/Sch_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            using (StreamWriter sw = new StreamWriter(logfile, true, Encoding.UTF8))
            {
                sw.WriteLine("---");
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
                sw.WriteLine("");
            }
        }

        /// <summary>
        /// 写入一条信息
        /// </summary>
        /// <param name="text"></param>
        public static void WriteJuhe(string text)
        {
            string logfile = WebHelper.GetMapPath("~/App_Data/Juh_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            using (StreamWriter sw = new StreamWriter(logfile, true, Encoding.UTF8))
            {
                sw.WriteLine("---");
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
                sw.WriteLine("");
            }
        }

    }
}
