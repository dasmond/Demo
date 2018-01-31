

//------------------------------------------------------------------------------
//	此代码由T4模板自动生成
//	生成时间 2018-01-31 14:16:56 By XiaoKe
//	对此文件的更改可能会导致不正确的行为，并且如果重新生成代码，这些更改将会丢失。
//------------------------------------------------------------------------------
using System;
using System.Data.Entity;

namespace DsLib.EntityFramework
{

    /// <summary>
    /// 数据库上下文：应用程序数据库
    /// </summary>
    public class AppDbContext : DbContext
    {
	
        /// <summary>
        /// 数据库连接和初始化
        /// (App.config 或 Web.config)中配置连接字符串“AppDbContext”
        /// </summary>
        public AppDbContext() : base("name=AppDbContext")
        {
            //设定数据库初始化方式
            //Database.SetInitializer<AppContext>(new DropCreateDatabaseIfModelChanges<AppContext>());
        }
		
        //--------------------------------------------------------------------------------------------

		 
        /// <summary> 
        /// 数据实体：GPS设备信息
        /// </summary>
        public virtual DbSet<GpsDeviceInfo> GpsDeviceInfos { get; set; }
		
		 
        /// <summary> 
        /// 数据实体：GPS位置信息
        /// </summary>
        public virtual DbSet<GpsPositionInfo> GpsPositionInfos { get; set; }
		
		
        //--------------------------------------------------------------------------------------------


    }

}
