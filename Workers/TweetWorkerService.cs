using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Workers
{
    public class TweetWorkerService : BackgroundService
    {
        private readonly ITweetStreamService _tweetStreamService;

        public TweetWorkerService(ITweetStreamService tweetStreamService) {
            _tweetStreamService = tweetStreamService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // todo handle retry
            await _tweetStreamService.FetchTweetsAsync(cancellationToken);
        }
    }
}
