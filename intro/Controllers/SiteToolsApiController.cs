using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;
using Aron.Web.Api.Models.Message;
using Aron.Http.Tools.HttpMsg;
using intro.ViewModels.ServerTools;
using Microsoft.AspNetCore.Authorization;

namespace intro.Controllers
{
    [Route("SiteTools/[action]")]
    [ApiController]
    [Authorize]
    [ResponseCache(NoStore = true)]
    public class SiteToolsApiController : ControllerBase
    {
        private readonly ILogger<SiteToolsApiController> _logger;

        public SiteToolsApiController(ILogger<SiteToolsApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseMessage<PingResult>), 200)]
        public async Task<HttpResponseMessage> PingApi(string address, int timeout = 10000)
        {
            if(timeout > 20000)
                timeout = 20000;
            else if(timeout < 1)
                timeout = 100;
            try
            {
                IPAddress addr = IPAddress.Parse(address);
                Ping ping = new Ping();
                var res = await ping.SendPingAsync(addr, 10000);

                PingResult res2 = new PingResult();
                res2.Address = addr.ToString();
                res2.Status = res.Status;
                res2.TotalMs = res.RoundtripTime;

                return this.CreateResponse(res2);
            }
            catch(Exception ex)
            {
                _logger.LogError("{0}", ex);
            }
            return null;
        }

    }
}