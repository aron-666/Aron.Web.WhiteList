using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using intro.Models;
using System.Net.Http;
using intro.Models.Posts;

namespace intro.Controllers
{
    [Route("intro/[action]")]
    public class IntroApiController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly PostsContext _postsContext;

        public IntroApiController(ILogger<IntroController> logger, PostsContext context)
        {
            _logger = logger;
            _postsContext = context;
        }


        public HttpResponseMessage GetPosts()
        {
            
            ViewModels.Intro.PostList list = new ViewModels.Intro.PostList();
            var data = _postsContext
                    .Posts
                    .OrderByDescending(x => x.Created)
                    .Select(x => new ViewModels.Intro.Post(){
                        Subject = x.PName,
                        Content = $"瀏覽次數:{x.PCount}",
                        Url = "~/Posts/" + x.Id,
                        Time = x.Created.Value,
                        Image = GetDataURL(x.PPhoto)
                    })
                    .ToList();
            list.Posts = data;

            // ViewModels.Intro.Post p = new ViewModels.Intro.Post();
            // p.Url = "#";
            // p.Image = "~/images/aaa.jpg";
            // p.Time = new DateTime(2018, 1, 20);
            // p.Subject = "%%%%%";
            // p.Content = "%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%";
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(20);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(15);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(17);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(8);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(12);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(55);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(62);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(12);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(5);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(2);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(32);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(14);
            // list.Posts.Add(p);
            // p = p.Clone();
            // p.Time += TimeSpan.FromDays(25);
            // list.Posts.Add(p);
            // list.Posts = list.Posts.OrderByDescending(x => x.Time).ToList();
            return this.CreateResponse(list);
        }

        private static string GetDataURL(byte[] imgFile)
        {
            var b64String = Convert.ToBase64String(imgFile);
            var dataUrl = "data:image/jpeg;base64," + b64String;
            return dataUrl;
        }
    }
}
