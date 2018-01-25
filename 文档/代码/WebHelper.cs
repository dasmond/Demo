using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;

using System.Reflection;

namespace DSNN.Core.Common
{
    public class WebHelper
    {        
        /// <summary>
        /// 构造函数
        /// </summary>
        public WebHelper(){}


        //--- 页面类 -----------------------------------------------------------

        //警告消息：实体
        #region - AlertInfo  -
        /// <summary>
        /// 警告消息：实体
        /// </summary>
        public class AlertInfo
        {
            /// <summary>
            /// 是否已完成
            /// </summary>
            public bool Success { set; get; }
            /// <summary>
            /// 在头部面板显示的标题文本
            /// </summary>
            public string Title { set; get; }
            /// <summary>
            /// 显示的消息文本
            /// </summary>
            public string Msg { set; get; }
            /// <summary>
            /// 显示的图标图像。可用值有：error,question,info,warning。
            /// </summary>
            public EnumIcon Icon { set; get; }
            /// <summary>
            /// 在窗口关闭的时候触发该回调函数。（也作为参数返回，如：返回添加完成之后的新ID）
            /// </summary>
            public string Fn { set; get; }

            /// <summary>
            /// 图标图像枚举
            /// </summary>
            public enum EnumIcon
            {
                error,
                question,
                info,
                warning
            }
        }
        #endregion

        //警告消息：获取消息Json
        #region - AlertInfo GetAlertJson(bool _success, string _msg) -
        /// <summary>
        /// 警告消息：获取消息Json
        /// </summary>
        /// <param name="_success">是否已完成</param>
        /// <param name="_msg">消息内容</param>
        /// <param name="_fn">触发的回调函数（也作为参数返回，如：返回添加完成之后的新ID）</param>
        /// <returns></returns>
        public static string GetAlertJson(bool _success, string _msg, string _fn = "")
        {
            AlertInfo alert = new AlertInfo();
            alert.Success = _success;
            if (_success)
            {
                alert.Title = "提示消息";
                alert.Icon = AlertInfo.EnumIcon.info;
            }
            else
            {
                alert.Title = "错误消息";
                alert.Icon = AlertInfo.EnumIcon.error;
            }
            alert.Msg = _msg;
            alert.Fn = _fn;

            return JsonHelper.SerializeObject(alert);
        }
        #endregion

        //信息提示类
        #region - MessageOutput(string key, string scriptstr) -
        /// <summary>
        /// 信息提示类 刷新使用：location.href='xxx';
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scriptstr">JS代码或文字</param>
        public static void MessageOutput(string key, string scriptstr)
        {
            Page page = (Page)HttpContext.Current.CurrentHandler;

            key = key.ToLower();

            // 直接在页面内执行JS代码
            if (key == "")
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), key, scriptstr);// 输出在<form>的下一行
                //pg.ClientScript.RegisterStartupScript(this.GetType(), key, scriptstr);   // 输出在</form>的上一行
            }
            else
            {
                switch (key)
                {
                    case "page":// 在页面内输出已定义代码
                        break;
                    case "begin":// 在页面内输出 开始执行的模式窗口
                        break;
                    case "finish":// 在页面内输出 执行完成的模式窗口
                        break;
                    default:
                        page.ClientScript.RegisterClientScriptBlock(page.GetType(), key, scriptstr);// 输出在<form>的下一行
                        break;
                }
            }
        }
        #endregion


        // 获取分页代码
        #region - GetPager(_RecordCount, _PageSize, _PageCurrent) -
        /// <summary>
        /// 获取分页代码
        /// </summary>
        /// <param name="_page">对象：System.Web.UI.Page</param>
        /// <param name="_recordCount">记录总数</param>
        /// <param name="_pageSize">每页条目数</param>
        /// <param name="_pageCurrent">当前页码</param>
        /// <returns></returns>
        public static string GetPager(Page _page, int _recordCount, int _pageSize, int _pageCurrent)
        {
            /*
             System.Collections.Specialized.NameValueCollection nvCol = _page.Request.QueryString;
            foreach (string key in nvCol.Keys)
            {
                //键：key, 值：nvCol[key]
            }
             */

            StringBuilder strtmp = new StringBuilder();
            
            string PageUrl = _page.Request.RawUrl;
            //string PageUrl = _page.Request.Url.ToString();//
            
            if (PageUrl.IndexOf("?page=") > -1)
            {
                PageUrl = PageUrl.Replace("?page=" + _pageCurrent, "") + "?";
            }
            else if (PageUrl.IndexOf("?") < 0)
            {
                PageUrl = PageUrl + "?";
            }
            else
            {
                // 初始化链接字符串：末尾加“&”字符
                if (PageUrl.IndexOf("&page=") > -1)
                {
                    PageUrl = PageUrl.Replace("&page=" + _pageCurrent, "") + "&";
                }
                if (PageUrl.Substring(PageUrl.Length - 1, 1) != "&")
                {
                    PageUrl = PageUrl + "&";
                }
            }

            if (_pageCurrent == 0) { _pageCurrent = 1; }
            

            if (_recordCount > _pageSize)
            {

                int _numCount = 5;     // 页码数
                int _offset = 2;        // 偏移量
                int _pageCount = 0;     // 分页总数

                int _from = 1;  // 需循环页码的起始位置
                int _to = 1;    // 需循环页码的结束位置

                _pageCount = (int)Math.Ceiling((double)(_recordCount) / _pageSize);

                // “分页总数”在“页码数”之内
                if (_pageCount <= _numCount)
                {
                    _from = 1;
                    _to = _pageCount;
                }

                // “分页总数”在“页码数”之外
                if (_pageCount > _numCount)
                {
                    _from = 1;
                    _to = _numCount + 1;

                    if (_pageCurrent > _offset)
                    {
                        _from = _pageCurrent - _offset;

                        _to = _pageCurrent + _numCount - _offset;
                        if (_to > _pageCount) { _to = _pageCount; }

                        if (_to - _from < _numCount) { _from = _pageCount - _numCount; }
                    }
                }

                              

                strtmp.Append("<ul class=\"pagination pagination-sm\">");


                // 上一页
                if (_pageCurrent > 1)
                {
                    strtmp.Append("<li><a href=\"" + PageUrl + "page=" + (_pageCurrent - 1) + "\" target=\"_self\">&laquo;</a></li>");
                }
                else
                {
                    strtmp.Append("<li class=\"disabled\"><span>&laquo;</span></li>");
                }

                // 首页
                if (_pageCurrent > _offset + 1 && _from > 1)
                {
                    strtmp.Append("<li><a href=\"" + PageUrl + "page=1\" target=\"_self\">1...</a></li>");                    
                }

                // 页码循环
                for (int i = _from; i <= _to; i++)
                {
                    if (i == _pageCurrent)
                    {
                        strtmp.Append("<li class=\"active\"><span>" + i.ToString() + " <span class=\"sr-only\">(current)</span></span></li>");                        
                    }
                    else
                    {
                        strtmp.Append("<li><a href=\"" + PageUrl + "page=" + i.ToString() + "\" target=\"_self\">" + i.ToString() + "</a></li>");                        
                    }
                }

                // 尾页
                if (_pageCurrent < _pageCount - _numCount + _offset && _to < _pageCount)
                {
                    strtmp.Append("<li><a href=\"" + PageUrl + "page=" + _pageCount + "\" target=\"_self\">..." + _pageCount + "</a></li>"); 
                }

                // 下一页
                if (_pageCurrent < _pageCount)
                {
                    strtmp.Append("<li><a href=\"" + PageUrl + "page=" + (_pageCurrent + 1) + "\" target=\"_self\">&raquo;</a></li>");                    
                }
                else
                {
                    strtmp.Append("<li class=\"disabled\"><span>&raquo;</span></li>");
                }

                strtmp.Append("</ul>");
            }
            return strtmp.ToString();

        }
        #endregion

        //--- 页面类 -----------------------------------------------------------

        //设置页面不被缓存
        #region - NoCache() -
        /// <summary>
        /// 设置页面不被缓存
        /// </summary>
        public static void NoCache()
        {
            Page pg = (Page)HttpContext.Current.CurrentHandler;

            //设置服务器上不缓存结果
            pg.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            pg.Response.Buffer = true;
            pg.Response.ExpiresAbsolute = System.DateTime.Now.AddDays(-1);
            pg.Response.Expires = 0;
            pg.Response.CacheControl = "no-cache";
            pg.Response.AddHeader("Pragma", "No-Cache");


        }
        #endregion



        //--- 服务端 -----------------------------------------------------------

        //信息：返回指定的服务器变量信息
        #region - string GetServerString(string strName) -
        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
                return "";

            return HttpContext.Current.Request.ServerVariables[strName].ToString();

            //常用参数说明：
            //服务器名 SERVER_NAME
            //服务器IP LOCAL_ADDR
            //服务器端口 SERVER_PORT
            //IIS版本 SERVER_SOFTWARE
            //本文件路径 PATH_TRANSLATED
            //服务器CPU数量 NUMBER_OF_PROCESSORS
            //服务器操作系统 OS
        }
        #endregion

        //操作：建立文件夹
        #region - bool CreateDir(string name) -
        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="name">文件夹名称包括路径</param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("name"));    //创建name文件夹
            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("file")) == false)
            {
                return false;
            }
            return true;
        }
        #endregion



        //--- 客户端 -----------------------------------------------------------

        //信息：获得当前页面客户端的IP
        #region - string GetIP() -
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
                return "127.0.0.1";

            return result;
        }
        #endregion

        //信息：判断当前访问是否来自浏览器软件
        #region - bool IsBrowserGet() -
        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                    return true;
            }
            return false;
        }
        #endregion

        //信息：判断是否来自搜索引擎链接
        #region - bool IsSearchEnginesGet() -
        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                    return true;
            }
            return false;
        }
        #endregion



        //--- Url 信息 -----------------------------------------------------------
        
        //得到当前完整主机头
        #region - string GetHostCurrentFull() -
        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHostCurrentFull()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());

            return request.Url.Host;
        }
        #endregion

        //得到主机头
        #region - string GetHost() -
        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }
        #endregion

        
        //获得当前物理路径
        #region - string GetMapPath(string strPath) -
        /// <summary>
        /// 获得当前物理路径 有“\”
        /// </summary>
        /// <param name="strPath">指定的虚拟路径</param>
        /// <returns>返回与虚拟路径对应的物理路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用,或HttpContext终止的情况下
            {
                strPath = strPath.Replace("~/", "").Replace("/","\\");
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        //获得系统Url地址 无“/”
        #region- string GetUrlBase() -
        /// <summary> 
        /// 获得系统Url地址 无“/”
        /// </summary> 
        public static string GetUrlBase()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }
        #endregion

        //获得当前Url地址(包含http://***/)
        #region - string GetUrl() -
        /// <summary>
        /// 获得当前Url地址(包含http://***/)
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        #endregion

        //获得当前Url地址( 注意：方法不安全！)
        #region - string GetUrlRaw() -
        /// <summary>
        /// 注意：方法不安全！ 获得当前Url地址(不包含http://***/)
        /// </summary>
        /// <returns></returns>
        public static string GetUrlRaw()
        {
            return HttpContext.Current.Request.RawUrl;
        }
        #endregion

        //获得当前页面的名称
        #region - string GetUrlPageName() -
        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetUrlPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }
        #endregion
        
        //返回上一个页面的Url地址
        #region - string GetUrlReferrer() -
        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;
        }
        #endregion


        //返回上一个页面的Url地址
        #region - GetUrlReferrer(string _default) -
        /// <summary>
        /// 返回上一个页面的Url地址
        /// </summary>
        /// <param name="_default">默认的上一个页面</param>
        /// <returns></returns>
        public static string GetUrlReferrer(string _default)
        {
            string url = GetUrlPageName().ToLower();
            string def = _default.ToString().ToLower();

            if (url.IndexOf(def) > -1)
            {
                return GetUrlReferrer();
            }
            else
            {
                return _default;
            }
        }
        #endregion


        //--- Url 传参 -----------------------------------------------------------

        //获得所有Url参数的总个数
        #region - int GetUrlParamCount() -
        /// <summary>
        /// 返回Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetUrlParamCount()
        {
            return HttpContext.Current.Request.QueryString.Count;
        }
        #endregion

        //获得指定Url参数的值并过滤SQL非安全字符
        #region - string GetQueryString(string strName) -
        /// <summary>
        /// 获得指定Url参数的值并过滤SQL非安全字符
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            return GetQueryString(strName, true);
        }
        #endregion

        //获得指定Url参数的值
        #region - string GetQueryString(string strName) -
        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="boolCheck">是否过滤SQL非安全字符</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName, bool boolCheck)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
                return "";

            string strtmp = HttpContext.Current.Request.QueryString[strName].ToString();
            strtmp = System.Web.HttpUtility.UrlDecode(strtmp, Encoding.GetEncoding("UTF-8"));
            
            // 进行SQL安全检查
            if (boolCheck && strtmp.Length > 0 && !Utils.IsSafeSqlString(strtmp))
            {
                strtmp = Utils.FilterSqlString(strtmp);
            }

            return strtmp;
        }
        #endregion

        //获得指定Url参数的值（数字）
        #region - int GetQueryInt(strName,defValue) -
        /// <summary>
        /// 获得指定Url参数的值（数字）
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">默认值</param>
        /// <returns>Url参数的值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
                return defValue;

            return Utils.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }
        #endregion

        //获得指定Url参数的float类型值
        #region - float GetQueryFloat(string strName, float defValue) -
        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的float类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return Utils.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);           
        }
        #endregion


        //获得指定Url参数的 decimal 类型值
        #region - decimal GetQueryDecimal(string strName, decimal defValue) -
        /// <summary>
        /// 获得指定Url参数的decimal类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的decimal类型值</returns>
        public static decimal GetQueryDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContext.Current.Request.QueryString[strName], defValue);
        }
        #endregion



        //--- Post & Get ----------------------------------------------------------

        //判断当前页面是否接收到了Post请求
        #region - IsPost() -
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        #endregion

        //判断当前页面是否接收到了Get请求
        #region - IsGet() -
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }
        #endregion




        //--- From 参数 ------------------------------------------------------

        //获得所有表单参数的总个数
        #region - int GetFormParamCount() -
        /// <summary>
        /// 获得所有表单参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetFormParamCount()
        {
            return HttpContext.Current.Request.Form.Count;
        }
        #endregion

        //检查表单参数是否存在
        #region - ExistFormParam(string strName) -
        /// <summary>
        /// 检查表单参数是否存在（若存在返回 true）
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static bool ExistFormParam(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        //获得指定表单参数的值
        #region - string GetFormString(string strName) -
        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            return GetFormString(strName, false);
        }
        #endregion

        //获得指定表单参数的值
        #region - string GetFormString(string strName, bool sqlSafeCheck) -
        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
                return "";

            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContext.Current.Request.Form[strName]))
                return "unsafe string";

            return HttpContext.Current.Request.Form[strName];
        }
        #endregion

        // 获得指定表单参数的int类型值
        #region - int GetFormInt(string strName, int defValue) -
        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }
        #endregion

        // 获得指定表单参数的float类型值
        #region - float GetFormFloat(string strName, float defValue) -
        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return Utils.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
            
        }
        #endregion

        // 获得指定表单参数的decimal类型值
        #region - float GetFormDecimal(string strName, float defValue) -
        /// <summary>
        /// 获得指定表单参数的decimal类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的decimal类型值</returns>
        public static decimal GetFormDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContext.Current.Request.Form[strName], defValue);

        }
        #endregion

        // 获得指定表单参数的datetime类型值(默认值为Config.TimeEmpty)
        #region - DateTime GetFormDateTime(string strName) -
        /// <summary>
        /// 获得指定表单参数的datetime类型值(默认值为Config.TimeEmpty)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns></returns>
        public static DateTime GetFormDateTime(string strName)
        {
            return Utils.StrToDateTime(HttpContext.Current.Request.Form[strName], Config.TimeEmpty);
        }
        #endregion

        // 获取指定表单的Model
        #region - T GetFormModel<T>(NameValueCollection _source) where T : new() -
        /// <summary>
        /// 获取指定表单的Model，表单控件名称需与Model属性一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_source"></param>
        /// <returns></returns>
        public static T GetFormModel<T>(System.Collections.Specialized.NameValueCollection _source) where T : new()
        {
            T model = new T();
            int i = 0;

            //获取<T>的所有属性
            PropertyInfo[] ps = typeof(T).GetProperties();
            foreach (var p in ps)
            {
                string pname = p.Name.ToString();
                if (_source[p.Name] != null)
                {
                    try
                    {
                        p.SetValue(model, Convert.ChangeType(_source[p.Name], p.PropertyType), null); 
                        i++;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return model;
        }
        #endregion



        //--- cookie 操作 ------------------------------------------------------------------------------------------

        //写cookie值
        #region - WriteCookie(string strName, string strValue) -
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        //写cookie值
        #region - WriteCookie(string strName, string key, string strValue) -
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        //写cookie值
        #region - WriteCookie(string strName, int expires) -
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        //写cookie值
        #region - WriteCookie(string strName, string strValue, int expires) -
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        //读cookie值
        #region - GetCookie(string strName) -
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();

            return "";
        }
        #endregion

        //读cookie值
        #region - GetCookie(string strName, string key) -
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return HttpContext.Current.Request.Cookies[strName][key].ToString();

            return "";
        }
        #endregion


        #region 已注释
        /*

        //--- XML ------------------------------------------------------------------------------------------------

        //--- 加载远程Web页面
        #region - GetHtml(string _url) -
        /// <summary>
        /// 加载远程Web页面
        /// </summary>
        /// <param name="_url">连接</param>
        /// <returns></returns>
        public static string GetHtml(string _url)
        {
            try
            {
                // 设置打开页面
                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(_url) as System.Net.HttpWebRequest;
                request.Method = "GET";
                request.KeepAlive = false;
                request.Timeout = 5000;

                // 接收返回页面
                System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                System.IO.Stream stream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                String strhtml = reader.ReadToEnd();

                response.Close();
                stream.Close();
                reader.Close();

                return strhtml;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                return "";
            }
        }
        #endregion
        
        //--- 加载远程XML文档
        #region - GetXmlByUrl(string _url) -
        /// <summary>        
        /// 加载远程XML文档
        /// </summary>        
        /// <param name="URL"></param>        
        /// <returns></returns>        
        public static XmlDocument GetXmlByUrl(string _url)
        {
            try
            { 
                //使用rssURL的值建立了一个WebRequest项            
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(_url);
                myRequest.Method = "GET";

                //WebRequest请求的响应将会被放到一个WebResponse对象myResponse里,然后这个WebResponse对象被用来建立一个流来取出XML的值
                using (System.Net.WebResponse myResponse =myRequest.GetResponse())
                {
                    System.IO.Stream stream = myResponse.GetResponseStream();

                    //使用一个XmlDocument对象rssDoc来存储流中的XML内容。XmlDocument对象用来调入XML的内容
                    XmlDocument doc = new XmlDocument();
                    doc.Load(stream);

                    return doc;
                }

            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }

            return null;

            //将Xml的存储到服务器
            //string savepath = WebRequest.GetMapPath("~" + FilesLCallXml + "a.xml");
            //System.IO.StreamReader rd = new System.IO.StreamReader(stream, System.Text.Encoding.Default);
            //string xml = rd.ReadToEnd();
            //System.IO.File.WriteAllText(savepath, xml);//保存到本地
        }
        #endregion

        //--- 获取XML文档中特定标签的某个属性值
        #region - GetXmlTagValue(_xmldoc,_tagname,_atbname) -
        /// <summary>
        /// 获取XML文档中特定标签的某个属性值
        /// </summary>
        /// <param name="_xmldoc">XML文档</param>
        /// <param name="_tagname">标签名称</param>
        /// <param name="_atbname">属性名称</param>
        /// <returns></returns>
        public static string GetXmlTayValue(XmlDocument _xmldoc, string _tagname, string _atbname)
        {
            string revalue = string.Empty;
            try
            {
                //XmlDocument doc = new XmlDocument();
                //doc.Load(_xmlpath);
                //XmlDocument doc = GetXmlByUrl(_xmlpath);

                XmlNodeList node = _xmldoc.GetElementsByTagName(_tagname);

                foreach (XmlElement n in node)
                {
                    revalue = n.Attributes[_atbname].Value;
                }

            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }

            return revalue;
        }
        #endregion

        
        //--- JSON -----------------------------------------------------------------------------------------------

        //将数据表转换成JSON类型串
        #region - StringBuilder DataTableToJSON(System.Data.DataTable dt) -
        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">要转换的数据表</param>
        /// <returns></returns>
        public static StringBuilder DataTableToJSON(System.Data.DataTable dt)
        {
            return DataTableToJSON(dt, true);
        }
        #endregion

        //将数据表转换成JSON类型串
        #region - StringBuilder DataTableToJSON(System.Data.DataTable dt, bool dt_dispose) -
        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">要转换的数据表</param>
        /// <param name="dispose">数据表转换结束后是否dispose掉</param>
        /// <returns></returns>
        private static StringBuilder DataTableToJSON(System.Data.DataTable dt, bool dt_dispose)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            //数据表字段名和类型数组
            string[] dt_field = new string[dt.Columns.Count];
            int i = 0;
            string formatStr = "{{";
            string fieldtype = "";
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                dt_field[i] = dc.Caption.ToLower().Trim();
                formatStr += "\"" + dc.Caption.ToLower().Trim() + "\":";
                fieldtype = dc.DataType.ToString().Trim().ToLower();
                if (fieldtype.IndexOf("int") > 0 || fieldtype.IndexOf("deci") > 0 ||
                    fieldtype.IndexOf("floa") > 0 || fieldtype.IndexOf("doub") > 0 ||
                    fieldtype.IndexOf("bool") > 0)
                {
                    formatStr += "{" + i + "}";
                }
                else
                {
                    formatStr += "\"{" + i + "}\"";
                }
                formatStr += ",";
                i++;
            }

            if (formatStr.EndsWith(","))
                formatStr = formatStr.Substring(0, formatStr.Length - 1);//去掉尾部","号

            formatStr += "}},";

            i = 0;
            object[] objectArray = new object[dt_field.Length];
            foreach (System.Data.DataRow dr in dt.Rows)
            {

                foreach (string fieldname in dt_field)
                {   //对 \ , ' 符号进行转换 
                    objectArray[i] = dr[dt_field[i]].ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'");
                    switch (objectArray[i].ToString())
                    {
                        case "True":
                            {
                                objectArray[i] = "true"; break;
                            }
                        case "False":
                            {
                                objectArray[i] = "false"; break;
                            }
                        default: break;
                    }
                    i++;
                }
                i = 0;
                stringBuilder.Append(string.Format(formatStr, objectArray));
            }
            if (stringBuilder.ToString().EndsWith(","))
                stringBuilder.Remove(stringBuilder.Length - 1, 1);//去掉尾部","号

            if (dt_dispose)
                dt.Dispose();

            stringBuilder.Append("]");
            return stringBuilder;
        }
        #endregion

        */
        #endregion


    }
}
