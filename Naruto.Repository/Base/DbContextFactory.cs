using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Naruto.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Naruto.Repository.Object;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Naruto.Repository.Base
{

    public class DbContextFactory : IDbContextFactory, IDisposable
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 主库的连接实例
        /// </summary>
        private ConcurrentDictionary<Type, DbContext> _masterDbContexts { get; set; }
        /// <summary>
        /// 从库的连接实例
        /// </summary>
        private ConcurrentDictionary<Type, DbContext> _slaveDbContexts { get; set; }

        public DbContextFactory(IServiceProvider _serviceProvider)
        {
            _masterDbContexts = new ConcurrentDictionary<Type, DbContext>();
            _slaveDbContexts = new ConcurrentDictionary<Type, DbContext>();
            serviceProvider = _serviceProvider;
        }
        /// <summary>
        /// 设置上下文
        /// </summary>
        /// <param name="DbContextType"></param>
        /// <param name="dbContext"></param>
        public void Set(Type DbContextType, DbContext dbContext)
        {
            if (DbContextType == null)
                throw new ArgumentNullException(nameof(DbContextType));
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _masterDbContexts.AddOrUpdate(DbContextType, dbContext, (k, oldvalue) => dbContext);
            //注入从库的连接
            var nowEFOption = GetEfOption(DbContextType);
            if (nowEFOption != null && nowEFOption.IsOpenMasterSlave)
            {
                //获取从库的上下文
                var slaveDbContext = serviceProvider.GetRequiredService(nowEFOption.SlaveDbContextType) as DbContext;
                //加入到集合
                _slaveDbContexts.AddOrUpdate(DbContextType, slaveDbContext, (k, oldvalue) => slaveDbContext);
            }
        }
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <param name="DbContextType"></param>
        /// <returns></returns>
        public DbContext GetMaster<TDbContext>() where TDbContext : DbContext => _masterDbContexts[typeof(TDbContext)];
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <param name="DbContextType"></param>
        /// <returns></returns>
        public DbContext GetSlave<TDbContext>() where TDbContext : DbContext => _slaveDbContexts[typeof(TDbContext)];

        /// <summary>
        /// 获取所有的激活的主库上下文类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllMasterType() => _masterDbContexts?.Select(a => a.Key);
        /// <summary>
        /// 获取efoption
        /// </summary>
        /// <param name="DbContextType"></param>
        /// <returns></returns>
        private EFOptions GetEfOption(Type DbContextType)
        {
            return serviceProvider.GetService(MergeNamedType.Get(DbContextType.Name)) as EFOptions;
        }
        void IDisposable.Dispose()
        {
            //释放资源
            if (_masterDbContexts != null && _masterDbContexts.Count > 0)
            {
                foreach (var item in _masterDbContexts)
                {
                    item.Value?.Dispose();
                }
            }
            _masterDbContexts?.Clear();
            _masterDbContexts = null;

            //释放资源
            if (_slaveDbContexts != null && _slaveDbContexts.Count > 0)
            {
                foreach (var item in _slaveDbContexts)
                {
                    item.Value?.Dispose();
                }
            }
            _slaveDbContexts?.Clear();
            _slaveDbContexts = null;

            GC.SuppressFinalize(this);
        }
    }
}
