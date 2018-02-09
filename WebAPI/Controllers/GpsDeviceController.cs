//------------------------------------------------------------------------------
//  ！！！ 数据库表必须有说明信息才能自动生成 ！！！
//	此代码由T4模板自动生成 
//	生成时间 2018-02-07 09:42:56 By XiaoKe
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

namespace WebAPI.Controllers
{


    /// <summary>
    /// GPS设备信息
    /// </summary>
    [RoutePrefix("api/GpsDevice")]
    public partial class GpsDeviceController : ApiController
    {


        //GET: api/GpsDevice/GetMaxValue?field={field}
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
            var _value = GpsDevices.Max(field);
            if (_value.ToInt(-1) == -1)
            {
                return BadRequest("最大值查询[Max]查询失败！");
            }
            return Ok(_value);
        }
        #endregion
        
        //GET: api/GpsDevice/Exists
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
            var _value = GpsDevices.Count(field, keywords);
            if (_value == -1)
                return BadRequest("行数查询[Count]未找到对象！");

            return Ok(_value);
        }
        #endregion
        
        //GET: api/GpsDevice/GetModel?GpsDeviceId={GpsDeviceId}
        //获取实体对象
        #region - GetModel(Guid GpsDeviceId) -
        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="GpsDeviceId">主键：GpsDeviceId</param>
        /// <returns></returns>
        [Route("GetModel")]
        [HttpGet]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult GetModel(Guid GpsDeviceId)
        {
            var _value = GpsDevices.GetModel(GpsDeviceId);
            return Ok(_value);

            //Expression<Func<GpsDeviceInfo, bool>> where = null;
            //where = w => w.gpsdeviceid == GpsDeviceId;
            //var _value = GpsDevices.GetModel(where);
        }
        #endregion
        
        //GET: api/GpsDevice/GetList?sql={sql}
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
            
            List<GpsDeviceInfo> ml = GpsDevices.GetList(rows, pagenum, field, sort, out records, out total);

            //附带查询条件
            //Expression<Func<GpsDeviceInfo, bool>> where = null;
            //where = w => w.gpsdeviceid == 107;
            //List<GpsDeviceInfo> ml = GpsDevices.GetList(where, rows, pagenum, field, sort, out records, out total);

            var _value = new {
                pagenum = pagenum, //当前页码
                records = records, //总记录数
                total = total,     //总页码
                list = ml          //数据列表
            };

            return Ok(_value);
            
            //Expression<Func<GpsDeviceInfo, bool>> where = null;
            //where = w => w.gpsdeviceid == GpsDeviceId;
            //var _value = GpsDevices.GetModel(where);


        }
        #endregion
        


        //POST: api/GpsDevice/Add
        //增加一条数据
        #region - Add(GpsDeviceInfo model) -
        /// <summary>
        /// 增加一条数据
        /// </summary>
        [Route("Add")]
        [HttpPost]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult Add(GpsDeviceInfo model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (GpsDevices.Add(model) > 0)
                return Ok();
            else
                return BadRequest("添加失败！");

            //return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }
        #endregion


        //POST: api/GpsDevice/AddList
        //增加多条数据
        #region - AddList(List<GpsDeviceInfo> models) -
        /// <summary>
        /// 增加多条数据
        /// </summary>
        [Route("AddList")]
        [HttpPost]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult AddList(List<GpsDeviceInfo> models)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (GpsDevices.AddList(models) > 0)
                return Ok();
            else
                return BadRequest("添加失败！"); 

            //return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }
        #endregion

        
        //PUT: api/GpsDevice/Update
        //修改一条数据
        #region - Update(GpsDeviceInfo model) -
        /// <summary>
        /// 修改一条数据
        /// </summary>
        [Route("Update")]
        [HttpPut]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult Update(GpsDeviceInfo model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!GpsDevices.Exists(model.GpsDeviceId))
                return BadRequest("未找到标识为[" + model.GpsDeviceId + "]信息！");

            if (GpsDevices.Update(model) > 0)
                return Ok();
            else
                return BadRequest("修改失败！");

            //return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }
        #endregion


        //PUT: api/GpsDevice/UpdateList
        //修改多条数据
        #region - UpdateList(List<GpsDeviceInfo> models) -
        /// <summary>
        /// 修改多条数据
        /// </summary>
        /// <param name="models">实体列</param>
        [Route("UpdateList")]
        [HttpPut]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult UpdateList(List<GpsDeviceInfo> models)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (GpsDeviceInfo model in models)
            {
                if (!GpsDevices.Exists(model.GpsDeviceId))
                    return BadRequest("未找到标识为[" + model.GpsDeviceId + "]信息！");
            }

            if (GpsDevices.UpdateList(models) > 0)
                return Ok();
            else
                return BadRequest("修改失败！");

            //return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }
        #endregion


        //DELETE: api/GpsDevice/Delete
        //删除一条数据
        #region - Delete(Guid aid) -
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="GpsDeviceId">ID</param>
        [Route("Delete")]
        [HttpDelete]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult Delete(Guid GpsDeviceId)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            GpsDeviceInfo model = GpsDevices.GetModel(GpsDeviceId);
            if (model == null)
                return BadRequest("未找到标识为[" + GpsDeviceId + "]信息！");
            
            if (GpsDevices.Delete(model) > 0)
                return Ok();
            else
                return BadRequest("删除失败！");
        }
        #endregion


        //DELETE: api/GpsDevice/DeleteList
        //删除多条数据
        #region - DeleteList(List<Guid> GpsDeviceIdList) -
        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="GpsDeviceIdList">ID列</param>
        /// <returns></returns>
        [Route("DeleteList")]
        [HttpDelete]
        [ResponseType(typeof(BaseResult))]
        public IHttpActionResult DeleteList(List<Guid> GpsDeviceIdList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (GpsDeviceIdList.Count == 0)
                return BadRequest("参数错误：空！");

            Expression<Func<GpsDeviceInfo, bool>> where = null;
            where = w => GpsDeviceIdList.Contains(w.GpsDeviceId);

            if (GpsDevices.Delete(where) > 0)
                return Ok();
            else
                return BadRequest("删除失败！");             
        }
        #endregion

         


    }

 
}
