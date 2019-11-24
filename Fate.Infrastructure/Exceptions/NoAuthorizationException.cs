using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Fate.Infrastructure.Exceptions
{
    /// <summary>
    /// 未授权的
    /// </summary>
   public class NoAuthorizationException:HttpStatusExcetion
    {
        public NoAuthorizationException(string message):base(message) {

        }
        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    }
}
