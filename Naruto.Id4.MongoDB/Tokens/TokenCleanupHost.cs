using System.Threading;
using System.Threading.Tasks;
using Naruto.Id4.MongoDB.Options;
using Microsoft.Extensions.Hosting;

namespace Naruto.Id4.MongoDB.Tokens
{
    internal class TokenCleanupHost : IHostedService
    {
        private readonly TokenCleanup _tokenCleanup;
        private readonly TokenCleanupOptions _options;

        public TokenCleanupHost(TokenCleanup tokenCleanup, TokenCleanupOptions options)
        {
            _tokenCleanup = tokenCleanup;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.EnableTokenCleanup)
            {
                _tokenCleanup.Start(cancellationToken);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_options.EnableTokenCleanup)
            {
                _tokenCleanup.Stop();
            }

            return Task.CompletedTask;
        }
    }
}