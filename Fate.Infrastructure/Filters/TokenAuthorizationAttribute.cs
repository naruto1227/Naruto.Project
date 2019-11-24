using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Fate.Infrastructure.Filters
{
    public class TokenAuthorizationAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
