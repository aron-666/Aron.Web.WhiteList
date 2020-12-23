using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using intro.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using intro.Helpers;
using intro.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using intro.Models.Posts;
using System.IO;
using Microsoft.EntityFrameworkCore;
using intro.ViewModels.Posts;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace intro.Controllers
{
    [Route("api/[controller]/[action]")]
    
    public class AdminApiController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly JwtHelpers jwt;
        private readonly UserManager<IdentityUser> userManager;
        private readonly PostsContext _postsContext;
        public AdminApiController(ILogger<AdminApiController> logger, UserManager<IdentityUser> userManager, JwtHelpers jwt, PostsContext context)
        {
            _logger = logger;
            this.jwt = jwt;
            this.userManager = userManager;
            _postsContext = context;
        }

        /// <summary>
        /// A02
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> LoginAsync([FromBody]ViewModels.Identity.Login login)
        {

            if (await ValidateUser(login))
            {
                var obj = new {access_token = jwt.GenerateToken(login.email, 60*24*10)};
                return this.CreateResponse(obj);
            }
            else
            {
                return this.CreateErrorResponse("", "帳號或密碼錯誤");
            }
        }

        /// <summary>
        /// A03
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        public HttpResponseMessage Logout()
        {
            return this.CreateResponse("登出成功");
        }

        [Route("/api/userapi/image/{id}")]
        [HttpGet]
        public IActionResult Image(ulong id)
        {
            var img =_postsContext.PostImage.Where(x => x.Id == id).First();
            return File(img.IImage, "image/jpeg");

        }
        /// <summary>
        /// A04
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public HttpResponseMessage Profile()
        {

            return this.CreateResponse(new { Username = User.Identity.Name.Split('@')[0], Email = User.Identity.Name});
        }

        /// <summary>
        /// A05
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("/api/[controller]/posts/[action]")]
        public UploadImageRes Uploadimage(List<IFormFile> upload)
        {
            List<string> urls = new List<string>();
            foreach(var i in upload)
            {
                var s = i.OpenReadStream();
                
                byte[] buffer = new byte[i.Length];
                s.Read(buffer, 0, (int)i.Length);
                var img = new PostImage{ UId = userManager.FindByEmailAsync(User.Identity.Name).GetAwaiter().GetResult().Id, IImage = buffer };
                _postsContext.PostImage.Add(img);
                _postsContext.SaveChanges();
                urls.Add("https://aronhome.in" + Request.PathBase + "/api/userapi/image/" + img.Id);
            }
            return new UploadImageRes { uploaded = true, url = urls };
        }
        /// <summary> 
        /// A06
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("/api/[controller]/posts/[action]")]
        public HttpResponseMessage Post([FromBody]CreatePost post)
        {
            string base64 = post.photo.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64);
            byte[] buffer = null;
            using(var stream = new MemoryStream(bytes))
            {
                var img = System.Drawing.Image.FromStream(stream);
                using(var s2 = new MemoryStream())
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, (long)70);
	                myEncoderParameters.Param[0] = myEncoderParameter;
                    img.Save(s2, jpgEncoder, myEncoderParameters);
                    buffer = s2.ToArray();
                }
                

            }
            var dpost = new Posts(){ PAccountId = userManager.FindByEmailAsync(User.Identity.Name).GetAwaiter().GetResult().Id,  PName = post.name, PPhoto = buffer, PContent = post.content };
            _postsContext.Posts.Add(dpost);
            _postsContext.SaveChanges();

            foreach(var i in post.tags)
            {
                var tl = new PostTagList()
                {
                    TId = i,
                    PId = dpost.Id
                };
                _postsContext.PostTagList.Add(tl);
            }
            _postsContext.SaveChanges();


            
            return this.CreateResponse(new {id = dpost.Id});
        }

        /// <summary> 
        /// A07
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("/api/[controller]/posts/[action]")]
        public HttpResponseMessage Post([FromBody] ModifyPost post)
        {
            string base64 = post.content.photo.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64);
            byte[] buffer = null;
            using(var stream = new MemoryStream(bytes))
            {
                var img = System.Drawing.Image.FromStream(stream);
                using(var s2 = new MemoryStream())
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, (long)70);
	                myEncoderParameters.Param[0] = myEncoderParameter;
                    img.Save(s2, jpgEncoder, myEncoderParameters);
                    buffer = s2.ToArray();
                }
                

            }
            var dpost = _postsContext.Posts.Where(x => x.Id == post.p_id).First();
            dpost.PContent = post.content.content;
            dpost.PName = post.content.name;
            dpost.PPhoto = buffer;
            _postsContext.SaveChanges();


            _postsContext.RemoveRange(_postsContext.PostTagList.Where(x => x.PId == dpost.Id));

            foreach(var i in post.content.tags)
            {
                var tl = new PostTagList()
                {
                    TId = i,
                    PId = dpost.Id
                };
                _postsContext.PostTagList.Add(tl);
            }
            _postsContext.SaveChanges();


            
            return this.CreateResponse(new {id = dpost.Id});
        }

        /// <summary>
        /// A08
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        [Route("/api/[controller]/posts/[action]")]
        public HttpResponseMessage Post([FromBody] DeletePost post)
        {
            _postsContext.Remove(_postsContext.Posts.Where(x => x.Id == post.p_id).First());
            _postsContext.SaveChanges();
            
            return this.CreateResponse("刪除成功");

        }
        
        /// <summary>
        /// A09
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("/api/[controller]/posts/[action]/{id}")]
        public HttpResponseMessage Post(ulong id)
        {
            var post = _postsContext.Posts.Where(x => x.Id == id).First();
            var ret = new Post();
            ret.id = post.Id;
            ret.name = post.PName;
            ret.photo = GetDataURL(post.PPhoto);
            ret.count = post.PCount;
            ret.content = post.PContent;
            ret.created_time = post.Created.Value;
            ret.update_time = post.Modified.Value;
            ret.tags = _postsContext.PostTagList.Include(x => x.T).Where(x => x.PId == ret.id).Select(x => new ViewModels.Posts.Tag(){ t_id = x.TId, name = x.T.TName}).ToList();

            
            return this.CreateResponse(ret);

        }


        /// <summary>
        /// A12
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("/api/[controller]/posts/[action]")]
        public HttpResponseMessage Tag()
        {
            var data = _postsContext.Tags.ToList();
            return this.CreateResponse(data);
        }

        /// <summary>
        /// A13
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("/api/[controller]/posts/[action]")]
        public HttpResponseMessage Search(string q, int page = 1)
        {
            int count = 10;
            var qArr = q.Split(' ');
            List<object> args = new List<object>();
            // args.Add(qArr);
            string qu = "";
            for(int i = 0; i < qArr.Length; i++)
            {
                if(i != 0) qu += " or ";
                qu += $"PName.Contains(@{i})";
                args.Add(qArr[i]);

            }
            var data = _postsContext.Posts
                .Where(qu, args.ToArray())
                //.Where(qu, args.ToArray())
                .OrderByDescending(x => x.PCount)
                .Skip((page - 1) * count)
                .Take(count)
                .Select(x => new ViewModels.Posts.PostInterface(){
                    p_count = x.PCount,
                    p_name = x.PName,
                    p_id = x.Id,
                    p_photo = GetDataURL(x.PPhoto)
                })
                .ToList();

            return this.CreateResponse(data);
        }

        /// <summary>
        /// A14
        /// </summary>
        /// <param name="t_id"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("/api/[controller]/posts/[action]")]
        public HttpResponseMessage Top(uint? t_id = null, int top = 5)
        {
            if(top > 10)
                top = 10;
            
            List<ViewModels.Posts.PostInterface> data = null;
            if(t_id.HasValue)
            {
                data = _postsContext
                    .PostTagList
                    .Include(x => x.P)
                    .Where(x => x.TId == t_id)
                    .OrderByDescending(x => x.P.PCount)
                    .Take(top)
                    .Select(x => new ViewModels.Posts.PostInterface(){
                        p_count = x.P.PCount,
                        p_name = x.P.PName,
                        p_id = x.P.Id,
                        p_photo = GetDataURL(x.P.PPhoto)
                    })
                    .ToList();
            }
            else
            {
                data = _postsContext
                    .Posts
                    .OrderByDescending(x => x.PCount)
                    .Take(top)
                    .Select(x => new ViewModels.Posts.PostInterface(){
                        p_count = x.PCount,
                        p_name = x.PName,
                        p_id = x.Id,
                        p_photo = GetDataURL(x.PPhoto)
                    })
                    .ToList();

            }
            
            return this.CreateResponse(data);
        }

        private static string GetDataURL(byte[] imgFile)
        {
            var b64String = Convert.ToBase64String(imgFile);
            var dataUrl = "data:image/jpeg;base64," + b64String;
            return dataUrl;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        { 
            ImageCodecInfo codec =  ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == format.Guid).FirstOrDefault();
            if (codec==null)
            {
                return null;
            } 
            return codec;
             
        }

        private async Task<bool> ValidateUser(ViewModels.Identity.Login login)
        {
            var user = await userManager.FindByEmailAsync(login.email);
            if (user != null && await userManager.CheckPasswordAsync(user, login.password))
            {
                return true; // TODO

            }
            return false;
        }
    }
}
