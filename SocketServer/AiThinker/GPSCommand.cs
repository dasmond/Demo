using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DsLib.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Command;

namespace SocketServer.AiThinker
{
    //GPSCommand 命令
    //$GNRMC解析[北斗]
    //$GNRMC,00039.262,V,2236.3748,N,11350.4114,E,0.000,0.00,060180,,,N*50
    //$GPRM字符串解析[GPS]

    //GP 全球定位系统（GPS-global positioning system） 
    //BD 北斗导航卫星系统（COMPASS）  
    //GN 全球导航卫星系统（GNSS-global navigation satellite system） 
    //GL GLONASS系统
    //GA 伽利略系统
    //CC 计算机系统
    //CF 自定义信息

    //GN GGA：时间、位置、定位数据
    //GN RMC：日期，时间，位置，方向，速度数据。是最常用的一个消息
    //GN VTG：方位角与对地速度
    //BD GSA：接收机模式和卫星工作数据,包括位置和水平/竖直稀释精度等。稀释精度（Dilution of Precision）是个地理定位 术语.一个接收器可以在同一时间得到许多颗卫星定位信息，但在精密定位上，只要四颗卫星讯号即已足够了
    //BD GSV：接收机能接收到的卫星信息，包括卫星ID，海拔，仰角，方位角，信噪比（SNR）等
    //GLL：经纬度,UTC时间和定位状态
    //MSS：信噪比(SNR), 信号强度，频率，比特率
    //ZDA：时间和日期数据


    /// <summary>
    /// 数据标识：GNRMC
    /// 北斗定位信息
    /// </summary>
    public class GNRMC : CommandBase<GPSSession, StringRequestInfo>
    {        
        /// <summary>
        /// 命令处理类
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(GPSSession session, StringRequestInfo requestInfo)
        {
            var key = requestInfo.Key;
            var param = requestInfo.Parameters;
            var body = requestInfo.Body;

            //GNRMC解析[北斗]
            //$GNRMC,00039.262,V,2236.3748,N,11350.4114,E,0.000,0.00,060180,,,N*50
            //协议头,
            //【0】UTC时间（hhmmss.sss）,
            //【1】V/A=无效/有效,
            //【2】纬度,
            //【3】N/S=北/南,
            //【4】经度,
            //【5】E/W=东/西,
            //【6】对地速度(1节= 1852米/小时),
            //【7】方位角（度）
            //【8】日期（ddmmyy）
            //【9】磁偏角
            //【10】磁偏角方向
            //【11】模式指示
            //【12】校验和
            //<CR><LF>回车换行，消息结束

            var info = new GPSInfo();
            info.GPSType = "BD";
            info.GPSTime = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", param[8].Substring(4, 2), param[8].Substring(2, 2), param[8].Substring(0, 2),
                                                                      param[0].Substring(0, 2), param[0].Substring(2, 2), param[0].Substring(4, 2)).ToDateTime(DateTime.MinValue);
            info.GPSStatus = param[1];
            info.Speed = param[6].Trim() == "" ? "" : (param[6].ToDouble(0) * 1852).ToString("0.00");
            info.Latitude = param[2].Trim() == "" ? "" : (Utils.GPSConvertToDegree(param[2])).ToString("0.000000");
            info.Longitude = param[4].Trim() == "" ? "" : (Utils.GPSConvertToDegree(param[4])).ToString("0.000000");
            info.Heading = param[7];

            session.Logger.Debug(JsonHelper.SerializeObject(info));


        }

    }







}
