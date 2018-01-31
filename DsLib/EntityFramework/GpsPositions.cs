//------------------------------------------------------------------------------
//  ！！！ 数据库表必须有说明信息才能自动生成 ！！！
//	此代码由T4模板自动生成 
//	生成时间 2018-01-31 14:17:06 By XiaoKe
//	对此文件的更改可能会导致不正确的行为，并且如果重新生成代码，这些更改将会丢失。
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text; 

namespace DsLib.EntityFramework
{
    /// <summary>
    /// GPS位置信息
    /// </summary>
    public partial class GpsPositions
    {
        private static readonly Repository<GpsPositionInfo> db = new Repository<GpsPositionInfo>();
				
        /// <summary>
        /// 查询存在
        /// </summary>
        /// <param name="GpsPositionId">主键ID</param>
        /// <returns></returns>
        public static bool Exists(Guid GpsPositionId)
        {
            return db.Exists(GpsPositionId);
        }

        /// <summary>
        /// 查询最大值，只用于（int long double decimal）等数字类型的查询，失败返回:-1
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns></returns>
        public static string Max(string field)
        {
            return db.Max(field);
        }

        /// <summary>
        /// 查询行数，可用于查询存在，未找到字段名称返回:-1
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="keywords">关键字</param>
        /// <returns></returns>
        public static int Count(string field, string keywords)
        {
            return db.Count(field, keywords);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(GpsPositionInfo model)
        {
            return db.Add(model);
            //return _m.Id;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static int AddList(List<GpsPositionInfo> models)
        {
            return db.AddList(models);            
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Update(GpsPositionInfo model)
        {
            return db.Update(model);
        }
		
        /// <summary>
        /// 更新多个
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static int UpdateList(List<GpsPositionInfo> models)
        {
            return db.UpdateList(models);
        }

        //----------------------------------------------------

        /// <summary>
        /// 删除：删除一个实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Delete(GpsPositionInfo model)
        {
            return db.Delete(model);
        }

        /// <summary>
        /// 删除：按条件删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int Delete(Expression<Func<GpsPositionInfo, bool>> where)
        {
            return db.Delete(where);
        }

        //----------------------------------------------------

        /// <summary>
        /// 获取：按主键获取一个实体
        /// </summary>
        /// <param name="GpsPositionId">主键值</param>
        /// <returns></returns>
        public static GpsPositionInfo GetModel(Guid GpsPositionId)
        {
            return db.GetModel(GpsPositionId);
        }

        /// <summary>
        /// 获取：按条件获取一个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static GpsPositionInfo GetModel(Expression<Func<GpsPositionInfo, bool>> where)
        {
            return db.GetModel(where);
        }
		
        /// <summary>
        /// 列表：按分页获取
        /// </summary>
        /// <param name="rows">每页条目数</param>
        /// <param name="pagenum">当前页码</param>
        /// <param name="field">排序字段（分隔符',',可附带排序类型）</param>
        /// <param name="sort">排序类型（ASC,DESC）</param>
        /// <param name="records">总记录数</param>
        /// <param name="total">总页码</param>
        /// <returns></returns>
        public static List<GpsPositionInfo> GetList(int rows, int pagenum, string field, string sort, out int records, out int total)
        {
            return db.GetList(rows, pagenum, field, sort, out records, out total);
        }

        /// <summary>
        /// 列表：按分页获取
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="rows">每页条目数</param>
        /// <param name="pagenum">当前页码</param>
        /// <param name="field">排序字段（分隔符',',可附带排序类型）</param>
        /// <param name="sort">排序类型（ASC,默认：DESC）</param>
        /// <param name="records">总记录数</param>
        /// <param name="total">总页码</param>
        /// <returns></returns>
        public static List<GpsPositionInfo> GetList(Expression<Func<GpsPositionInfo, bool>> where, int rows, int pagenum, string field, string sort, out int records, out int total)
        {
            return db.GetList(where, rows, pagenum, field, sort, out records, out total);
        }



        //----------------------------------------------------

        /// <summary>
        /// 查询：IQueryable只存贮条件，不立即运行，实现延迟加载。
        /// </summary>
        /// <returns></returns>
        public static IQueryable<GpsPositionInfo> IQueryable()
        {
            return db.IQueryable();
        }

        /// <summary>
        /// 查询：按条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IQueryable<GpsPositionInfo> IQueryable(Expression<Func<GpsPositionInfo, bool>> where)
        {
            return db.IQueryable(where);
        }

        /// <summary>
        /// 列表：使用SQL语句获取列表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static List<GpsPositionInfo> GetList(string sql)
        {
            return db.GetList(sql);
        }

        /// <summary>
        /// 列表：使用SQL语句和SQL参数获取列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<GpsPositionInfo> GetList(string sql, DbParameter[] param)
        {
            return db.GetList(sql, param);
        }

        //----------------------------------------------------





    }
 
}
