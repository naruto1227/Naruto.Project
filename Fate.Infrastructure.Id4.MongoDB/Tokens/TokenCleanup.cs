using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fate.Infrastructure.Id4.MongoDB.Options;
using Fate.Infrastructure.Mongo.Interface;
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
        /// Method to clear expired persisted grants.
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
                    var mongoDBRepository = serviceScope.ServiceProvider.GetService<IMongoRepository<IdentityServerMongoContext>>();

                    var tokenCleanupNotification = serviceScope.ServiceProvider.GetService<IOperationalStoreNotification>();
                    if (tokenCleanupNotification == null)
                    {
                        var result = await mongoDBRepository.Command<Entities.PersistedGrant>()
                            .DeleteManyAsync(x => x.Expiration < DateTime.UtcNow)
                            .ConfigureAwait(false);

                        _logger.LogInformation("Cleared {tokenCount} tokens", result);
                        return;
                    }

                    while (found >= _options.TokenCleanupBatchSize)
                    {
                        //获取过期的条数
                        var expired = await mongoDBRepository.Query<Entities.PersistedGrant>().AsQueryable()
                            .Where(x => x.Expiration < DateTime.UtcNow)
                            .Take(_options.TokenCleanupBatchSize)
                            .ToListAsync()
                            .ConfigureAwait(false);

                        found = expired.Count;
                        _logger.LogInformation("Clearing {tokenCount} tokens", found);

                        if (expired.Count > 0)
                        {
                            var ids = expired.Select(x => x._id).ToArray();
                            await mongoDBRepository.Command<Entities.PersistedGrant>().DeleteManyAsync(x => ids.Contains(x._id)).ConfigureAwait(false);

                            // notification
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