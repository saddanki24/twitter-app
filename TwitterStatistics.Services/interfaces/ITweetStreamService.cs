namespace TwitterStatistics.Services.interfaces
{
    /// <summary>
    /// service to handle tweet volume data
    /// </summary>
    public interface ITweetStreamService
    {
        /// <summary>
        /// fetch tweets from volume stream
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>task</returns>
        Task FetchTweetsAsync(CancellationToken cancellationToken);      
    }
}
