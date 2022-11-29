using Polly;
using Polly.Extensions.Http;
using TwitterStatistics.Models;
using TwitterStatistics.Services;
using TwitterStatistics.Services.interfaces;
using TwitterStatistics.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

IConfigurationRoot config = builder.Configuration
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .Build();

builder.Services.Configure<TwitterSettings>(config.GetRequiredSection(TwitterSettings.Twitter));

// register services
builder.Services.AddScoped<ITweetStatisticsService, TweetStatisticsService>();
builder.Services.AddSingleton<ITweetStreamService, TweetStreamService>();
builder.Services.AddSingleton<ITweetCacheService, TweetCacheService>();

// add http client services.
builder.Services.AddHttpClient<ITweetStreamService, TweetStreamService>(client =>
{
    var twitterConfig = builder.Configuration.GetSection(TwitterSettings.Twitter);
    client.BaseAddress = new Uri(twitterConfig["BaseUrl"]);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {twitterConfig["BearerToken"]}");
})
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
    .AddPolicyHandler(GetRetryPolicy());

// register hosted service
builder.Services.AddHostedService<TweetWorkerService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}