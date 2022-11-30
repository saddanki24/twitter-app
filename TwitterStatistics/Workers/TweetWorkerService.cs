using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Workers
{
    /// <summary>
    /// worker service to fetch tweet in background
    /// </summary>
    public class TweetWorkerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TweetWorkerService> _logger;

        public TweetWorkerService(IServiceProvider serviceProvider, ILogger<TweetWorkerService> logger)
        {
            (_serviceProvider, _logger) = (serviceProvider, logger);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _serviceProvider.CreateScope();
                    ITweetStreamService tweetStreamService =
                        scope.ServiceProvider.GetRequiredService<ITweetStreamService>();
                    await tweetStreamService.FetchTweetsAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "an error occured");
                    throw;
                }
            }
        }
    }
}
