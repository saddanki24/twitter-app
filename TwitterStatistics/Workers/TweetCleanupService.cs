using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Workers
{
    /// <summary>
    /// worker service to clean up tweet in background
    /// </summary>
    public class TweetCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;      

        public TweetCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // cleans tweets every one minute to prevent excessive memory buildup
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {           
            TimeSpan interval = TimeSpan.FromMinutes(1);           
            using PeriodicTimer timer = new(interval);
            while (!cancellationToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(cancellationToken))
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                ITweetCacheService tweetCacheService =
                    scope.ServiceProvider.GetRequiredService<ITweetCacheService>();
                tweetCacheService.Clear();
            }
        }
    }
}
