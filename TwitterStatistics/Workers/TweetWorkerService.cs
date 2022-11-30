using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Workers
{
    /// <summary>
    /// worker service to fetch tweet in background
    /// </summary>
    public class TweetWorkerService : BackgroundService
    {
        private readonly ITweetStreamService _tweetStreamService;

        public TweetWorkerService(ITweetStreamService tweetStreamService) {
            _tweetStreamService = tweetStreamService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {           
            await _tweetStreamService.FetchTweetsAsync(cancellationToken);
        }
    }
}
