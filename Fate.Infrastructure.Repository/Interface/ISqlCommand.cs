using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Repository.Interface
{
    public interface ISqlCommand : ISqlCommon
    {
        /// <summary>
        /// 执行增删改的异步操作 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        Task<int> ExecuteNonQueryAsync(string sql,  object[] _params=default);
        /// <summary>
        /// 执行增删改的操作 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql,  object[] _params = default);
    }
    /// <summary>
    /// 张海波
    /// 2019-10-25
    /// 执行SQL语句的增删改操作
    /// </summary>
    public interface ISqlCommand<TDbContext> : ISqlCommand, IRepositoryDependency where TDbContext : DbContext
    {

    }
}
