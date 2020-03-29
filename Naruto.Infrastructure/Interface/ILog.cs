using Naruto.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Infrastructure.Interface
{
    /// <summary>
    /// 日志的操作
    /// </summary>
    public interface ILog : ICommonSingleDependency
    {

        Task Info(string message);

        Task Error(string message);

        Task Debug(string message);
        Task Trace(string message);
        Task Fatal(string message);

    }
}
