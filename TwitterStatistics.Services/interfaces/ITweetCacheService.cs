namespace TwitterStatistics.Services.interfaces
{
    /// <summary>
    /// caching service to persist tweet statistics
    /// </summary>
    public interface ITweetCacheService
    {
        /// <summary>
        /// tweets downloaded
        /// </summary>
        long TweetCount { get; }
        /// <summary>
        /// hashtags with number of appearences
        /// </summary>
        IReadOnlyDictionary<string, int> HashTags { get; }
        /// <summary>
        /// Increment tweet count by 1
        /// </summary>
        /// <returns></returns>
        long IncrementTweetCount();
        /// <summary>
        /// adds hash tag with count if not exists
        /// increments hash tag by count if exists
        /// </summary>
        /// <param name="hashTag">hashtag to add/update</param>
        /// <param name="count">hashtag count</param>
        /// <returns></returns>
        int IncrementHashTagCount(string hashTag, int count);
    }
}
