using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Naruto.Infrastructure.Exceptions
{
    /// <summary>
    /// 客户段输入一场
    /// </summary>
    public class NoFoundException : HttpStatusExcetion
    {
        public NoFoundException(string message) : base(message) {

        }
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}
