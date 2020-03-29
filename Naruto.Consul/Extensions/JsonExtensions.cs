using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Naruto.Consul.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="soure"></param>
        /// <returns></returns>
        public static string ToJson(this object soure)
        {
            if (soure == null)
                return null;
            return JsonConvert.SerializeObject(soure);
        }
        /// <summary>
        /// 将json字符串序列化为字典类型
        /// </summary>
        /// <param name="soure"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDic(this string soure)
        {
            if (string.IsNullOrWhiteSpace(soure))
                return null;
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(soure);
        }
        /// <summary>
        /// 转换成对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soure"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string soure) where T : class
        {
            if (soure == null)
                return default;
            return JsonConvert.DeserializeObject<T>(soure);
        }
    }
}
