using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using intro.Models;
using intro.Models.Posts;
using intro.ViewModels.Posts;
using Microsoft.EntityFrameworkCore;

namespace intro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostsContext _postsContext;

        public HomeController(ILogger<HomeController> logger, PostsContext context)
        {
            _logger = logger;
            _postsContext = context;
        }

        public IActionResult Index()
        {
            ViewBag.root = Request.PathBase.ToString();
            return View();
        }

        [Route("/Posts/{id}")]
        public IActionResult Posts(ulong id)
        {

            try{
                ViewBag.root = Request.PathBase.ToString();
                var post = _postsContext.Posts.Where(x => x.Id == id).First();
                post.PCount++;
                _postsContext.SaveChanges();
                var ret = new Post();
                ret.id = post.Id;
                ret.name = post.PName;
                ret.photo = GetDataURL(post.PPhoto);
                ret.count = post.PCount;
                ret.content = post.PContent;
                ret.created_time = post.Created.Value;
                ret.update_time = post.Modified.Value;
                ret.tags = _postsContext.PostTagList.Include(x => x.T).Where(x => x.PId == ret.id).Select(x => new ViewModels.Posts.Tag(){ t_id = x.TId, name = x.T.TName}).ToList();
                return View(ret);
            }
            catch
            {
                return RedirectToAction("Index");

            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Forbidden()
        {
            Response.StatusCode = (int)(System.Net.HttpStatusCode.Forbidden);
            return View();
        }
        private static string GetDataURL(byte[] imgFile)
        {
            var b64String = Convert.ToBase64String(imgFile);
            var dataUrl = "data:image/jpeg;base64," + b64String;
            return dataUrl;
        }
    }
}
