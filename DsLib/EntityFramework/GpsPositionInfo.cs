//------------------------------------------------------------------------------
//  ！！！ 数据库表必须有说明信息才能自动生成 ！！！
//	此代码由T4模板自动生成 
//	生成时间 2018-01-31 14:17:01 By XiaoKe
//	对此文件的更改可能会导致不正确的行为，并且如果重新生成代码，这些更改将会丢失。
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DsLib.EntityFramework
{
    /// <summary> 
    /// GPS位置信息
    /// </summary>   
    [Table("GpsPosition")]
    public class GpsPositionInfo
    {		
		
		/// <summary>
        /// 位置信息ID 
        /// </summary>
		[Key]
		public Guid GpsPositionId { set; get; } = System.Guid.NewGuid();
		
		/// <summary>
        /// GPS设备信息ID 
        /// </summary>
		public Guid GpsDeviceId { set; get; } = System.Guid.NewGuid();
		
		/// <summary>
        /// 定位状态 （A=数据有效，V=数据无效） 
        /// </summary>
		public string GpsStatus { set; get; }
		
		/// <summary>
        /// 定位时间 
        /// </summary>
		public DateTime GpsTime { set; get; } = DateTime.Parse("1900-01-01");
		
		/// <summary>
        /// 定位类型（BD=北斗，GP=全球定位系统） 
        /// </summary>
		public string GpsType { set; get; }
		
		/// <summary>
        /// 定位卫星数 
        /// </summary>
		public int GpsSat { set; get; }
		
		/// <summary>
        /// 纬度（decimal(12,8)） 
        /// </summary>
		public string GpsLatitude { set; get; }
		
		/// <summary>
        /// 经度（decimal(12,8)） 
        /// </summary>
		public string GpsLongitude { set; get; }
		
		/// <summary>
        /// 速度 
        /// </summary>
		public string GpsSpeed { set; get; }
		
		/// <summary>
        /// 航向 
        /// </summary>
		public string GpsHeading { set; get; }
		
		/// <summary>
        /// 国家代号(MCC) 
        /// </summary>
		public string LbsMcc { set; get; }
		
		/// <summary>
        /// 移动网号码(MNC) 
        /// </summary>
		public string LbsMnc { set; get; }
		
		/// <summary>
        /// 位置区码 (LAC) 
        /// </summary>
		public string LbsLac { set; get; }
		
		/// <summary>
        /// 基站ID (Cell ID) 
        /// </summary>
		public string LbsCellId { set; get; }
		
		/// <summary>
        /// 创建时间 
        /// </summary>
		public DateTime CreatTime { set; get; } = DateTime.Parse("1900-01-01");
		
	}
	 
} 