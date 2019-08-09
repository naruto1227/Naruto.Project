using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace Fate.Common.Repository.Interceptor
{
    public class EFDiagnosticListener : IObserver<DiagnosticListener>
    {
        private IServiceProvider _serviceProvider;
        public EFDiagnosticListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(DiagnosticListener value)
        {
            //监听拦截的为EF上下文的事件
            if (value.Name == DbLoggerCategory.Name)
            {
                value.Subscribe(_serviceProvider.GetService(typeof(EFCommandInterceptor)) as EFCommandInterceptor);
            }
        }
    }
}
