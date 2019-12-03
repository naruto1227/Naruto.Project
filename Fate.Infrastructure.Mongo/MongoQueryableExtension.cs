using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mongo
{
    public static class MongoQueryableExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IMongoQueryable<T> PageBy<T>(this IMongoQueryable<T> @this, int page, int pageSize)
        {
            if (@this == null)
                return null;
            if (page < 1)
                page = 1;
            return @this.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
