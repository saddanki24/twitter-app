namespace TwitterStatistics.Services.interfaces
{
    public interface ITweetCacheService
    {
        long TweetCount { get; }
        IReadOnlyDictionary<string, int> HashTags { get; }
        long IncrementTweetCount();
        int IncrementHashTagCount(string hashTag, int count);
    }
}
