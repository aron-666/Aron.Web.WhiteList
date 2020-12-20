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
using System.Collections.Generic;
using intro.ViewModels;

namespace intro.Controllers
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }


        public HttpResponseMessage Get()
        {
            _logger.LogInformation($"Route: {this.Request.Path}, Address: {this.Request.HttpContext.Connection.RemoteIpAddress.ToString()}");
            List<MsServer> servers = new List<MsServer>();
            servers.Add(new MsServer(){ Name= "Aron Test Server", Address = "aronhome.in", Port = 8484, Site = "https://aronhome.in"});

            return this.CreateResponse(servers);
        }
    }
}