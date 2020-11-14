using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intro.Config
{
    public class Sql
    {
        public string Provider { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public override string ToString()
        {
            string ret = "";
            switch (Provider.ToUpper())
            {
                case "MYSQL":
                    ret = "server={0};port={1};Database={2};Uid={3};Pwd={4};CharSet=utf8;";
                    break;
                case "MSSQL":
                    ret = "Server={0},{1};Database={2};User Id={3};Password={4};";
                    break;
            }
            return string.Format(ret, Host, Port, Database, Username, Password);
        }

        //public void UseSqlService(IServiceCollection services)
        //{
        //    switch (Provider.ToUpper())
        //    {
        //        case "MYSQL":
        //            services.AddDbContext<ApplicationDbContext>(options =>
        //                options.UseMySql(
        //                ToString()));
        //            break;
        //        case "MSSQL":
        //            services.AddDbContext<ApplicationDbContext>(options =>
        //                options.UseSqlServer(
        //                ToString()));
        //            break;
        //    }

        //}
        public void UseSqlService(DbContextOptionsBuilder options)
        {

            switch (Provider.ToUpper())
            {
                case "MYSQL":
                    options.UseMySql(
                    ToString());
                    break;
                case "MSSQL":
                    options.UseSqlServer(
                    ToString());
                    break;
            }

        }
    }
}
