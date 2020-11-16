using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace intro.Services
{
    public class LogReqMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public LogReqMiddleware(ILogger<LogReqMiddleware> logger, RequestDelegate next)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            logger.LogInformation("source: {0}, route: {1}", context.Connection.RemoteIpAddress.ToString(), context.Request.Path);
            await next(context);
        }

        
    }
}