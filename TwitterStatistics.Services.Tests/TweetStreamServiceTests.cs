using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using TwitterStatistics.Models;
using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Services.Tests
{
    public class TweetStreamServiceTests
    {      
        private Mock<ITweetCacheService> _tweetCacheServiceMock;
        private TweetStreamService _tweetStreamService;
        private Mock<IOptions<TwitterSettings>> _mockOptions;
        private Mock<HttpMessageHandler> _handlerMock;
        private const string BaseAddress = "https://test.com/";

        [SetUp]
        public void Setup()
        {
            _tweetCacheServiceMock = new Mock<ITweetCacheService>();
            _mockOptions = new Mock<IOptions<TwitterSettings>>();
            _mockOptions.Setup(p => p.Value).Returns(new TwitterSettings());
            // 
            // ARRANGE
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _handlerMock
               .Protected()             
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("{\"data\":{\"entities\":{\"hashTags\":[{\"tag\":\"test1\"}]},\"id\":\"123\",\"text\":\"test1\"}}"),
               })
               .Verifiable();
            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(BaseAddress)
            };

            _tweetStreamService = new TweetStreamService(httpClient, _tweetCacheServiceMock.Object, _mockOptions.Object);
        }

        [Test]
        public async Task FetchTweets_Test()
        {
            var expectedUri = new Uri($"{BaseAddress}2/tweets/sample/stream?tweet.fields=entities");

            await _tweetStreamService.FetchTweetsAsync(CancellationToken.None);

            // verify
            _handlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => 
                req.Method == HttpMethod.Get && 
                req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>());

            _tweetCacheServiceMock.Verify(p => p.IncrementTweetCount(), Times.Exactly(1));
            _tweetCacheServiceMock.Verify(p => p.IncrementHashTagCount("test1", 1), Times.Exactly(1));
        }       
    }
}