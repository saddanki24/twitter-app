
using System.Collections.Concurrent;
using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Services
{
    public class TweetCacheService : ITweetCacheService
    {      
        private long _tweetCount = 0;
        private readonly ConcurrentDictionary<string, int> _hashTags = new();       

        public long TweetCount
        {
            get
            {
                return _tweetCount;
            }
        }
        public IReadOnlyDictionary<string, int> HashTags
        {
            get
            {
                return _hashTags;
            }
        }

        public long IncrementTweetCount()
        {
            return Interlocked.Increment(ref _tweetCount);
        }

        public int IncrementHashTagCount(string hashTag, int count)
        {
           return _hashTags.AddOrUpdate(hashTag, count, (key, oldValue) => oldValue + count);
        }       
    }
}
