using Aron.Web.WhiteList.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Aron.Web.WhiteList
{
    public class WhiteListService : IWhiteListService
    {
        private IEnumerable<WhiteLists> _whiteLists;
        private readonly ILogger _logger;
        private readonly IContentChecker _contentChecker;
        private readonly WhiteListOptions _options;
        private readonly ConcurrentDictionary<long, Models.Task> tasks;

        private object taskLock = new object();
        private long taskAi = 0;

        public WhiteListService(IEnumerable<WhiteLists> whiteLists, IContentChecker contentChecker, ILogger<WhiteListService> logger, WhiteListOptions options = null)
        {
            _whiteLists = whiteLists;
            _logger = logger;
            _contentChecker = contentChecker;
            if(options == null)
            {
                options = new WhiteListOptions();
            }
            _options = options;

            tasks = new ConcurrentDictionary<long, Models.Task>();
        }

        public bool Check(string path, IPAddress address)
        {

            path = path.ToLower();
            if(!_options.Enable)
            {
                return true;
            }
            if(_options.BasePath != "/")
            {
                if(path.StartsWith(_options.BasePath.ToLower()))
                {
                    path = path.Remove(1, _options.BasePath.Length - 1);
                }
            }
            //task start
            long tempID = -1;

            lock(taskLock) tempID = taskAi++; 

            var task = new Models.Task() { ID = tempID, Created = DateTime.Now, Path = path, Address = address };
            tasks.TryAdd(tempID, task);
            _logger.LogInformation("task start task. count:{0}", tasks.Count);

            bool ok = false;
            try
            {
                var routes = _whiteLists.Where(x => path.StartsWith(x.Route.ToLower()));
                if (routes.Count() == 0)
                {
                    ok = true;
                }
                else
                {
                    var db = routes.Select(x => x.WlContent as IEnumerable<WlContent>).Aggregate((x, y) => x.Concat(y));
                    if (db.Count() > 0)
                    {
                        bool[] b = new bool[3];
                        if (_contentChecker.CheckForAny(db, address, out b[0])
                            || _contentChecker.CheckForSingleAddr(db, address, out b[1])
                            || _contentChecker.CheckForCidr(db, address, out b[2]))
                            ok = true;

                        if (b.Any(x => x))
                            ok = false;
                    }
                    else ok = true;
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogError("{0}", ex);
                ok = false;
            }
            
            if(!ok)
            {
                _logger.LogWarning("source: {0}, path: {1} kill for white list.", address, path);
            }

            //task stop
            tasks.TryRemove(tempID, out var _);
            _logger.LogInformation("task success task. count:{0}", tasks.Count);
            return ok;
        }
        public bool Check(string path, string address)
        {
            return Check(path, IPAddress.Parse(address));
        }

        public bool Enable()
        {
            if (!IsEnable())
            {
                _options.Enable = true;
                _logger.LogInformation("The white list has been started.");
            }
            else
            {
                _logger.LogInformation("The white list has already started.");
            }
            return true;
        }

        public bool Disable(int waitMs = 10000)
        {
            if (IsEnable())
            {
                _logger.LogInformation("Wait for white list to disable...");
                _options.Enable = false;
                if(Wait4Clean(waitMs))
                {
                    _logger.LogInformation("The white list has been stopped.");
                    return true;
                }
                else 
                {
                    _logger.LogError("Timeout! Unable to stop white list.");
                    return false;
                }
            }
            else
            {
                _logger.LogInformation("The white list has already stopped.");
                return true;
            }
        }

        public bool IsEnable()
        {
            return tasks.Count != 0 || _options.Enable;
        }

        public bool SetWhiteLists(IEnumerable<WhiteLists> whiteLists)
        {
            if(Disable())
            {
                this._whiteLists = whiteLists;
                Enable();
                return true;
            }
            else
                return false;
        }

        private bool Wait4Clean(int waitMs = 10000)
        {
            for (int i = 0; i < waitMs; i+= 50)
            {
                if(tasks.Count == 0) break;
                System.Threading.Thread.Sleep(50);
            }
            return tasks.Count == 0;
        }
    }
}
