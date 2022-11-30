namespace TwitterStatistics.Services.interfaces
{
    /// <summary>
    /// tweet statistics service
    /// </summary>
    public interface ITweetStatisticsService
    {      
        /// <summary>
        /// get top 10 most appeared hash tags
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetTop10HashTags();
        /// <summary>
        /// get total number of tweets downloaded
        /// </summary>
        /// <returns></returns>
        long GetTweetCount();
    }
}
