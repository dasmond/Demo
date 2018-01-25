using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace DSNN.Core.Common
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utils
    {

        //获取距离描述
        #region - GetDistance(decimal _fLng, decimal _fLat, decimal _tLng, decimal _tLat) -
        /// <summary>
        /// 获取距离描述
        /// </summary>
        /// <param name="_fLng">起点坐标</param>
        /// <param name="_fLat">起点坐标</param>
        /// <param name="_tLng">终点坐标</param>
        /// <param name="_tLat">终点坐标</param>
        /// <returns></returns>
        public static string GetDistance(decimal _fLng, decimal _fLat, decimal _tLng, decimal _tLat)
        {
            if (_fLng > 0 && _fLat > 0 && _tLng > 0 && _tLat > 0)
            {
                double jd = 102834.74258026089786013677476285;//(米/每经度)
                double wd = 111712.69150641055729984301412873;//(米/每纬度)
                double aX = (double)_fLng;
                double aY = (double)_fLat;
                double bX = (double)_tLng;
                double bY = (double)_tLat;

                double x = Math.Abs((aX - bX) * jd);
                double y = Math.Abs((aY - bY) * wd);

                double d = Math.Sqrt((x * x + y * y));

                string _dw = "米";
                if (d > 1000)
                {
                    d = (d / 1000);
                    _dw = "公里";
                }

                return d.ToString("0.0") + " " + _dw;
            }

            return "-1";
        }
        #endregion

        //图片转二进制
        #region - ImageToBytes(*) -
        /// <summary>
        /// 图片对象转二进制
        /// </summary>
        /// <param name="_img"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image _img)
        {
            ImageFormat format = _img.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                _img.Save(ms, format);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// 图片文件转二进制
        /// </summary>
        /// <param name="_imgpath"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(string _imgpath)
        {
            //根据图片文件的路径使用文件流打开，并保存为byte[]   
            FileStream fs = new FileStream(_imgpath, FileMode.Open);//可以是其他重载方法   
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return byData;
        }
        #endregion

        //GB2312转Unicode（针对部分乱码问题）
        #region - GB2312ToUnicode(string _str) -
        /// <summary>
        /// GB2312转Unicode（针对部分乱码问题）
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string GB2312ToUnicode(string _str)
        {
            var en = System.Text.Encoding.GetEncoding("GB2312");
            var bsbyte = en.GetBytes(_str);
            return System.Text.Encoding.Unicode.GetString(bsbyte);
        }
        #endregion

        //--- 类型转换 ------------------------------------------------------------------------------------------

        //将对象转换为bool型
        #region - bool StrToBool(object expression, bool defValue) -
        /// <summary>
        /// 将对象转换为bool型
        /// </summary>
        /// <param name="expression">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }
        #endregion

        //string型转换为bool型
        #region - bool StrToBool(string strValue, bool defValue) -
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string strValue, bool defValue)
        {
            if (strValue != null)
            {
                if (string.Compare(strValue, "true", true) == 0)
                    return true;
                else if (string.Compare(strValue, "false", true) == 0)
                    return false;
            }
            return defValue;
        }
        #endregion
        
        //将对象转换为Int32类型，转换失败返回0
        #region - int ObjectToInt(object expression) -
        /// <summary>
        /// 将对象转换为Int32类型，转换失败返回0
        /// </summary>
        /// <param name="expression">对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression)
        {
            return ObjectToInt(expression, 0);
        }
        #endregion

        //将对象转换为Int32类型
        #region - int ObjectToInt(object expression, int defValue) -
        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }
        #endregion
        
        //string型转换为Int32类型,转换失败返回0
        #region - int StrToInt(string str) -
        /// <summary>
        /// string型转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str)
        {
            return StrToInt(str, 0);
        }
        #endregion

        //string型转换为Int32类型
        #region - int StrToInt(string str, int defValue) -
        /// <summary>
        /// string型转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 11 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(str, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(str, defValue));
        }
        #endregion
        
        //将对象转换为float型,转换失败返回0
        #region - float ObjectToFloat(object strValue) -
        /// <summary>
        /// 将对象转换为float型,转换失败返回0
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue)
        {
            return ObjectToFloat(strValue.ToString(), 0);
        }
        #endregion

        //将对象转换为float型
        #region - float ObjectToFloat(object expression, float defValue) -
        /// <summary>
        /// 将对象转换为float型
        /// </summary>
        /// <param name="expression">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的float类型结果</returns>
        public static float ObjectToFloat(object expression, float defValue)
        {
            if ((expression == null))
                return defValue;

            return StrToFloat(expression.ToString(), defValue);
        }
        #endregion

        //string型转换为float型,转换失败返回0
        #region - float StrToFloat(string strValue) -
        /// <summary>
        /// string型转换为float型,转换失败返回0
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue)
        {
            if ((strValue == null))
                return 0;

            return StrToFloat(strValue.ToString(), 0);
        }
        #endregion

        //string型转换为float型
        #region - float StrToFloat(string strValue, float defValue) -
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
                return defValue;

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(strValue, out intValue);
            }
            return intValue;
        }
        #endregion
        
        //string型转换为decimal型,转换失败返回0
        #region - float StrToDecimal(string strValue) -
        /// <summary>
        /// string型转换为decimal型,转换失败返回0
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(string strValue)
        {
            if ((strValue == null))
                return 0;

            return StrToDecimal(strValue.ToString(), 0);
        }
        #endregion

        //string型转换为decimal型
        #region - decimal StrToDecimal(string strValue, float defValue) -
        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(string strValue, decimal defValue)
        {
            if ((strValue == null))
                return defValue;

            decimal intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    decimal.TryParse(strValue, out intValue);
            }
            return intValue;
        }
        #endregion
        
        //string型转换为日期时间类型
        #region - DateTime StrToDateTime(string str, DateTime defValue) -
        /// <summary>
        /// string型转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }
        #endregion

        //string型转换为日期时间类型，默认返回（DateTime.Now）
        #region - DateTime StrToDateTime(string str) -
        /// <summary>
        /// /string型转换为日期时间类型，默认返回（DateTime.Now）
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }
        #endregion

        //将对象转换为日期时间类型
        #region - DateTime ObjectToDateTime(object obj) -
        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }
        #endregion

        //将对象转换为日期时间类型，默认返回（DateTime.Now）
        #region - DateTime ObjectToDateTime(object obj, DateTime defValue) -
        /// <summary>
        /// 将对象转换为日期时间类型，默认返回（DateTime.Now）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }
        #endregion

        //Unix时间戳转换为DateTime
        #region - TimestampToDateTime(string _timestamp) -
        /// <summary>
        /// Unix时间戳转换为DateTime
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(string _timestamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(_timestamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion

        //DateTime转换为Unix时间戳
        #region - DateTimeToTimestamp(DateTime _datetime) -
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name=”time”></param>
        /// <returns></returns>
        public static int DateTimeToTimestamp(DateTime _datetime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(_datetime - startTime).TotalSeconds;
        }
        #endregion

        //将Base64字符串转换为普通字符串
        #region - Base64ToStr(string strb) -
        /// <summary>
        /// 将Base64字符串转换为普通字符串，默认返回("")
        /// </summary>
        /// <param name="strb"></param>
        /// <returns></returns>
        public static string Base64ToStr(string strb)
        {
            if (!string.IsNullOrEmpty(strb))
            {
                if (IsBase64String(strb))
                {
                    byte[] b = Convert.FromBase64String(strb);
                    return System.Text.Encoding.UTF8.GetString(b);
                }
                else
                {
                    return "";
                }
            }
            return "";
        }
        #endregion


        //--- 类型判断 ------------------------------------------------------------------------------------------

        //判断字符串是否为Int32类型的数字
        #region - bool IsNumeric(string str) -
        /// <summary>
        /// 判断字符串是否为Int32类型的数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            if (str != null)
            {
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    //if ((str.Length < 10) || (str.Length == 11 && str[0] == '1') || (str.Length == 12 && str[0] == '-' && str[1] == '1'))
                    return true;
                }
            }
            return false;
        }
        #endregion

        //判断字符串是否为Double类型
        #region - bool IsDouble(string str) -
        /// <summary>
        /// 判断字符串是否为Double类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(string str)
        {
            if (str != null)
                return Regex.IsMatch(str, @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }
        #endregion

        //判断字符串数组(strNumber)中的数据是不是都为数值型
        #region - bool IsNumericArray(string[] strNumber) -
        /// <summary>
        /// 判断字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
                return false;

            if (strNumber.Length < 1)
                return false;

            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                    return false;
            }
            return true;
        }
        #endregion

        //判断字符串是否为base64字符串
        #region - bool IsBase64String(string str) -
        /// <summary>
        /// 判断字符串是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        #endregion


        //--- 字符检测 ------------------------------------------------------------------------------------------

        //检测字符串中是否带有危险的Sql字符
        #region - bool IsSafeSql(string str) -
        /// <summary>
        /// 检测字符串中是否带有危险的Sql字符,有危险字符返回True
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        #endregion

        //过滤字符串中包含的危险Sql字符
        #region - string FilterSqlString(string str) -
        /// <summary>
        /// 过滤字符串中包含的危险Sql字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>过滤后的字串</returns>
        public static string FilterSqlString(string str)
        {
            if (IsSafeSqlString(str))
            {
                return str;
            }
            else
            {
                return Regex.Replace(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']", string.Empty);
            }
        }
        #endregion


        //检测用户信息中是否包含危险的字符串
        #region - bool IsSafeUserInfo(string str) -
        /// <summary>
        /// 检测用户信息中是否包含危险的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfo(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }
        #endregion
        
        //检测字符串中是否为存在“http://”
        #region - bool IsHttpString(string str) -
        /// <summary>
        /// 检测字符串中是否为存在“http://”
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsHttpString(string str)
        {
            return !Regex.IsMatch(str, @"http://");
        }
        #endregion

        //检测字符串是否为ip地址
        #region - bool IsIP(string ip) -
        /// <summary>
        /// 检测字符串是否为ip地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        //检测字符串中是否部分为ip 
        #region - bool IsIPSect(string ip) -
        /// <summary>
        /// 检测字符串中是否部分为ip 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }
        #endregion
        
        //检测字符是否为Email地址
        #region - bool IsEmail(str) -
        /// <summary>
        /// 检测是否为Email地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(string str)
        {
            str = str.Trim();
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion
        

        //---------------------------------------------------------------------

        //返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        #region - bool InIPArray()  -
        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {

            string[] userip = Utils.SplitString(ip, @".");
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = Utils.SplitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                if (r == 4)
                {
                    return true;
                }


            }
            return false;

        }
        #endregion
                
        //自定义的替换字符串函数
        #region - ReplaceString(SourceString, SearchString, ReplaceString, IsCaseInsensetive) -
        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        /// <param name="SourceString">源字符串</param>
        /// <param name="SearchString">需替换的字符串</param>
        /// <param name="ReplaceString">目标字符串</param>
        /// <param name="IsCaseInsensetive">是否判断大小写</param>
        /// <returns></returns>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
        #endregion

        //生成指定数量的html空格符号
        #region - string Spaces(nSpaces) -
        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        public static string Spaces(int nSpaces)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nSpaces; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }
        #endregion

        //过滤Html标记
        #region - RemoveHtml(content) -
        /// <summary>
        /// 过滤Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }
        #endregion

        //过滤HTML中的不安全标签
        #region - RemoveUnsafeHtml(content) -
        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }
        #endregion

        //从HTML中获取文本,保留br,p,img
        #region - GetTextFromHTML(HTML) -
        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string HTML)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(HTML, "");
        }
        #endregion

        //输出Html字符(不解析)
        #region - NoExcuteHtml(_htmlstr) -
        /// <summary>
        /// 输出Html字符(不解析)
        /// </summary>
        /// <param name="_htmlstr"></param>
        /// <returns></returns>
        public static string NoExcuteHtml(string _htmlstr)
        {
            string x = string.Empty;
            if (_htmlstr != null && _htmlstr.Length > 0)
            {
                x = _htmlstr.Replace(@"&", "&amp;");//将&设置为保留字
                x = x.Replace(@"<", "&lt;");
                x = x.Replace(@">", "&gt;");
                x = x.Replace(@" ", "&nbsp;");
                x = x.Replace("\r\n", "<br/>");
            }
            return x;
        }
        #endregion





        //--- 日期和时间 返回和判断 -------------------------------------------

        //返回字符串 标准日期（当前日期）
        #region - string GetDate() -
        /// <summary>
        /// 返回字符串 标准日期（当前日期）("yyyy-MM-dd")
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion

        //返回字符串 标准日期（日期字符） 重载+1 
        #region - string GetDate(string datetimestr, string replacestr) -
        /// <summary>
        /// 返回字符串 标准日期（日期字符）("yyyy-MM-dd")
        /// 将字符串转换为日期形式的字符串
        /// </summary>
        /// <param name="datetimestr">字符串</param>
        /// <param name="replacestr">需要替换成"1900-01-01"的字符串</param>
        /// <returns></returns>
        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }

            if (datetimestr.Equals(""))
            {
                return replacestr;
            }

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;

        }
        #endregion

        //返回当前时间（标准格式）
        #region - string GetTime() -
        /// <summary>
        /// 返回当前时间 ("HH:mm:ss")
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion

        //返回当前日期和时间（标准格式）
        #region - string GetDateTime()  -
        /// <summary>
        /// 返回当前日期和时间(yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        //返回相对于当前时间的相对天数（标准格式日期和时间）
        #region - string GetDateTime(int relativeday)  -
        /// <summary>
        /// 返回相对于当前时间的相对天数("yyyy-MM-dd HH:mm:ss")
        /// </summary>
        /// <param name="relativeday">相对加减天数（7,-7）</param>
        /// <returns></returns>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        //返回相差的秒数
        #region - StrDateDiffSeconds(Time, Sec) -
        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Sec"></param>
        /// <returns></returns>
        public static int StrDateDiffSeconds(string Time, int Sec)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddSeconds(Sec);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }
        #endregion

        //返回相差的分钟数
        #region - StrDateDiffMinutes(time, minutes) -
        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int StrDateDiffMinutes(string time, int minutes)
        {
            if (time == "" || time == null)
                return -1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }
        #endregion

        //返回相差的小时数
        #region - StrDateDiffHours(time, hours) -
        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int StrDateDiffHours(string time, int hours)
        {
            if (time == "" || time == null)
                return -1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }
        #endregion

        //返回标准日期和时间（详细格式）
        #region - string GetDateTimeF()  -
        /// <summary>
        /// 返回标准日期和时间(yyyy-MM-dd HH:mm:ss:fffffff)
        /// </summary>
        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }
        #endregion

        //判断是否符合时间格式
        #region - bool IsTime(string timeval) -
        /// <summary>
        /// 判断是否符合时间格式
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }
        #endregion

        //格式化日期制式
        #region - string FormatDateType(string _datestr, string _dateMode) -
        /// <summary>
        /// 格式化日期制式
        /// </summary>
        /// <param name="_datestr">日期字符串</param>
        /// <param name="_dateMode">显示制式</param>
        public static string FormatDateType(string _datestr, string _dateMode)
        {
            DateTime returnvalue;
            // 格式化 string 为 datetime
            returnvalue = DateTime.Parse(_datestr); //DateTime.ParseExact(_datestr, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            // 返回 模式字符串
            return returnvalue.ToString(_dateMode);
        }
        #endregion

        //获取星期几
        #region - string GetWeek() -
        /// <summary>
        /// 获取星期几
        /// </summary>
        /// <returns></returns>
        public static string GetWeek()
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            return week;
        }
        /// <summary>
        /// 数字星期转中文星期
        /// </summary>
        /// <param name="_w">数字星期</param>
        /// <returns></returns>
        public static string GetWeek(int _w)
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[_w];
            return week;
        }
        #endregion

        //公历转农历
        #region - string GetLunarCalendar(DateTime _today)   -
        /// <summary>
        /// 公历转农历
        /// </summary>
        /// <param name="_today">公历日期</param>
        /// <returns></returns>
        public static string GetLunarCalendar(DateTime _day)
        {
            ////天干
            //string[] TianGan = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

            ////地支
            //string[] DiZhi = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

            ////十二生肖
            //string[] ShengXiao = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

            //农历日期
            string[] DayName = {"*","初一","初二","初三","初四","初五",
                                "初六","初七","初八","初九","初十",
                                "十一","十二","十三","十四","十五",
                                "十六","十七","十八","十九","二十",
                                "廿一","廿二","廿三","廿四","廿五",
                                "廿六","廿七","廿八","廿九","三十"};

            //农历月份
            string[] MonthName = { "*", "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };

            //公历月计数天
            int[] MonthAdd = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            //农历数据
            int[] LunarData = {2635,333387,1701,1748,267701,694,2391,133423,1175,396438
                                ,3402,3749,331177,1453,694,201326,2350,465197,3221,3402
                                ,400202,2901,1386,267611,605,2349,137515,2709,464533,1738
                                ,2901,330421,1242,2651,199255,1323,529706,3733,1706,398762
                                ,2741,1206,267438,2647,1318,204070,3477,461653,1386,2413
                                ,330077,1197,2637,268877,3365,531109,2900,2922,398042,2395
                                ,1179,267415,2635,661067,1701,1748,398772,2742,2391,330031
                                ,1175,1611,200010,3749,527717,1452,2742,332397,2350,3222
                                ,268949,3402,3493,133973,1386,464219,605,2349,334123,2709
                                ,2890,267946,2773,592565,1210,2651,395863,1323,2707,265877};

            string sYear = _day.Year.ToString();
            string sMonth = _day.Month.ToString();
            string sDay = _day.Day.ToString();
            int year;
            int month;
            int day;
            try
            {
                year = int.Parse(sYear);
                month = int.Parse(sMonth);
                day = int.Parse(sDay);
            }
            catch
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
                day = DateTime.Now.Day;
            }

            int nTheDate;
            int nIsEnd;
            int k, m, n, nBit, i;
            string calendar = string.Empty;
            //计算到初始时间1921年2月8日的天数：1921-2-8(正月初一)
            nTheDate = (year - 1921) * 365 + (year - 1921) / 4 + day + MonthAdd[month - 1] - 38;
            if ((year % 4 == 0) && (month > 2))
                nTheDate += 1;
            //计算天干，地支，月，日
            nIsEnd = 0;
            m = 0;
            k = 0;
            n = 0;
            while (nIsEnd != 1)
            {
                if (LunarData[m] < 4095)
                    k = 11;
                else
                    k = 12;
                n = k;
                while (n >= 0)
                {
                    //获取LunarData[m]的第n个二进制位的值
                    nBit = LunarData[m];
                    for (i = 1; i < n + 1; i++)
                        nBit = nBit / 2;
                    nBit = nBit % 2;
                    if (nTheDate <= (29 + nBit))
                    {
                        nIsEnd = 1;
                        break;
                    }
                    nTheDate = nTheDate - 29 - nBit;
                    n = n - 1;
                }
                if (nIsEnd == 1)
                    break;
                m = m + 1;
            }
            year = 1921 + m;
            month = k - n + 1;
            day = nTheDate;
            //return year + "-" + month + "-" + day;

            if (k == 12)
            {
                if (month == LunarData[m] / 65536 + 1)
                    month = 1 - month;
                else if (month > LunarData[m] / 65536 + 1)
                    month = month - 1;
            }
            ////年
            //calendar = year + "年";
            ////生肖
            //calendar += ShengXiao[(year - 4) % 60 % 12].ToString() + "年 ";
            //// //天干
            //calendar += TianGan[(year - 4) % 60 % 10].ToString();
            //// //地支
            //calendar += DiZhi[(year - 4) % 60 % 12].ToString() + " ";

            //农历月
            if (month < 1)
                calendar += "闰" + MonthName[-1 * month].ToString() + "月";
            else
                calendar += MonthName[month].ToString() + "月";

            //农历日
            calendar += DayName[day].ToString();

            return calendar;
        }
        #endregion



        //--- 字符串、数据再处理 ------------------------------------------------------
                
        //得到字符串长度，一个汉字长度为2
        #region - int StrLength(string inputString) -
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        #endregion

        //截取指定长度字符串
        #region - string ClipString(string inputString, int len) -
        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="len">字符数</param>
        /// <returns></returns>
        public static string ClipString(string inputString, int len)
        {
            bool isShowFix = true;
            //bool isShowFix = false;
            //if (len % 2 == 1)
            //{
            //    isShowFix = true;
            //    len--;
            //}
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (isShowFix && mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        #endregion

        //排除重复字符串(','数组)
        #region - RemoveEcho(string _str) -
        /// <summary>
        /// 排除重复字符串(','数组)
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string RemoveEcho(string _str)
        {
            string returnvalue = "";

            string[] stringArray = Utils.SplitString(_str, ",");

            List<string> listString = new List<string>();
            foreach (string eachString in stringArray)
            {
                if (!listString.Contains(eachString))
                    listString.Add(eachString);
            }

            foreach (string eachString in listString)
            {
                if (eachString != "")
                    returnvalue += eachString + ",";
            }

            return returnvalue;
        }
        #endregion


        //单复数转换
        #region - ToSingular(string _str),ToPlural(string _str) -
        /// <summary>
        /// 单词变成单数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToSingular(string _str)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex plural3 = new Regex("(?<keep>[sxzh])es$");
            Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

            if (plural1.IsMatch(_str))
                return plural1.Replace(_str, "${keep}y");
            else if (plural2.IsMatch(_str))
                return plural2.Replace(_str, "${keep}");
            else if (plural3.IsMatch(_str))
                return plural3.Replace(_str, "${keep}");
            else if (plural4.IsMatch(_str))
                return plural4.Replace(_str, "${keep}");

            return _str;
        }
        /// <summary>
        /// 单词变成复数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToPlural(string _str)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
            Regex plural3 = new Regex("(?<keep>[sxzh])$");
            Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

            if (plural1.IsMatch(_str))
                return plural1.Replace(_str, "${keep}ies");
            else if (plural2.IsMatch(_str))
                return plural2.Replace(_str, "${keep}s");
            else if (plural3.IsMatch(_str))
                return plural3.Replace(_str, "${keep}es");
            else if (plural4.IsMatch(_str))
                return plural4.Replace(_str, "${keep}s");

            return _str;
        }
        #endregion


        //--- 字符串截取 ----------------------------------------------------------------------------------------------

        //取指定长度的字符串
        #region - string GetSubString(string p_SrcString, int p_Length, string p_TailString) -
        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }
        #endregion

        //取指定长度的字符串
        #region - string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString) -
        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }
        #endregion

        //取指定长度的字符串(Unicode)
        #region - string GetSubStringUnicode(string str, int len, string p_TailString) -
        /// <summary>
        /// 取指定长度的字符串(Unicode)
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <param name="len">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns></returns>
        public static string GetSubStringUnicode(string str, int len, string p_TailString)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = str.Substring(0, pos) + p_TailString;
            }
            else
                result = str;

            return result;
        }
        #endregion
        
        //分割字符串
        #region - string[] SplitString(string strContent, string strSplit)  -
        /// <summary>
        /// 按照分割符将字符串分割为数组
        /// </summary>
        /// <param name="strContent">需要分割的内容</param>
        /// <param name="strSplit">分割符</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 按照分割符将字符串分割为数组
        /// </summary>
        /// <param name="strContent">需要分割的内容</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="p_3">获得数组的数量</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int p_3)
        {
            string[] result = new string[p_3];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < p_3; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        //提取两个字符之间的字符串
        #region - string SplitString(string strContent, string strStart, string strEnd) -
        /// <summary>
        /// 提取两个字符之间的字符串
        /// </summary>
        /// <param name="strContent">需要提取的内容</param>
        /// <param name="strStart">起始字符</param>
        /// <param name="strEnd">结束字符</param>
        /// <returns></returns>
        public static string SplitString(string strContent, string strStart, string strEnd)
        {
            int iStart = strContent.IndexOf(strStart.Trim()) + strStart.Trim().Length;
            string revalue = strContent;
            revalue = strContent.Substring(iStart);

            int iEnd = revalue.LastIndexOf(strEnd.Trim());
            revalue = revalue.Substring(0, iEnd);

            return revalue;
        }
        #endregion



        //--- 验证码 -----------------------------------------------------------------------------------------------

        //验证码生成的取值范围
        #region - string[] verifycodeRange -
        /// <summary>
        /// 验证码生成的取值范围
        /// </summary>
        private static string[] verifycodeRange = { "0","1","2","3","4","5","6","7","8","9",
                                                    "a","b","c","d","e","f","g",
                                                    "h",    "j","k",    "m","n",
                                                        "p","q",    "r","s","t",
                                                    "u","v","w",    "x","y"
        
                                                  };
        #endregion

        //生成验证码所使用的随机数发生器
        #region - Random verifycodeRandom -
        /// <summary>
        /// 生成验证码所使用的随机数发生器
        /// </summary>
        private static Random verifycodeRandom = new Random();
        #endregion

        //产生验证码
        #region - string CreateAuthStr(len) -
        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>验证码</returns>
        public static string CreateAuthStr(int len)
        {
            int number;
            StringBuilder checkCode = new StringBuilder();

            Random random = new Random();

            for (int i = 0; i < len; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                {
                    checkCode.Append((char)('0' + (char)(number % 10)));
                }
                else
                {
                    checkCode.Append((char)('A' + (char)(number % 26)));
                }

            }

            return checkCode.ToString();
        }
        #endregion

        //产生验证码
        #region - string CreateAuthStr(len, OnlyNum) -
        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="OnlyNum">是否仅为数字 true:仅数字</param>
        /// <returns></returns>
        public static string CreateAuthStr(int len, bool OnlyNum)
        {
            int number;
            StringBuilder checkCode = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                if (!OnlyNum)
                {
                    number = verifycodeRandom.Next(0, verifycodeRange.Length);
                }
                else
                {
                    number = verifycodeRandom.Next(0, 10);
                }
                checkCode.Append(verifycodeRange[number]);
            }

            return checkCode.ToString();
        }
        #endregion



        //--- Utils <- AES&DES 加密解密 -------------------------------------------------------------------------------

        // Url字符串转码
        #region - UrlDecode(string str)，UrlEncode(string str) -
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(string str)
        {
           return System.Web.HttpUtility.UrlDecode(str);
        }
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            return System.Web.HttpUtility.UrlEncode(str);
        }
        #endregion

        //MD5函数
        #region - MD5(string str) -
        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret;
        }
        #endregion

        //SHA256函数
        #region - SHA256(string str) -
        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
        }
        #endregion

        //加密字符串
        #region - string Encrypt(string encryptString) -
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encrypt(string encryptString)
        {
            string encryptKey = "dscmskey";
            return DES.Encode(encryptString, encryptKey);
        }
        #endregion

        //解密字符串
        #region - string Decrypt(string encryptString) -
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="encryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decrypt(string encryptString)
        {
            string encryptKey = "dscmskey";
            return DES.Decode(encryptString, encryptKey);
        }
        #endregion
        
    }

    #region - Class:AES（加密解密） -
    /// <summary> 
    /// AES 加密解密类
    /// </summary> 
    public class AES
    {
        //默认密钥向量
        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = Utils.GetSubString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = Keys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = Utils.GetSubString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }
        }
    }
    #endregion

    #region - Class:DES（加密解密） -
    /// <summary> 
    /// DES 加密解密类
    /// </summary> 
    public class DES
    {
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = Utils.GetSubString(encryptKey, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = Utils.GetSubString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();//执行引擎错误
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }
    } 
    #endregion


}
