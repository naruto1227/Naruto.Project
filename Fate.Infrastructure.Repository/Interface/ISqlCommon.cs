using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Repository.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// sql的公共接口
    /// </summary>
    public interface ISqlCommon 
    {
        /// <summary>
        /// 执行sql语句返回第一行第一列的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, params object[] _params);

        /// <summary>
        /// 执行sql语句返回第一行第一列的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(string sql, params object[] _params);
    }
}
