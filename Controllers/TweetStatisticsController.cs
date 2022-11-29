using Microsoft.AspNetCore.Mvc;
using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetStatisticsController : ControllerBase
    {
        private readonly ILogger<TweetStatisticsController> _logger;
        private readonly ITweetStatisticsService _tweetStatisticsService;

        public TweetStatisticsController(ITweetStatisticsService tweetStatisticsService, ILogger<TweetStatisticsController> logger)
        {
            _tweetStatisticsService = tweetStatisticsService;
            _logger = logger;
        }

        [HttpGet("GetTop10HashTags")]
        public IEnumerable<string> GetTop10HashTags()
        {
            return _tweetStatisticsService.GetTop10HashTags();
        }


        [HttpGet("GetTweetCount")]
        public long GetTweetCount()
        {
            return _tweetStatisticsService.GetTweetCount();
        }
    }
}