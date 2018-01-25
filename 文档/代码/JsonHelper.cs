using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DSNN.Core.Common
{
    //public class JObject : Newtonsoft.Json.Linq.JObject
    //{
    //}

    /// <summary>
    /// Json  帮助类
    /// </summary>
    public class JsonHelper
    {
        //默认构造
        public JsonHelper() { }


        //反序列化对象（匿名 | 索引器：JObject）??? 无用 JObject、JArray...封装问题未解决 ???
        #region - DeserializeObject(string _json) -
        /// <summary>
        /// 反序列化对象（匿名 | 索引器：JObject）??? 无用 JObject、JArray...封装问题未解决 ???
        /// </summary>
        /// <param name="_json"></param>
        /// <returns></returns>
        public static object DeserializeObject(string _json)
        {            
            return JsonConvert.DeserializeObject(_json);
            // return JsonConvert.DeserializeObject(_json) as JObject;   
        }
        #endregion
        
        //反序列化对象
        #region - DeserializeObject<T>(string _json) -
        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// 使用方法： List&lt;model&gt; ml = JsonHelper.DeserializeObject&lt;List&lt;model&gt;&gt;(JsonString);
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string _json)
        {
            var _settings = new JsonSerializerSettings();

            //_settings.DateFormatString = "yyyy-MM-dd HH:mm";

            //Gets or sets the maximum depth allowed when reading JSON. Reading past this depth will throw a JsonReaderException.
            //_settings.MaxDepth = 1;

            //Gets or sets how reference loops (e.g. a class referencing itself) is handled.
            //_settings.ReferenceLoopHandling = ReferenceLoopHandling.Error;

            //_settings.MissingMemberHandling = MissingMemberHandling.Ignore;

            //_settings.NullValueHandling = NullValueHandling.Include;

            //_settings.DefaultValueHandling = DefaultValueHandling.Ignore;

            //_settings.ObjectCreationHandling = ObjectCreationHandling.Auto;

            //_settings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            //_settings.ConstructorHandling = ConstructorHandling.Default;

            //_settings.TypeNameHandling = TypeNameHandling.None;

            //_settings.Formatting = Formatting.Indented;

            return JsonConvert.DeserializeObject<T>(_json, _settings);
            

        }
        #endregion

        //反序列化JSON到给定的匿名对象
        #region - DeserializeAnonymousType<T>(string json, T anonymousTypeObject) -
        /// <summary>
        /// 反序列化JSON到给定的匿名对象
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            //var _settings = new JsonSerializerSettings();
            //_settings.ContractResolver = new NullToEmptyStringResolver();
            //var _settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }

            #region - Json反序列化时的属性值处理 -
            //public class NullToEmptyStringResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
            //{
            //    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            //    {
            //        return type.GetProperties()
            //                .Select(p =>
            //                {
            //                    var jp = base.CreateProperty(p, memberSerialization);
            //                    jp.ValueProvider = new NullToEmptyStringValueProvider(p);
            //                    return jp;
            //                }).ToList();
            //    }
            //}
            //public class NullToEmptyStringValueProvider : IValueProvider
            //{
            //    PropertyInfo _MemberInfo;
            //    public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
            //    {
            //        _MemberInfo = memberInfo;
            //    }
            //    public object GetValue(object target)
            //    {
            //        object result = _MemberInfo.GetValue(target);
            //        if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
            //        return result;
            //    }
            //    public void SetValue(object target, object value)
            //    {
            //        _MemberInfo.SetValue(target, value);
            //    }
            //}
            #endregion

        #endregion

        //序列化对象
        #region - SerializeObject(object _obj) -
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="_obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object _obj)
        {
            var _settings = new JsonSerializerSettings();
            _settings.DateFormatString = "yyyy-MM-dd HH:mm";

            return JsonConvert.SerializeObject(_obj, Formatting.Indented, _settings);
        }
        #endregion



        //序列化对象
        #region - SerializeObject(object _obj, string[] _props) -
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="_obj">对象</param>
        /// <param name="_props">需要序列化的属性</param>
        /// <returns></returns>
        public static string SerializeObject(object _obj, string[] _props)
        {

            var _settings = new JsonSerializerSettings();
            //格式化时间
            _settings.DateFormatString = "yyyy-MM-dd HH:mm";
            //哪些属性要序列化
            _settings.ContractResolver = new PropsContractResolver(_props);

            //TODO: 序列化对象时一部分对象丢失，泛型中包含泛型，异常处理
            //Gets or sets how reference loops (e.g. a class referencing itself) is handled.
            //_settings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
            
            return JsonConvert.SerializeObject(_obj, Formatting.Indented, _settings);

        }
        #endregion

        /// 自定义的Json解析器
        #region - PropsContractResolver : DefaultContractResolver -
        private class PropsContractResolver : DefaultContractResolver
        {
            string[] props = null;

            public PropsContractResolver(string[] props)
            {
                //指定要序列化属性的清单
                this.props = props;
            }

            /// <summary>
            /// 重写CreateProperties方法
            /// </summary>
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                //过滤base.CreateProperties()传回的IList<JsonProperty>
                IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
                //只保留清单有列出的属性
                list = list.Where(p => props.Contains(p.PropertyName)).ToList();
                return list;
            }

            //REF: http://james.newtonking.com/archive/2009/10/23/efficient-json-with-json-net-reducing-serialized-json-size.aspx
        }
        #endregion


        /* 
        public class HideAgeContractResolver : DefaultContractResolver
        {
            //REF: http://james.newtonking.com/projects/json/help/index.html?topic=html/ContractResolver.htm
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty p = base.CreateProperty(member, memberSerialization);
                if (p.PropertyName == "Age")
                {
                    //依性别决定是否要序列化
                    p.ShouldSerialize = instance =>
                    {
                        Person person = (Person)instance;
                        return person.Gender == Gender.Male;
                    };
                }
                return p;
            }
        }
        */


    }
}
