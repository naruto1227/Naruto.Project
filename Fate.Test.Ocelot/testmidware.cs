using Fate.Common.OcelotStore.EFCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fate.Test.Ocelot
{
    public class testmidware
    {

        public testmidware(RequestDelegate next)
        {

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            
        }
    }
}
