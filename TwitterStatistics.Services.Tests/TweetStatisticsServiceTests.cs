using Moq;
using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Services.Tests
{
    public class TweetStatisticsServiceTests
    {
        private const string Tag1 = "Tag1";
        private const string Tag2 = "Tag2";
        private const string Tag3 = "Tag3";
        private Mock<ITweetCacheService> _tweetCacheServiceMock;
        private TweetStatisticsService _tweetStatisticsService;
        [SetUp]
        public void Setup()
        {
            _tweetCacheServiceMock = new Mock<ITweetCacheService>();
            _tweetStatisticsService = new TweetStatisticsService(_tweetCacheServiceMock.Object);
            _tweetCacheServiceMock.SetupGet(p => p.HashTags).Returns(GetHashTags());
            _tweetCacheServiceMock.SetupGet(p => p.TweetCount).Returns(It.IsAny<int>());
        }

        private static IReadOnlyDictionary<string, int> GetHashTags()
        {
            return new Dictionary<string, int>() { 
                { Tag1, 2 },                
                { Tag2, 1 },           
                { Tag3, 3 }
            };
        }

        [Test]
        public void GetTop10HashTagsTest()
        {
            var hashTags = _tweetStatisticsService.GetTop10HashTags().ToArray();
            Assert.Multiple(() =>
            {
                Assert.That(hashTags[0], Is.EqualTo(Tag3));
                Assert.That(hashTags[1], Is.EqualTo(Tag1));
                Assert.That(hashTags[2], Is.EqualTo(Tag2));
            });
            _tweetCacheServiceMock.VerifyGet(p => p.HashTags, Times.Once);
        }

        [Test]
        public void GetTweetCountTest()
        {
            var count = _tweetStatisticsService.GetTweetCount();           
            Assert.That(count, Is.AtLeast(0));
            _tweetCacheServiceMock.VerifyGet(p => p.TweetCount, Times.Once);
        }
    }
}