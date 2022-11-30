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

        [HttpGet("GetTop10HashTags")]
        public ActionResult<IEnumerable<string>> GetTop10HashTags()
        {
            return Ok(_tweetStatisticsService.GetTop10HashTags());
        }


        [HttpGet("GetTweetCount")]
        public ActionResult<long> GetTweetCount()
        {
            return Ok(_tweetStatisticsService.GetTweetCount());
        }
    }
}