using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DsLib.Common
{
    public static class Utils
    {
        
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
                strPath = strPath.Replace("~/", "").Replace("/", "\\");
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion


        //GPS：降度分秒格式经纬度转换为小数经纬度
        #region - double GPSConvertToDegree(string _Value) -
        /// <summary>
        /// GPS：降度分秒格式经纬度转换为小数经纬度
        /// </summary>
        /// <param name="_Value">度分秒经纬度</param>
        /// <returns>小数经纬度</returns>
        public static double GPSConvertToDegree(string _Value)
        {
            double Ret = 0.0;
            string[] TempStr = _Value.Split('.');
            string x = TempStr[0].Substring(0, TempStr[0].Length - 2);
            string y = TempStr[0].Substring(TempStr[0].Length - 2, 2);
            string z = TempStr[1].Substring(0, 4);
            Ret = Convert.ToDouble(x) + Convert.ToDouble(y) / 60 + Convert.ToDouble(z) / 600000;
            return Ret;
        }
        #endregion

        //static readonly String[] trueStr = new String[] { "True", "Y", "Yes", "On" };
        //static readonly String[] falseStr = new String[] { "False", "N", "N", "Off" };

        //转为整数
        #region - Int32 ToInt(this Object value, Int32 defaultValue = 0) -
        /// <summary>
        /// 转为整数
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static Int32 ToInt(this Object value, Int32 defaultValue = 0)
        {
            if (value == null) return defaultValue;

            // 特殊处理字符串，也是最常见的
            if (value is String)
            {
                var str = value as String;
                str = ToDBC(str).Trim();
                if (String.IsNullOrEmpty(str)) return defaultValue;

                var n = defaultValue;
                if (Int32.TryParse(str, out n)) return n;
                return defaultValue;
            }
            else if (value is Byte[])
            {
                var buf = (Byte[])value;
                if (buf == null || buf.Length < 1) return defaultValue;

                switch (buf.Length)
                {
                    case 1:
                        return buf[0];
                    case 2:
                        return BitConverter.ToInt16(buf, 0);
                    case 3:
                        return BitConverter.ToInt32(new Byte[] { buf[0], buf[1], buf[2], 0 }, 0);
                    case 4:
                        return BitConverter.ToInt32(buf, 0);
                    default:
                        break;
                }
            }

            //var tc = Type.GetTypeCode(value.GetType());
            //if (tc >= TypeCode.Char && tc <= TypeCode.Decimal) return Convert.ToInt32(value);

            try
            {
                return Convert.ToInt32(value);
            }
            catch { return defaultValue; }
        }
        #endregion

        //转为浮点数
        #region - Double ToDouble(this Object value, Double defaultValue = 0) -
        /// <summary>
        /// 转为浮点数
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static Double ToDouble(this Object value, Double defaultValue = 0)
        {
            if (value == null) return defaultValue;

            // 特殊处理字符串，也是最常见的
            if (value is String)
            {
                var str = value as String;
                str = ToDBC(str).Trim();
                if (String.IsNullOrEmpty(str)) return defaultValue;

                var n = defaultValue;
                if (Double.TryParse(str, out n)) return n;
                return defaultValue;
            }
            else if (value is Byte[])
            {
                var buf = (Byte[])value;
                if (buf == null || buf.Length < 1) return defaultValue;

                switch (buf.Length)
                {
                    case 1:
                        return buf[0];
                    case 2:
                        return BitConverter.ToInt16(buf, 0);
                    case 3:
                        return BitConverter.ToInt32(new Byte[] { buf[0], buf[1], buf[2], 0 }, 0);
                    case 4:
                        return BitConverter.ToInt32(buf, 0);
                    default:
                        // 凑够8字节
                        if (buf.Length < 8)
                        {
                            var bts = new Byte[8];
                            Buffer.BlockCopy(buf, 0, bts, 0, buf.Length);
                            buf = bts;
                        }
                        return BitConverter.ToDouble(buf, 0);
                }
            }

            try
            {
                return Convert.ToDouble(value);
            }
            catch { return defaultValue; }
        }
        #endregion

        //转为浮点数
        #region - Boolean ToBoolean(this Object value, Boolean defaultValue = false) -
        /// <summary>
        /// 转为布尔型。支持大小写True/False、0和非零
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static Boolean ToBoolean(this Object value, Boolean defaultValue = false)
        {
            if (value == null) return defaultValue;

            // 特殊处理字符串，也是最常见的
            if (value is String)
            {
                var str = value as String;
                str = ToDBC(str).Trim();
                if (String.IsNullOrEmpty(str)) return defaultValue;

                var b = defaultValue;
                if (Boolean.TryParse(str, out b)) return b;

                if (String.Equals(str, Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) return true;
                if (String.Equals(str, Boolean.FalseString, StringComparison.OrdinalIgnoreCase)) return false;

                // 特殊处理用数字0和1表示布尔型
                var n = 0;
                if (Int32.TryParse(str, out n)) return n > 0;

                return defaultValue;
            }

            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return defaultValue; }
        }
        #endregion

        //转为时间日期
        #region - DateTime ToDateTime(this Object value) -
        /// <summary>
        /// 转为时间日期
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Object value)
        {
            return ToDateTime(value, DateTime.MinValue);
        }
        #endregion

        //转为时间日期
        #region - DateTime ToDateTime(this Object value) -
        /// <summary>转为时间日期</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Object value, DateTime defaultValue)
        {
            if (value == null) return defaultValue;

            // 特殊处理字符串，也是最常见的
            if (value is String)
            {
                var str = value as String;
                str = ToDBC(str).Trim();
                if (String.IsNullOrEmpty(str)) return defaultValue;

                var n = defaultValue;
                if (DateTime.TryParse(str, out n)) return n;
                if (str.Contains("-") && DateTime.TryParseExact(str, "yyyy-M-d", null, DateTimeStyles.None, out n)) return n;
                if (str.Contains("/") && DateTime.TryParseExact(str, "yyyy/M/d", null, DateTimeStyles.None, out n)) return n;
                if (DateTime.TryParse(str, out n)) return n;
                return defaultValue;
            }

            try
            {
                return Convert.ToDateTime(value);
            }
            catch { return defaultValue; }
        }
        #endregion

        //时间日期转为yyyy-MM-dd HH:mm:ss完整字符串
        #region - String ToFullString(this DateTime value, String emptyValue = null) -
        /// <summary>
        /// 时间日期转为yyyy-MM-dd HH:mm:ss完整字符串
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="emptyValue">字符串空值时显示的字符串，null表示原样显示最小时间，String.Empty表示不显示</param>
        /// <returns></returns>
        public static String ToFullString(this DateTime value, String emptyValue = null)
        {
            if (emptyValue != null && value <= DateTime.MinValue) return emptyValue;

            //return value.ToString("yyyy-MM-dd HH:mm:ss");

            var dt = value;
            var sb = new StringBuilder();
            sb.Append(dt.Year.ToString().PadLeft(4, '0'));
            sb.Append("-");
            sb.Append(dt.Month.ToString().PadLeft(2, '0'));
            sb.Append("-");
            sb.Append(dt.Day.ToString().PadLeft(2, '0'));
            sb.Append(" ");

            sb.Append(dt.Hour.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(dt.Minute.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(dt.Second.ToString().PadLeft(2, '0'));

            return sb.ToString();
        }
        #endregion

        //时间日期转为指定格式字符串
        #region - String ToString(this DateTime value, String format, String emptyValue) -
        /// <summary>
        /// 时间日期转为指定格式字符串
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="emptyValue">字符串空值时显示的字符串，null表示原样显示最小时间，String.Empty表示不显示</param>
        /// <returns></returns>
        public static String ToString(this DateTime value, String format, String emptyValue)
        {
            if (emptyValue != null && value <= DateTime.MinValue) return emptyValue;

            //return value.ToString(format ?? "yyyy-MM-dd HH:mm:ss");

            if (format == null || format == "yyyy-MM-dd HH:mm:ss") return ToFullString(value, emptyValue);

            return value.ToString(format);
        }
        #endregion

        //全角转半角
        #region - String ToDBC(this String str) -
        /// <summary>全角转半角</summary>
        /// <remarks>全角半角的关系是相差0xFEE0</remarks>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String ToDBC(this String str)
        {
            var ch = str.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                // 全角空格
                if (ch[i] == 0x3000)
                    ch[i] = (char)0x20;
                else if (ch[i] > 0xFF00 && ch[i] < 0xFF5F)
                    ch[i] = (char)(ch[i] - 0xFEE0);
            }
            return new string(ch);
        }
        #endregion




    }
}
