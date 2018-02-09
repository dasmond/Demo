
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI
{
    public class Tools
    {

        /// <summary>
        /// 给指定的 Cookies 赋值
        /// </summary>
        /// <param name="cookKey">Cookies 名称</param>
        /// <param name="value">Cookies 值</param>
        /// <param name="domain">设置与此 Cookies 关联的域（如：“.baidu.com”）（可以使该域名下的二级域名访问）</param>
        public static void SetCookiesValue(string cookKey, string value, string domain)
        {
            HttpCookie cookie = new HttpCookie(cookKey);
            cookie.Value = value;
            cookie.HttpOnly = true;
            if (!string.IsNullOrEmpty(domain) && domain.Length > 0)
                cookie.Domain = domain;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}