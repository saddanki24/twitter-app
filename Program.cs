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
