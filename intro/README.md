# How To Start?
1. Copy [appsettings.enpty.json](https://github.com/aron-666/Aron.Web.WhiteList/blob/main/intro/appsettings.enpty.json "appsettings.enpty.json") and rename it to appsettings.json.
2. Edit appsettings.json. 
    If you use database, please configure Sql session. 
    You can use migration to create database
    ### Create WhiteListContext to database.
    
       dotnet ef migrations add whitelists --context intro.Models.WhiteLists.WhiteListContext 
   
   ### Unmark MyWhiteListService

       //inject WhiteListService from database intro.Models.WhiteLists.WhiteListContext
       services.AddSingleton<IWhiteListService, MyWhiteListService>();
   ### Mark hard-coding region

		#region hard-coding

		// //inject whitelist record

		// {

		// var whitelists = new List<WhiteLists>()

		// {

		// new WhiteLists(){

		// Id = 1,

		// Name = "register",

		// Route = "/Identity/Account/Register",

		// WlContent = new List<WlContent>()

		// }

		// };

		// var content = new List<WlContent>()

		// {

		// //Allow ::1 (localhost)

		// new WlContent()

		// {

		// Id = 1,

		// Wid = whitelists.First().Id,

		// Policy = "Allow",

		// Content = "::1",

		// Source = whitelists.First()

		// },

		// //Allow 127.0.0.1

		// new WlContent()

		// {

		// Id = 2,

		// Wid = whitelists.First().Id,

		// Policy = "Allow",

		// Content = "127.0.0.1",

		// Source = whitelists.First()

		// },

		// //Allow 192.168.64.129-254

		// new WlContent()

		// {

		// Id = 3,

		// Wid = whitelists.First().Id,

		// Policy = "Allow",

		// Content = "192.168.64.128/25",

		// Source = whitelists.First()

		// },

		// //Deny 192.168.64.201

		// new WlContent()

		// {

		// Id = 4,

		// Wid = whitelists.First().Id,

		// Policy = "Deny",

		// Content = "192.168.64.201",

		// Source = whitelists.First()

		// },

		// };

		// whitelists[0].WlContent = content;

		  

		// services.AddSingleton<IEnumerable<WhiteLists>>(whitelists);

		// }

		// services.AddSingleton<IWhiteListService, WhiteListService>();

		  

		// #endregion hard-coding
4. Add record to database
 
	 ### Whitelists table:
	| ID | Name     | Route                         |
	|----|-----------|-------------------------------|
	| 1  | register | /Identity/Account/Register    |

	### WL_Content table

	| ID | Wid | Content             | Policy  |
	|----|-----|---------------------|---------|
	| 1  | 1   | ::1                 | Allow   |
	| 2  | 1   | 127.0.0.1           | Allow   |
	| 3  | 1   | 192.168.64.128/25   | Allow   |
	| 4  | 1   | 192.168.64.201      | Deny    |

5. Join it!
 You can go to url `/Identity/Account/Register` to test.
