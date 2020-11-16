using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;

namespace intro.Controllers
{
    [AutoValidateAntiforgeryToken]
    [ResponseCache(NoStore = true)]
    [Authorize]
    public class SiteToolsController : Controller
    {
        private readonly ILogger<SiteToolsController> _logger;

        public SiteToolsController(ILogger<SiteToolsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Ping()
        {

            return View();
        } 


    }
}