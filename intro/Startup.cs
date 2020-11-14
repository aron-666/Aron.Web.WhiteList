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

            services.AddDefaultIdentity<IdentityUser>(options => 
            {
                options.SignIn.RequireConfirmedAccount = true;
                
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton<IContentChecker, ContentChecker>();

            services.AddSingleton<IWhiteListService, MyWhiteListService>();

            services.AddSingleton(whiteListOptions);
            var config = new MapperConfiguration(c => c.AddProfile(new Models.MyMapper()));
            services.AddSingleton(config.CreateMapper());
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
            var whiteListOptions = new WhiteListOptions();
            Configuration.GetSection("WhiteListOptions").Bind(whiteListOptions);
            app.UsePathBase(whiteListOptions.BasePath);
            app.UseDefaultWhiteListMiddleWare(x => x.Response.Redirect(whiteListOptions.BasePath));
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
