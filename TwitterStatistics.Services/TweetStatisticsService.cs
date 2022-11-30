using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Services
{
    /// <inheritdoc />
    public class TweetStatisticsService : ITweetStatisticsService
    {
        private readonly ITweetCacheService _tweetCacheService;     
        public TweetStatisticsService(ITweetCacheService tweetCacheService)
        {
            _tweetCacheService = tweetCacheService;          
        }
        /// <inheritdoc />
        public IEnumerable<string> GetTop10HashTags()
        {
            return _tweetCacheService.HashTags.OrderByDescending(p => p.Value).Select(p => p.Key).Take(10);
        }
        /// <inheritdoc />
        public long GetTweetCount()
        {
            return _tweetCacheService.TweetCount;
        }
    }
}