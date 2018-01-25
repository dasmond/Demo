﻿using System.ComponentModel;
using System.Globalization;
using System.Text;
using System;

namespace DSNN.Core.Common
{
    /// <summary>工具类</summary>
    /// <remarks>
    /// 采用静态架构，允许外部重载工具类的各种实现<seealso cref="DefaultConvert"/>。
    /// 所有类型转换均支持默认值，默认值为该default(T)，在转换失败时返回默认值。
    /// </remarks>
    public static class Utility
    {
        #region 类型转换
        private static DefaultConvert _Convert = new DefaultConvert();
        /// <summary>类型转换提供者</summary>
        /// <remarks>重载默认提供者<seealso cref="DefaultConvert"/>并赋值给<see cref="Convert"/>可改变所有类型转换的行为</remarks>
        public static DefaultConvert Convert { get { return _Convert; } set { _Convert = value; } }

        /// <summary>转为整数，转换失败时返回默认值。支持字符串、全角、字节数组（小端）</summary>
        /// <remarks>Int16/UInt32/Int64等，可以先转为最常用的Int32后再二次处理</remarks>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static Int32 ToInt(this Object value, Int32 defaultValue = 0) { return _Convert.ToInt(value, defaultValue); }

        /// <summary>转为浮点数，转换失败时返回默认值。支持字符串、全角、字节数组（小端）</summary>
        /// <remarks>Single可以先转为最常用的Double后再二次处理</remarks>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static Double ToDouble(this Object value, Double defaultValue = 0) { return _Convert.ToDouble(value, defaultValue); }

        /// <summary>转为布尔型，转换失败时返回默认值。支持大小写True/False、0和非零</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static Boolean ToBoolean(this Object value, Boolean defaultValue = false) { return _Convert.ToBoolean(value, defaultValue); }

        /// <summary>转为时间日期，转换失败时返回最小时间</summary>
        /// <param name="value">待转换对象</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Object value) { return _Convert.ToDateTime(value, DateTime.MinValue); }

        /// <summary>转为时间日期，转换失败时返回默认值</summary>
        /// <remarks><see cref="DateTime.MinValue"/>不是常量无法做默认值</remarks>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Object value, DateTime defaultValue) { return _Convert.ToDateTime(value, defaultValue); }

        /// <summary>时间日期转为yyyy-MM-dd HH:mm:ss完整字符串</summary>
        /// <remarks>最常用的时间日期格式，可以无视各平台以及系统自定义的时间格式</remarks>
        /// <param name="value">待转换对象</param>
        /// <returns></returns>
        public static String ToFullString(this DateTime value) { return _Convert.ToFullString(value); }

        /// <summary>时间日期转为yyyy-MM-dd HH:mm:ss完整字符串，支持指定最小时间的字符串</summary>
        /// <remarks>最常用的时间日期格式，可以无视各平台以及系统自定义的时间格式</remarks>
        /// <param name="value">待转换对象</param>
        /// <param name="emptyValue">字符串空值时（DateTime.MinValue）显示的字符串，null表示原样显示最小时间，String.Empty表示不显示</param>
        /// <returns></returns>
        public static String ToFullString(this DateTime value, String emptyValue = null) { return _Convert.ToFullString(value, emptyValue); }

        /// <summary>时间日期转为指定格式字符串</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="emptyValue">字符串空值时显示的字符串，null表示原样显示最小时间，String.Empty表示不显示</param>
        /// <returns></returns>
        public static String ToString(this DateTime value, String format, String emptyValue) { return _Convert.ToString(value, format, emptyValue); }
        #endregion
    }

    /// <summary>默认转换</summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class DefaultConvert
    {
        /// <summary>转为整数</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public virtual Int32 ToInt(Object value, Int32 defaultValue)
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

        /// <summary>转为浮点数</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public virtual Double ToDouble(Object value, Double defaultValue)
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

        //static readonly String[] trueStr = new String[] { "True", "Y", "Yes", "On" };
        //static readonly String[] falseStr = new String[] { "False", "N", "N", "Off" };

        /// <summary>转为布尔型。支持大小写True/False、0和非零</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public virtual Boolean ToBoolean(Object value, Boolean defaultValue)
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

        /// <summary>转为时间日期</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public virtual DateTime ToDateTime(Object value, DateTime defaultValue)
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

        /// <summary>全角为半角</summary>
        /// <remarks>全角半角的关系是相差0xFEE0</remarks>
        /// <param name="str"></param>
        /// <returns></returns>
        String ToDBC(String str)
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

        /// <summary>时间日期转为yyyy-MM-dd HH:mm:ss完整字符串</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="emptyValue">字符串空值时显示的字符串，null表示原样显示最小时间，String.Empty表示不显示</param>
        /// <returns></returns>
        public virtual String ToFullString(DateTime value, String emptyValue = null)
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

        /// <summary>时间日期转为指定格式字符串</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="emptyValue">字符串空值时显示的字符串，null表示原样显示最小时间，String.Empty表示不显示</param>
        /// <returns></returns>
        public virtual String ToString(DateTime value, String format, String emptyValue)
        {
            if (emptyValue != null && value <= DateTime.MinValue) return emptyValue;

            //return value.ToString(format ?? "yyyy-MM-dd HH:mm:ss");

            if (format == null || format == "yyyy-MM-dd HH:mm:ss") return ToFullString(value, emptyValue);

            return value.ToString(format);
        }
    }
}