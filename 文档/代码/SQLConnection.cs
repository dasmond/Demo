using System;
using System.Configuration;

namespace DSNN.Core.Common
{
    /// <summary>
    /// ���ݿ�������
    /// </summary>
    public class SQLConnection
    {        
        /// <summary>
        /// ��ȡ�����ַ���
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
