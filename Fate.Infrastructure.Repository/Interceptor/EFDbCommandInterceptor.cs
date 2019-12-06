using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Repository.Interceptor
{
    /// <summary>
    /// ef db 拦截器
    /// </summary>
    public class EFDbCommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            //查询总数时替换掉*
            command.CommandText = command.CommandText.Replace("COUNT(*)", "COUNT(1)");
            return base.ReaderExecuting(command, eventData, result);
        }
        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            return base.ReaderExecuted(command, eventData, result);
        }
        public override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }
        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            //查询总数时替换掉*
            command.CommandText = command.CommandText.Replace("COUNT(*)", "COUNT(1)");
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}
