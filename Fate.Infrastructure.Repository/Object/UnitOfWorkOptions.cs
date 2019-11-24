using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Repository.Object
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 工作单元的参数
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// master 库的连接字符串
        /// </summary>
        public string WriteReadConnectionString;

        /// <summary>
        /// 上下文的类型
        /// </summary>
        public Type DbContextType = default;
        /// <summary>
        /// 是否提交事务
        /// </summary>
        public bool IsSumbitTran = false;

        /// <summary>
        /// 是否当前的连接为读库的还是主库的 true 为从库 false 为主库
        /// </summary>
        public bool IsSlaveOrMaster = false;

        /// <summary>
        /// 更改的数据库的名字
        /// </summary>
        public string ChangeDataBaseName = default;

        /// <summary>
        /// 是否已经强制更改数据库连接 
        /// </summary>
        public bool IsMandatory = false;

        /// <summary>
        /// 是否开启读写分离的操作 
        /// </summary>
        public bool IsOpenMasterSlave = false;

    }
}
