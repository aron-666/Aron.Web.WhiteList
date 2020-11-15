


![](https://komarev.com/ghpvc/?username=aron-666&color=green)
# Aron.Web.WhiteList
This is a firewall solution work from .Net Core 3.1, you can achieve the whitelist effect by setting policy for routing.

We use [IPNetwork2](https://github.com/lduchosal/ipnetwork)  to handle address and cirb.

## Installation
[Nuget page](https://www.nuget.org/packages/Aron.Web.WhiteList/)

### PM
    nuget install Aron.Web.WhiteList

### Dotnet Cli

    dotnet add package Aron.Web.WhiteList
    
## How To Use?

### Startup.ConfigureServices
 1. inject IContentChecker
 
        services.AddSingleton<IContentChecker, ContentChecker>();
 
2. inject IWhiteListService

   inject WhiteListService from database intro.Models.WhiteLists.WhiteListContext.
  
       services.AddSingleton<IWhiteListService, MyWhiteListService>();
 
   or inject WhiteListService from hard-coding.
 
        {
            var whitelists = new List<WhiteLists>()
            {
                new WhiteLists(){
                    Id = 1,
                    Name = "register",
                    Route = "/Identity/Account/Register",
                    WlContent = new List<WlContent>()
                }
            };
            var content = new List<WlContent>()
            {
                //Allow ::1 (localhost)
                new WlContent()
                {
                    Id = 1,
                    Wid = whitelists.First().Id,
                    Policy = "Allow",
                    Content = "::1",
                    Source = whitelists.First()
                },
                //Allow 127.0.0.1
                new WlContent()
                {
                    Id = 2,
                    Wid = whitelists.First().Id,
                    Policy = "Allow",
                    Content = "127.0.0.1",
                    Source = whitelists.First()
                },
                //Allow 192.168.64.129-254
                new WlContent()
                {
                    Id = 3,
                    Wid = whitelists.First().Id,
                    Policy = "Allow",
                    Content = "192.168.64.128/25",
                    Source = whitelists.First()
                },
                //Deny 192.168.64.201
                new WlContent()
                {
                    Id = 4,
                    Wid = whitelists.First().Id,
                    Policy = "Deny",
                    Content = "192.168.64.201",
                    Source = whitelists.First()
                },
            };
            whitelists[0].WlContent = content;
            
            services.AddSingleton<IEnumerable<WhiteLists>>(whitelists);
        }
        services.AddSingleton<IWhiteListService, WhiteListService>();

  
3. inject options. if not need, you can not to inject.

       services.AddSingleton(whiteListOptions);

### Startup.Configure
 1. UseDefaultWhiteListMiddleWare and configure onKill event.  

        app.UseDefaultWhiteListMiddleWare(x =>

            x.Response.Redirect(Path.Combine(whiteListOptions.BasePath, "Home/Forbidden")));
# Examples
See [intro](https://github.com/aron-666/Aron.Web.WhiteList/tree/main/intro "intro")
