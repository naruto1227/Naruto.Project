using Fate.Infrastructure.Repository.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Repository
{
    /// <summary>
    /// 从库的连接工厂
    /// </summary>
    public interface ISlaveDbConnectionFactory
    {
        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="eFOptions"></param>
        /// <returns></returns>
        List<SlaveDbConnection> Get(EFOptions eFOptions);
    }
}
