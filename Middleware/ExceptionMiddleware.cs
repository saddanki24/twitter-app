using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace TwitterStatistics.Middleware
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(builder =>
              {
                  builder.Run(async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.ContentType = "application/json";

                      var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                      if (contextFeature != null)
                      {
                          var exception = contextFeature.Error;
                          var isDev = app.Environment.IsDevelopment();
                          await context.Response.WriteAsync(JsonSerializer.Serialize(
                              new ProblemDetails
                              {
                                  Type = exception.GetType().Name,
                                  Status = (int)HttpStatusCode.InternalServerError,
                                  Instance = contextFeature?.Path,
                                  Title = isDev ? $"{exception.Message}" : "An error occurred.",
                                  Detail = isDev ? exception.StackTrace : null
                              }));                         
                      }
                  });
              });
        }
    }
}
