using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.MongoDB
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// 工具类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="source"></param>
        public static void CheckNull(this object source)
        {
            if (source == null)
                throw new ArgumentNullException(source.GetType().Name);
        }

        public static bool IsNullOrEmpty(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return true;
            else
                return false;
        }
    }
}
