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
    [Route("api/[controller]/[action]")]
    public class TwMs113AppInfoController : ControllerBase
    {
        private readonly ILogger _logger;
        public TwMs113AppInfoController(ILogger<TwMs113AppInfoController> logger)
        {
            _logger = logger;
        }

        public HttpResponseMessage GetAsmScriptA()
        {
            if(DateTime.Now <= new DateTime(2021, 1, 15, 23, 59, 59))
            {
                string str = System.IO.File.ReadAllText(System.IO.Path.Combine("Docs", "113new.CT"));
                str = Services.MyAesCryptography.Encrypt("dvsaniewjqornkwomvriwhacucbeubvu", "sjideownvirpownr", str);

                return this.CreateResponse(str);
            }
            else return this.CreateErrorResponse("", "已經過期");

        }

    }
}