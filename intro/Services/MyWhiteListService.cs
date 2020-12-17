using Aron.Web.WhiteList;
using Aron.Web.WhiteList.Models;
using AutoMapper;
using intro.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wt = intro.Models.WhiteLists;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using System.Threading;

namespace intro.Services
{
    public class MyWhiteListService : WhiteListService
    {
        private IMapper _mapper;
        private Sql _sql;
        private IWebHostEnvironment _env;

        public MyWhiteListService(Sql sql, IMapper mapper, IWebHostEnvironment env, IContentChecker contentChecker, ILogger<WhiteListService> logger, WhiteListOptions options = null) : base(GetData(CreateContext(sql, env), mapper), contentChecker, logger, options)
        {
            _mapper = mapper;
            _sql = sql;
            _env = env;
            // Thread t = new Thread(() =>
            // {
            //     while (true)
            //     {
            //         Thread.Sleep(3000);
            //         Update();
            //     }
            // });

            // t.IsBackground = true;
            // t.Start();
        }

        public void Update()
        {
            this.SetWhiteLists(GetData(CreateContext(_sql, _env), _mapper));
        }

        private static IEnumerable<WhiteLists> GetData(wt.WhiteListContext context, IMapper mapper)
        {
            var whiteLists = context
                .Whitelists
                .Include(x => x.Source)
                .ToList();
            var wl2 = whiteLists
                .Select(x => mapper.Map<WhiteLists>(x))
                .ToList();
            context.Dispose();
            return wl2;
        }

        internal static wt.WhiteListContext CreateContext(Sql sql, IWebHostEnvironment env)
        {
            DbContextOptionsBuilder<wt.WhiteListContext> op = new DbContextOptionsBuilder<wt.WhiteListContext>();
            sql.UseSqlService(op);
            if (env == null || env.IsDevelopment())
            {
                op
                    .UseLoggerFactory(LoggerFactory.Create(builder =>
                    {
                        builder.AddDebug().AddConsole();
                    }))
                    .EnableSensitiveDataLogging();
            }

            var context = new wt.WhiteListContext(op.Options);
            return context;
        }
    }
}
