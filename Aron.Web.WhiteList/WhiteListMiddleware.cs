using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aron.Web.WhiteList
{
    public class WhiteListMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private readonly Action<HttpContext> _onKill;
        //private WhiteListContext _whiteListContext;

        public WhiteListMiddleware(ILogger<WhiteListMiddleware> logger, RequestDelegate next, Action<HttpContext> onKill = null)
        {
            this.next = next;
            this.logger = logger;
            _onKill = onKill;
        }

        public async Task Invoke(HttpContext context, IWhiteListService whiteListService)
        {
            if (!whiteListService.Check(context.Request.Path, context.Connection.RemoteIpAddress))
            {
                
                _onKill(context);
                return;
            }
            await next(context);
        }

        
    }

    public static class WhiteListEx
    {
        public static void UseDefaultWhiteListMiddleWare(this IApplicationBuilder app, Action<HttpContext> onkill)
        {
            app.UseMiddleware<WhiteListMiddleware>(onkill);
        }

        
    }
}
