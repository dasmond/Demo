using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;
using DsLib.Common;

namespace WebAPI
{
    /// <summary>
    /// 自定义异常筛选器
    /// </summary>
    public class HandleApiException : ExceptionFilterAttribute
    {

        /// <summary>
        /// 统一对调用异常信息进行处理，返回自定义的异常信息
        /// </summary>
        /// <param name="context">HTTP上下文对象</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            //返回信息实体
            BaseResult result = new BaseResult();
            result.Success = false;
            result.Data = null;
            result.StatusCode = -1;
            result.Message = context.Exception.Message;

            var ex = new HttpResponseException(HttpStatusCode.InternalServerError);

            //自定义异常的处理
            if (context.Exception is NotImplementedException)
            {
                result.StatusCode = (int)HttpStatusCode.NotImplemented;
                ex = new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented)
                {
                    //封装处理异常信息，返回指定JSON对象
                    Content = new StringContent(JsonHelper.SerializeObject(result), Encoding.UTF8, "application/json"),
                    ReasonPhrase = "NotImplementedException"
                });
            }
            else if (context.Exception is TimeoutException)
            {
                result.StatusCode = (int)HttpStatusCode.RequestTimeout;
                ex = new HttpResponseException(new HttpResponseMessage(HttpStatusCode.RequestTimeout)
                {
                    //封装处理异常信息，返回指定JSON对象
                    Content = new StringContent(JsonHelper.SerializeObject(result), Encoding.UTF8, "application/json"),
                    ReasonPhrase = "TimeoutException"
                });

            }
            else
            {
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                ex = new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    //封装处理异常信息，返回指定JSON对象
                    Content = new StringContent(JsonHelper.SerializeObject(result), Encoding.UTF8, "application/json"),
                    ReasonPhrase = "Exception"
                });
            }

            //日志记录  LogHelper.Debug("--类型：{0} --信息：{1} --堆栈：{2}", ex.GetType().ToString(), ex.Message, ex.StackTrace);
            LogHelper.Debug("[HandleApiException]：异常！- 状态码：{0} - 状态信息：{1} - 堆栈：{2}", result.StatusCode, result.Message, context.Exception.StackTrace);

            // 抛出异常 截断错误输出
            throw ex;

            // 执行基类中的 OnException
            //base.OnException(context);


        }
    }
}