using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fate.Test.Ocelot
{
    /// <summary>
    /// 创建一个聚合器
    /// </summary>
    public class Test2DefinedAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<DownstreamContext> responses)
        {
            var res = await responses[0].DownstreamResponse.Content.ReadAsStringAsync();
            var content = $"'hello:'{res}";
            var headers = responses.SelectMany(x => x.DownstreamResponse.Headers).ToList();
            return new DownstreamResponse(new StringContent(content), HttpStatusCode.OK, headers, "hi");
        }
    }
}
