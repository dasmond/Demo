using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.AiThinker
{
    public class GPSInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GPSInfo()
        {
        }

        /// <summary>
        /// GPS类型：BD（北斗导航卫星系统）GP（全球定位系统） 
        /// </summary>
        public string GPSType
        {
            get; set;
        }

        /// <summary>
        /// GPS时间
        /// </summary>
        public DateTime GPSTime
        {
            get; set;
        }

        /// <summary>
        /// GPS状态 A=数据有效；V=数据无效
        /// </summary>
        public string GPSStatus
        {
            get; set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude
        {
            get; set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude
        {
            get; set;
        }

        /// <summary>
        /// 速度
        /// </summary>
        public string Speed
        {
            get; set;
        }
                
        /// <summary>
        /// 航向
        /// </summary>
        public string Heading
        {
            get; set;
        }


    }
}
