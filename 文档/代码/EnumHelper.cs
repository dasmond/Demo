using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System;

namespace DSNN.Core.Common
{
    /// <summary>枚举类型助手类</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumHelper
    {


        /// <summary>获取枚举字段的注释</summary>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static String GetDescription(this Enum value)
        {
            var type = value.GetType();
            var item = type.GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);
            
            DescriptionAttribute att = Attribute.GetCustomAttribute(item,
                typeof(DescriptionAttribute), false) as DescriptionAttribute;
            if (att != null && !String.IsNullOrEmpty(att.Description))
            {
                return att.Description;
            }

            //object[] objs = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //if (objs != null || objs.Length > 0)
            //{
            //    DescriptionAttribute att = (DescriptionAttribute)objs[0];
            //    if (att != null && !String.IsNullOrEmpty(att.Description))
            //    {
            //        return att.Description;
            //    }
            //}

            return null;
        }


        /// <summary>获取枚举类型的所有字段注释</summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<TEnum, String> GetDescriptions<TEnum>()
        {
            var dic = new Dictionary<TEnum, String>();

            foreach (var item in GetDescriptions(typeof(TEnum)))
            {
                dic.Add((TEnum)(Object)item.Key, item.Value);
            }

            return dic;
        }

        /// <summary>获取枚举类型的所有字段注释</summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<Int32, String> GetDescriptions(Type enumType)
        {
            var dic = new Dictionary<Int32, String>();
            foreach (var item in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (!item.IsStatic) continue;
                
                Int32 value = Convert.ToInt32(item.GetValue(null));

                String des = item.Name;

                object[] objs = item.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                if (objs != null || objs.Length > 0)
                {
                    DisplayNameAttribute dna = (DisplayNameAttribute)objs[0];
                    if (dna != null && !String.IsNullOrEmpty(dna.DisplayName))
                    {
                        des = dna.DisplayName;
                    }
                }

                objs = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null || objs.Length > 0)
                {
                    DescriptionAttribute att = (DescriptionAttribute)objs[0];
                    if (att != null && !String.IsNullOrEmpty(att.Description))
                    {
                        des = att.Description;
                    }
                }
                
                //dic.Add(value, des);
                // 有些枚举可能不同名称有相同的值
                dic[value] = des;
            }

            return dic;
        }


    }
}