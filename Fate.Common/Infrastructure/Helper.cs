using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
namespace Fate.Common.Infrastructure
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public class Helper
    {
        /// 32位加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Get32MD5(string source)
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
    }
}
