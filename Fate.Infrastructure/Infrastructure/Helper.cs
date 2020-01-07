using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Linq;

namespace Fate.Infrastructure.Infrastructure
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class Helper
    {
        /// 32位加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToMD5(this string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2").PadLeft(2, '0'));
                }
                string hash = sBuilder.ToString();
                return hash.ToUpper();
            }
        }


        /// <summary>
        /// 实体映射
        /// </summary>
        /// <typeparam name="T">同步目标的类型</typeparam>
        /// <param name="sourceObj">同步源对象</param>
        /// <param name="targetObj">同步目标对象</param>
        /// <param name="ignoreFileds">不需要同步的键值</param>
        public static void AttributeSync<T>(this object sourceObj, ref T targetObj, params string[] ignoreFileds) where T : class
        {
            Type sourceType = sourceObj.GetType();
            Type targetType = targetObj.GetType();
            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] targetProperties = targetType.GetProperties();

            if (ignoreFileds == null)
            {
                ignoreFileds = new string[0];
            }
            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                if (ignoreFileds.Contains(sourceProperty.Name))
                {
                    continue;
                }
                foreach (PropertyInfo targetProperty in targetProperties)
                {
                    if (targetProperty.Name.Equals(sourceProperty.Name, StringComparison.CurrentCultureIgnoreCase) && sourceProperty.GetValue(sourceObj, null) != null)
                    {
                        targetProperty.SetValue(targetObj, Convert.ChangeType(sourceProperty.GetValue(sourceObj, null), targetProperty.PropertyType), null);
                        break;
                    }
                }
            }
        }
    }
}
