//------------------------------------------------------------------------------
//  ！！！ 数据库表必须有说明信息才能自动生成 ！！！
//	此代码由T4模板自动生成 
//	生成时间 2018-01-31 17:02:18 By XiaoKe
//	对此文件的更改可能会导致不正确的行为，并且如果重新生成代码，这些更改将会丢失。
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;

using DsLib.Common;
using DsLib.EntityFramework;

namespace DJW.WebAPI.Controllers
{


    /// <summary>
    /// GPS位置信息
    /// </summary>
    [RoutePrefix("api/GpsPosition")]
    public partial class GpsPositionController : ApiController
    {


        //GET: api/Attempt/GetMaxValue?field={field}
        //获取字段数据中的最大值 
        #region - Max(string field) -
        /// <summary>
        /// 获取字段数据中的最大值
        /// </summary>
        /// <param name="field">目标字段名称</param>
        /// <returns></returns>
        [Route("Max")]
        [HttpGet]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult Max(string field)
        {
            var _value = Attempts.Max(field);
            if (_value.ToInt(-1) == -1)
            {
                return BadRequest("最大值查询[Max]查询失败！");
            }
            return Ok(_value);
        }
        #endregion
        
        //GET: api/GpsPosition/Exists
        //获取字段中包含关键字的行数
        #region - Count(string field, string keywords) -
        /// <summary> 
        /// 获取字段中包含关键字的行数
        /// -- [field],[keywords]任意留空则查询全部
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="keywords">关键字</param>
        /// <returns></returns>
        [Route("Count")]
        [HttpGet]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult Count(string field, string keywords)
        {
            var _value = Attempts.Count(field, keywords);
            if (_value == -1)
                return BadRequest("行数查询[Count]未找到对象！");

            return Ok(_value);
        }
        #endregion
        
        //GET: api/GpsPosition/GetModel?GpsPositionId={GpsPositionId}
        //获取实体对象
        #region - GetModel(Guid GpsPositionId) -
        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="GpsPositionId">主键：GpsPositionId</param>
        /// <returns></returns>
        [Route("GetModel")]
        [HttpGet]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult GetModel(Guid GpsPositionId)
        {
            var _value = GpsPositions.GetModel(GpsPositionId);
            return Ok(_value);

            //Expression<Func<GpsPositionInfo, bool>> where = null;
            //where = w => w.gpspositionid == GpsPositionId;
            //var _value = GpsPositions.GetModel(where);
        }
        #endregion
        
        //GET: api/GpsPosition/GetList?sql={sql}
        //获取列表
        #region - GetList(int rows, int pagenum, string field, string sort) -
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="rows">每页条目数</param>
        /// <param name="pagenum">当前页码</param>
        /// <param name="field">排序字段（分隔符',',可附带排序类型）</param>
        /// <param name="sort">排序类型（ASC,默认：DESC）</param> 
        /// <returns></returns>
        [Route("GetList")]
        [HttpGet]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult GetList(int rows, int pagenum, string field, string sort)
        {
            int records = 0; //总记录数
            int total = 0;   //总页码
            
            List<GpsPositionInfo> ml = GpsPositions.GetList(rows, pagenum, field, sort, out records, out total);

            //附带查询条件
            //Expression<Func<GpsPositionInfo, bool>> where = null;
            //where = w => w.gpspositionid == 107;
            //List<GpsPositionInfo> ml = GpsPositions.GetList(where, rows, pagenum, field, sort, out records, out total);

            var _value = new {
                pagenum = pagenum, //当前页码
                records = records, //总记录数
                total = total,     //总页码
                list = ml          //数据列表
            };

            return Ok(_value);
            
            //Expression<Func<GpsPositionInfo, bool>> where = null;
            //where = w => w.gpspositionid == GpsPositionId;
            //var _value = GpsPositions.GetModel(where);


        }
        #endregion
        


        //POST: api/GpsPosition/Add
        //增加一条数据
        #region - Add(GpsPositionInfo model) -
        /// <summary>
        /// 增加一条数据
        /// </summary>
        [Route("Add")]
        [HttpPost]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult Add(GpsPositionInfo model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (GpsPositions.Add(model) > 0)
                return Ok();
            else
                return BadRequest("添加失败！");

            //return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }
        #endregion


        //POST: api/GpsPosition/AddList
        //增加多条数据
        #region - AddList(List<GpsPositionInfo> models) -
        /// <summary>
        /// 增加多条数据
        /// </summary>
        [Route("AddList")]
        [HttpPost]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult AddList(List<GpsPositionInfo> models)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (GpsPositions.AddList(models) > 0)
                return Ok();
            else
                return BadRequest("添加失败！"); 

            //return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }
        #endregion

        
        //PUT: api/GpsPosition/Update
        //修改一条数据
        #region - Update(GpsPositionInfo model) -
        /// <summary>
        /// 修改一条数据
        /// </summary>
        [Route("Update")]
        [HttpPut]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult Update(GpsPositionInfo model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!GpsPositions.Exists(model.GpsPositionId))
                return BadRequest("未找到标识为[" + model.GpsPositionId + "]信息！");

            if (GpsPositions.Update(model) > 0)
                return Ok();
            else
                return BadRequest("修改失败！");

            //return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }
        #endregion


        //PUT: api/GpsPosition/UpdateList
        //修改多条数据
        #region - UpdateList(List<GpsPositionInfo> models) -
        /// <summary>
        /// 修改多条数据
        /// </summary>
        /// <param name="models">实体列</param>
        [Route("UpdateList")]
        [HttpPut]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult UpdateList(List<GpsPositionInfo> models)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (GpsPositionInfo model in models)
            {
                if (!GpsPositions.Exists(model.GpsPositionId))
                    return BadRequest("未找到标识为[" + model.GpsPositionId + "]信息！");
            }

            if (GpsPositions.UpdateList(models) > 0)
                return Ok();
            else
                return BadRequest("修改失败！");

            //return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }
        #endregion


        //DELETE: api/GpsPosition/Delete
        //删除一条数据
        #region - Delete(Guid aid) -
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="GpsPositionId">ID</param>
        [Route("Delete")]
        [HttpDelete]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult Delete(Guid GpsPositionId)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            GpsPositionInfo model = GpsPositions.GetModel(GpsPositionId);
            if (model == null)
                return BadRequest("未找到标识为[" + GpsPositionId + "]信息！");
            
            if (GpsPositions.Delete(model) > 0)
                return Ok();
            else
                return BadRequest("删除失败！");
        }
        #endregion


        //DELETE: api/GpsPosition/DeleteList
        //删除多条数据
        #region - DeleteList(List<Guid> GpsPositionIdList) -
        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="GpsPositionIdList">ID列</param>
        /// <returns></returns>
        [Route("DeleteList")]
        [HttpDelete]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult DeleteList(List<Guid> GpsPositionIdList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (GpsPositionIdList.Count == 0)
                return BadRequest("参数错误：空！");

            Expression<Func<GpsPositionInfo, bool>> where = null;
            where = w => GpsPositionIdList.Contains(w.GpsPositionId);

            if (GpsPositions.Delete(where) > 0)
                return Ok();
            else
                return BadRequest("删除失败！");             
        }
        #endregion

         


    }

 
}
