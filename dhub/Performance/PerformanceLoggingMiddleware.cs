using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dhub.Performance
{

    public class PerformanceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceLoggingMiddleware> _logger;

        public PerformanceLoggingMiddleware(RequestDelegate next, ILogger<PerformanceLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            var elapsedMilliseconds = stopwatch.Elapsed.TotalSeconds;
            _logger.LogInformation("Request URL: {Url}, Elapsed Time: {ElapsedTime} seconds", context.Request.Path, elapsedMilliseconds);
        }
    }

}
