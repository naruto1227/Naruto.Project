using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mongo
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


    }
}
