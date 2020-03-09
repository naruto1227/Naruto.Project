using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fate.Infrastructure.Id4.MongoDB.Options;
using Fate.Infrastructure.MongoDB.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Fate.Infrastructure.Id4.MongoDB.Tokens
{
    /// <summary>
    /// Helper to perodically cleanup expired persisted grants.
    /// </summary>
    public class TokenCleanup
    {
        private readonly ILogger<TokenCleanup> _logger;
        private readonly TokenCleanupOptions _options;
        private readonly IServiceProvider _serviceProvider;

        private CancellationTokenSource _source;

        private TimeSpan CleanupInterval => TimeSpan.FromSeconds(_options.TokenCleanupInterval);

        public TokenCleanup(IServiceProvider serviceProvider, ILogger<TokenCleanup> logger, TokenCleanupOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            if (_options.TokenCleanupInterval < 1) throw new ArgumentException("Token cleanup interval must be at least 1 second");
            if (_options.TokenCleanupBatchSize < 1) throw new ArgumentException("Token cleanup batch size interval must be at least 1");

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Starts the token cleanup polling.
        /// </summary>
        public void Start()
        {
            Start(CancellationToken.None);
        }

        /// <summary>
        /// Starts the token cleanup polling.
        /// </summary>
        public void Start(CancellationToken cancellationToken)
        {
            if (_source != null) throw new InvalidOperationException("Already started. Call Stop first.");

            _logger.LogDebug("Starting grant removal");

            _source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Factory.StartNew(() => StartInternalAsync(_source.Token));
        }

        /// <summary>
        /// Stops the token cleanup polling.
        /// </summary>
        public void Stop()
        {
            if (_source == null) throw new InvalidOperationException("Not started. Call Start first.");

            _logger.LogDebug("Stopping grant removal");

            _source.Cancel();
            _source = null;
        }


        private async Task StartInternalAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("CancellationRequested. Exiting.");
                    break;
                }

                try
                {
                    await Task.Delay(CleanupInterval, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogDebug("TaskCanceledException. Exiting.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Task.Delay exception: {0}. Exiting.", ex.Message);
                    break;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("CancellationRequested. Exiting.");
                    break;
                }

                await RemoveExpiredGrantsAsync();
            }
        }

        /// <summary>
        /// 清理过期的授权信息
        /// </summary>
        /// <returns></returns>
        public async Task RemoveExpiredGrantsAsync()
        {
            try
            {
                _logger.LogTrace("Querying for expired grants to remove");

                var found = int.MaxValue;

                using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    //获取仓储
                    var mongoDBRepository = serviceScope.ServiceProvider.GetService<IMongoRepository<IdentityServerMongoContext>>();
                    //获取通知的接口
                    var tokenCleanupNotification = serviceScope.ServiceProvider.GetService<IOperationalStoreNotification>();
                    //如果没有实现通知接口的话 就执行删除过期的数据
                    if (tokenCleanupNotification == null)
                    {
                        var result = await mongoDBRepository.Command<Entities.PersistedGrant>()
                            .BulkDeleteAsync(x => x.Expiration < DateTime.UtcNow)
                            .ConfigureAwait(false);

                        _logger.LogInformation("Cleared {tokenCount} tokens", result);
                        return;
                    }
                    //如果有实现 验证条数 当found小于TokenCleanupBatchSize
                    //的时候代表此数据已全部删除不再执行
                    while (found >= _options.TokenCleanupBatchSize)
                    {
                        //获取过期的条数
                        var expired = await mongoDBRepository.Query<Entities.PersistedGrant>().AsQueryable()
                            .Where(x => x.Expiration < DateTime.UtcNow)
                            .Take(_options.TokenCleanupBatchSize)
                            .ToListAsync()
                            .ConfigureAwait(false);
                        //获取删除的总条数
                        found = expired.Count;
                        _logger.LogInformation("Clearing {tokenCount} tokens", found);

                        //如果有数据就删除
                        if (expired.Count > 0)
                        {
                            var ids = expired.Select(x => x.Id).ToArray();
                            await mongoDBRepository.Command<Entities.PersistedGrant>().BulkDeleteAsync(x => ids.Contains(x.Id)).ConfigureAwait(false);

                            //通知接口处理 已经删除的数据
                            await tokenCleanupNotification.PersistedGrantsRemovedAsync(expired);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception removing expired grants: {exception}", ex.Message);
            }
        }
    }
}