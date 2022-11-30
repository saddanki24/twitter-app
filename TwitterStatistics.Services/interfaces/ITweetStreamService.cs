namespace TwitterStatistics.Services.interfaces
{
    public interface ITweetStreamService
    {
        Task FetchTweetsAsync(CancellationToken cancellationToken);      
    }
}
