using Microsoft.AspNetCore.Mvc;
using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TweetStatisticsController : ControllerBase
    {
        private readonly ITweetStatisticsService _tweetStatisticsService;

        public TweetStatisticsController(ITweetStatisticsService tweetStatisticsService)
        {
            _tweetStatisticsService = tweetStatisticsService;
        }

        /// <summary>
        /// provides top 10 hash tags from the twitter volume stream
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTop10HashTags")]
        public ActionResult<IEnumerable<string>> GetTop10HashTags()
        {
            return Ok(_tweetStatisticsService.GetTop10HashTags());
        }

        /// <summary>
        /// provides the tweets downloaded
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTweetCount")]
        public ActionResult<long> GetTweetCount()
        {
            return Ok(_tweetStatisticsService.GetTweetCount());
        }
    }
}