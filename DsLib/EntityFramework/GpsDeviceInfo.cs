//------------------------------------------------------------------------------
//  ！！！ 数据库表必须有说明信息才能自动生成 ！！！
//	此代码由T4模板自动生成 
//	生成时间 2018-01-31 14:17:01 By XiaoKe
//	对此文件的更改可能会导致不正确的行为，并且如果重新生成代码，这些更改将会丢失。
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DsLib.EntityFramework
{
    /// <summary> 
    /// GPS设备信息
    /// </summary>   
    [Table("GpsDevice")]
    public class GpsDeviceInfo
    {		
		
		/// <summary>
        /// GPS设备信息ID 
        /// </summary>
		[Key]
		public Guid GpsDeviceId { set; get; } = System.Guid.NewGuid();
		
		/// <summary>
        /// GPS设备编码 
        /// </summary>
		public string GpsImei { set; get; }
		
		/// <summary>
        /// GPS设备类型 
        /// </summary>
		public string GpsType { set; get; }
		
		/// <summary>
        /// 设备接入的IP地址（包含端口） 
        /// </summary>
		public string IpAddress { set; get; }
		
		/// <summary>
        /// 接入的时间 
        /// </summary>
		public DateTime ConnectTime { set; get; } = DateTime.Parse("1900-01-01");
		
		/// <summary>
        /// 最后接收到数据的时间 
        /// </summary>
		public DateTime LastTime { set; get; } = DateTime.Parse("1900-01-01");
		
		/// <summary>
        /// 创建时间 
        /// </summary>
		public DateTime CreatTime { set; get; } = DateTime.Parse("1900-01-01");
		
		/// <summary>
        /// 更新时间 
        /// </summary>
		public DateTime UpdateTime { set; get; } = DateTime.Parse("1900-01-01");
		
		/// <summary>
        /// 显示排序 
        /// </summary>
		public int DisplayOrder { set; get; }


        /// <summary>
        /// 
        /// </summary>
        public List<GpsPositionInfo> infos { set; get; }
		
	}
	 
} 