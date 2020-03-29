using Microsoft.EntityFrameworkCore;
using System;

namespace Naruto.Repository.Object
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 工作单元的参数
    /// </summary>
    public abstract class UnitOfWorkOptions
    {
        /// <summary>
        /// master 库的连接字符串
        /// </summary>
        public string WriteReadConnectionString;
        /// <summary>
        /// 是否开启读写分离的操作 
        /// </summary>
        public bool IsOpenMasterSlave = false;
        /// <summary>
        /// 上下文的类型
        /// </summary>
        public Type DbContextType = default;
        /// <summary>
        /// 是否开启事务
        /// </summary>
        public bool IsBeginTran = false;

        /// <summary>
        /// 是否当前的连接为读库的还是主库的 true 为从库 false 为主库
        /// </summary>
        public bool IsSlaveOrMaster = false;

        /// <summary>
        /// 更改的数据库的名字
        /// </summary>
        public string ChangeDataBaseName = default;

        /// <summary>
        /// 超时时间
        /// </summary>
        public int? CommandTimeout = null;

    }
    /// <summary>
    /// 张海波
    /// 2020-02-20
    /// 工作单元的参数
    /// </summary>
    public class UnitOfWorkOptions<TDbContext>: UnitOfWorkOptions where TDbContext : DbContext
    {
    }
}
