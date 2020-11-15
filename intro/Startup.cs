using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using intro.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using intro.Services;
using Aron.Web.WhiteList.Models;
using Aron.Web.WhiteList;
using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace intro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var sql = new Config.Sql();
            Configuration.GetSection("Sql").Bind(sql);
            var whiteListOptions = new WhiteListOptions();
            Configuration.GetSection("WhiteListOptions").Bind(whiteListOptions);
            services.AddDbContext<Models.WhiteLists.WhiteListContext>(options => sql.UseSqlService(options));
            services.AddSingleton(sql);

            services.AddDbContext<ApplicationDbContext>(options =>
                sql.UseSqlService(options));


            services.AddDefaultIdentity<IdentityUser>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;

                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = false;
                opt.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            
            //1. inject IContentChecker
            services.AddSingleton<IContentChecker, ContentChecker>();

            //2. inject IWhiteListService
            services.AddSingleton<IWhiteListService, MyWhiteListService>();

            //3. inject options. if not need, you can not inject. 
            services.AddSingleton(whiteListOptions);

            //mapper for db models to whitelist models
            var config = new MapperConfiguration(c => c.AddProfile(new Models.MyMapper()));
            services.AddSingleton(config.CreateMapper());

            if(Configuration.GetValue<bool>("UseProxy"))
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });
            }
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if(Configuration.GetValue<bool>("UseProxy"))
            {
                app.UseForwardedHeaders();
            }
            var whiteListOptions = new WhiteListOptions();
            Configuration.GetSection("WhiteListOptions").Bind(whiteListOptions);

            app.Use((context, next) =>
            {
                context.Request.PathBase = new Microsoft.AspNetCore.Http.PathString(whiteListOptions.BasePath);

                if(context.Request.Path.Value.StartsWith("//"))
                    context.Request.Path = context.Request.Path.Value.Remove(0, 1);
                return next();
            });
            
            //use whitelist middleware
            app.UseDefaultWhiteListMiddleWare(x => 
                x.Response.Redirect(Path.Combine(whiteListOptions.BasePath, "Home/Forbidden")));

            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern:  "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                
            });

        }
    }
}
