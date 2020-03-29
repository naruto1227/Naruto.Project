using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Naruto.Configuration.Management
{
    /// <summary>
    /// 张海波
    /// 2019-11-3
    /// 接口请求的授权过滤器
    /// </summary>
    public interface IRequestAuthorizationFilters
    {
        Task<bool> AuthorizationAsync(RequestContext context);
    }
}
