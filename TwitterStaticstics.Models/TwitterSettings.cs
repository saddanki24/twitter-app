namespace TwitterStatistics.Models
{
    public class TwitterSettings
    {
        public const string Twitter = "Twitter";
        public string ApiBaseUrl { get; set; } = string.Empty;
        public string BearerToken { get; set; } = string.Empty;
        public int MaxParallelism { get; set; } = 1;
    }
}
