namespace TwitterStatistics.Services.interfaces
{
    public interface ITweetStatisticsService
    {      
        IEnumerable<string> GetTop10HashTags();
        long GetTweetCount();
    }
}
