
namespace TwitterStatistics.Services.Tests
{
    [TestFixture]
    public class TweetCacheServiceTests
    {
        private TweetCacheService _tweetCacheService;       
        [SetUp]
        public void Setup()
        {
            _tweetCacheService = new TweetCacheService();          
        }

        [TestCase("Tag1", 1)]
        [TestCase("Tag1", 2)]
        [TestCase("Tag2", 1)]
        [TestCase("Tag2", 3)]
        [TestCase("Tag1", 1)]
        [TestCase("Tag3", 1)]
        public void IncrementHashTag_Test(string tag, int count)
        {
            var initialTagCount = _tweetCacheService.HashTags.GetValueOrDefault(tag);
           _tweetCacheService.IncrementHashTagCount(tag, count);           
            Assert.That(count + initialTagCount, Is.EqualTo(_tweetCacheService.HashTags.GetValueOrDefault(tag)));
        }

        [TestCase()]
        [TestCase()]       
        [TestCase()]       
        public void IncrementTweetCount_Test()
        {
            var initialTweetCount = _tweetCacheService.HashTags.Count;
            _tweetCacheService.IncrementTweetCount();
            Assert.That(initialTweetCount + 1, Is.EqualTo(_tweetCacheService.TweetCount));
        }
    }
}