

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
 
     `services.AddSingleton<IContentChecker, ContentChecker>();`
 
2. inject IWhiteListService
  inject WhiteListService from database intro.Models.WhiteLists.WhiteListContext.
  `services.AddSingleton<IWhiteListService, MyWhiteListService>();`
 
   or inject WhiteListService from hard-coding.
 
       services.AddSingleton<IEnumerable<WhiteLists>>(whitelists);
   
       services.AddSingleton<IWhiteListService, WhiteListService>();

  
3. inject options. if not need, you can not to inject.

   `services.AddSingleton(whiteListOptions);`

### Startup.Configure
 1. UseDefaultWhiteListMiddleWare and configure onKill event.  

        app.UseDefaultWhiteListMiddleWare(x =>

        x.Response.Redirect(Path.Combine(whiteListOptions.BasePath, "Home/Forbidden")));
