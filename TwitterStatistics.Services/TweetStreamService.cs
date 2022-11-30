using Microsoft.Extensions.Options;
using System.Text.Json;
using TwitterStatistics.Models;
using TwitterStatistics.Services.interfaces;

namespace TwitterStatistics.Services
{
    /// <inheritdoc />
    public class TweetStreamService : ITweetStreamService
    {
        private readonly ITweetCacheService _cacheService;
        private readonly HttpClient _httpClient;
        private readonly ParallelOptions _parallelOptions;
        private readonly JsonSerializerOptions _jsonSerializeOptions;

        public TweetStreamService(HttpClient httpClient, ITweetCacheService cacheService, IOptions<TwitterSettings> options)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
            _parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = options.Value.MaxParallelism
            };
            _jsonSerializeOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Fetch tweets from twitter stream and tracks hashTags and tweet count.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task FetchTweetsAsync(CancellationToken cancellationToken)
        {
            await Parallel.ForEachAsync(GetTwitterStreamDataInternal(), _parallelOptions, async (tweet, cancellationToken) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return;
                }
                await ProcessTweetStatisticsInternalAsync(tweet).ConfigureAwait(false);
            });
        }

        private Task ProcessTweetStatisticsInternalAsync(Tweet? tweet)
        {
            if (tweet != null)
            {
                _cacheService.IncrementTweetCount();

                var hashTags = tweet.Data?.Entities?.HashTags?
                   .GroupBy(p => p.Tag)
                   ?.Select(p => new { Tag = p.Key, TagCount = p.Count() })
                   ?.ToArray();

                if (hashTags != null)
                {
                    foreach (var hashTag in hashTags)
                    {
                        _cacheService.IncrementHashTagCount(hashTag.Tag, hashTag.TagCount);
                    }
                }
            }
            return Task.CompletedTask;
        }

        private async IAsyncEnumerable<Tweet?> GetTwitterStreamDataInternal()
        {
            var streamUrlPath = "/2/tweets/sample/stream?tweet.fields=entities";
            using var response = await _httpClient.GetAsync(streamUrlPath,
                HttpCompletionOption.ResponseHeadersRead);              
            response.EnsureSuccessStatusCode();
            using var body = await response.Content.ReadAsStreamAsync();              
            using var reader = new StreamReader(body);
            while (!reader.EndOfStream)
            {
                var content = await reader.ReadLineAsync().ConfigureAwait(false);
                if (!string.IsNullOrEmpty(content))
                {
                    var tweet = JsonSerializer.Deserialize<Tweet>(content, _jsonSerializeOptions);
                    yield return tweet;
                }
            }
        }
    }
}