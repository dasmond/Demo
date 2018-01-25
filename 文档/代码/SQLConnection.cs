using System;
using System.Configuration;

namespace DSNN.Core.Common
{
    /// <summary>
    /// 数据库链接类
    /// </summary>
    public class SQLConnection
    {        
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {           
            get 
            {
                string _connectionString = ConfigurationManager.AppSettings["SQLConn"];
                return _connectionString; 
            }
        }

    }
}
