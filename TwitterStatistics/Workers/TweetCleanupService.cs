using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Workers
{
    /// <summary>
    /// worker service to clean up tweet statistics background
    /// </summary>
    public class TweetCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;      

        public TweetCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // clears tweet statistics every x minute's to prevent memory buildup
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {           
            TimeSpan interval = TimeSpan.FromMinutes(5);           
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
