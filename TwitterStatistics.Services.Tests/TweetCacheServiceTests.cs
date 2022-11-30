
using NUnit.Framework;

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

        [Test]
        public void Clear_Test()
        {
            _tweetCacheService.IncrementTweetCount();
            _tweetCacheService.IncrementTweetCount();
            _tweetCacheService.IncrementHashTagCount("test1", 1);
            _tweetCacheService.IncrementHashTagCount("test1", 1);
            _tweetCacheService.Clear();
            Assert.Multiple(() =>
            {
                Assert.That(_tweetCacheService.HashTags.Count(), Is.EqualTo(0));
                Assert.That(_tweetCacheService.TweetCount, Is.EqualTo(0));
            });
        }
    }
}