using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI
{
    /// <summary>
    /// 返回信息基础类
    /// </summary>
    public class BaseResult
    {

        /// <summary>
        /// 默认构造
        /// </summary>
        public BaseResult() { }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_succcess">是否成功</param>
        /// <param name="_data">返回数据</param>
        /// <param name="_statuscode">状态码 成功=0; 未知错误=-1; 其他错误=HTTP状态响应码;</param>
        /// <param name="_messsage">状态消息</param>
        public BaseResult(bool _succcess = true, object _data = null, int _statuscode = 0, string _messsage = "")
        {
            this.Success = _succcess;       //是否成功
            this.Data = _data;              //返回数据
            this.StatusCode = _statuscode;  //状态码
            this.Message = _messsage;       //状态消息
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 状态消息：如果不成功，返回的错误信息
        /// </summary>
        public string Message { get; set; }

    }
}