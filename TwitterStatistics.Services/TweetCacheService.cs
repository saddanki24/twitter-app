
using System.Collections.Concurrent;
using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Services
{
    /// <inheritdoc />
    public class TweetCacheService : ITweetCacheService
    {      
        private long _tweetCount = 0;
        private readonly ConcurrentDictionary<string, int> _hashTags = new();

        /// <inheritdoc />
        public long TweetCount
        {
            get
            {
                return _tweetCount;
            }
        }
        /// <inheritdoc />
        public IReadOnlyDictionary<string, int> HashTags
        {
            get
            {
                return _hashTags;
            }
        }
        /// <inheritdoc />
        public long IncrementTweetCount()
        {
            return Interlocked.Increment(ref _tweetCount);
        }
        /// <inheritdoc />
        public int IncrementHashTagCount(string hashTag, int count)
        {
           return _hashTags.AddOrUpdate(hashTag, count, (key, oldValue) => oldValue + count);
        }       
    }
}
