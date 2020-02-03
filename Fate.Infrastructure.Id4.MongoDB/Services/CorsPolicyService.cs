using System;
using System.Linq;
using System.Threading.Tasks;
using Fate.Infrastructure.MongoDB.Interface;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Entities = Fate.Infrastructure.Id4.Entities;
namespace Fate.Infrastructure.Id4.MongoDB.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CorsPolicyService> _logger;

        public CorsPolicyService(IHttpContextAccessor httpContextAccessor, ILogger<CorsPolicyService> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger;
        }
        /// <summary>
        /// 是否跨域
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            // doing this here and not in the ctor because: https://github.com/aspnet/CORS/issues/105
            var clientRepository = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IMongoRepository<IdentityServerMongoContext>>();

            var origins = await clientRepository.Query<Entities.Client>().AsQueryable().SelectMany(x => x.AllowedCorsOrigins).Select(x => x.Origin).ToListAsync().ConfigureAwait(false);

            var distinctOrigins = origins.Where(x => x != null).Distinct();
            var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

            return isAllowed;
        }
    }
}