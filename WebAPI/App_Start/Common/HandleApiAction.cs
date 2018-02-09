using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

using DsLib.Common;

namespace WebAPI
{

    /// <summary>
    /// 自定义操作筛选器
    /// </summary>
    public class HandleApiAction : ActionFilterAttribute
    {


        /// <summary>
        /// 统一对调用操作信息进行处理，自定义返回信息
        /// </summary>
        /// <param name="context">HTTP上下文对象</param>
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            //执行基类中的OnActionExecuted
            base.OnActionExecuted(context);

            BaseResult result = new BaseResult();

            if (context.ActionContext.Response != null)
            {
                // 取得由 API 返回的状态代码
                var statuscode = context.ActionContext.Response.StatusCode;
                result.StatusCode = statuscode.ToInt();
                result.Message = "[HandleApiAction]：" + statuscode.ToString();
                result.Success = false;

                if (statuscode != HttpStatusCode.OK)
                {
                    LogHelper.Debug("[HandleApiAction]：返回码(非OK)！{0} - {1}", result.StatusCode, result.Message);
                }
                else
                {
                    result.Success = true;
                }

                // 取得由 API 返回的资料 
                //if (result.success == true)
                if (context.ActionContext.Response.Content != null)
                {
                    result.Data = context.ActionContext.Response.Content.ReadAsAsync<object>().Result;
                }

                // 重新封装回传格式
                context.Response = context.Request.CreateResponse(statuscode, result);

            }
            else
            {
                //var ex = context.Exception;
                LogHelper.Debug("[HandleApiAction]：异常，响应消息为空！");
            }



        }
    }

}