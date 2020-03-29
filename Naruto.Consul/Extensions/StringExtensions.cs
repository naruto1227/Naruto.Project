using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Consul.Extensions
{
    public static class StringExtensions
    {
        public static void IsNull(this string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
        }
        /// <summary>
        /// 转换成byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string value)
        {
            value.IsNull();
            return UTF8Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 转换成byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CoverToString(this byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("需要转换的值不能为空");
            return UTF8Encoding.UTF8.GetString(value);
        }
    }
}
