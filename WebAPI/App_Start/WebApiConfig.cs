using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            //跨域配置
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //启用WebAPI特性路由 
            config.MapHttpAttributeRoutes();

            //路由映射
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //操作筛选器
            config.Filters.Add(new HandleApiAction());

            //异常筛选器
            config.Filters.Add(new HandleApiException());

            //清除返回格式
            config.Formatters.Clear();

            //添加JSON格式
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());

        }
    }
}
