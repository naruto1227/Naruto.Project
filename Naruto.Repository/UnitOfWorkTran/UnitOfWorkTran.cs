using Naruto.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Naruto.Repository.UnitOfWork
{
    public class UnitOfWorkTran : IUnitOfWorkTran
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDbContextFactory dbContextFactory;

        public UnitOfWorkTran(IServiceProvider _serviceProvider, IDbContextFactory _dbContextFactory)
        {
            serviceProvider = _serviceProvider;
            dbContextFactory = _dbContextFactory;
        }

        public void BeginTransaction() => ExecTransaction(unitOfWork => unitOfWork.BeginTransaction());

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ExecTransactionAsync(unitOfWork => unitOfWork.BeginTransactionAsync(cancellationToken)).ConfigureAwait(false);
        }

        public void CommitTransaction() => ExecTransaction(unitOfWork => unitOfWork.CommitTransaction());

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ExecTransactionAsync(unitOfWork => unitOfWork.CommitTransactionAsync(cancellationToken)).ConfigureAwait(false);
        }

        public void RollBackTransaction()=>ExecTransaction(unitOfWork => unitOfWork.RollBackTransaction());

        public async Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ExecTransactionAsync(unitOfWork => unitOfWork.RollBackTransactionAsync(cancellationToken)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取激活的上下文类型
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Type> GetActivateDbContextType()
        {
            var masterTypes = dbContextFactory.GetAllMasterType();
            if (masterTypes == null || masterTypes.Count() <= 0)
                throw new InvalidOperationException("当前无激活的上下文");
            return masterTypes;
        }

        private void ExecTransaction(Action<IUnitOfWork> exec)
        {
            //获取激活的上下文
            var masterTypes = GetActivateDbContextType();
            foreach (var item in masterTypes)
            {
                //获取工作单元
                var unitOfWork = serviceProvider.GetService(typeof(IUnitOfWork<>).MakeGenericType(item)) as IUnitOfWork;
                exec(unitOfWork);
            }
        }

        private async Task ExecTransactionAsync(Func<IUnitOfWork, Task> exec)
        {
            //获取激活的上下文
            var masterTypes = GetActivateDbContextType();
            foreach (var item in masterTypes)
            {
                //获取工作单元
                var unitOfWork = serviceProvider.GetService(typeof(IUnitOfWork<>).MakeGenericType(item)) as IUnitOfWork;
                await exec(unitOfWork).ConfigureAwait(false);
            }
        }
    }
}
