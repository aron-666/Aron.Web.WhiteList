using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using intro.Models;

namespace intro.Controllers
{
    public class IntroController : Controller
    {
        private readonly ILogger<IntroController> _logger;

        public IntroController(ILogger<IntroController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }

    }
}
