using Aron.Web.WhiteList.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Aron.Web.WhiteList
{
    public class ContentChecker : IContentChecker
    {
        private readonly ILogger _logger;
        public ContentChecker(ILogger<IContentChecker> logger)
        {
            _logger = logger;
        }

        public bool CheckForAny(IEnumerable<WlContent> wlContents, IPAddress address, out bool isBlack)
        {
            isBlack = false;
            var list = wlContents.Where(x => x.Content.Length == 3 && x.Content.ToUpper() == "ANY").ToList();
            if (list.Count != 0)
            {
                if(list.Any(x => x.policy == Policy.Deny))
                {
                    isBlack = true;
                    return false;
                }
                //只要有一個any就通過
                return true;
            }
            //都不通過 回傳驗證失敗
            return false;
        }

        public bool CheckForCidr(IEnumerable<WlContent> wlContents, IPAddress address, out bool isBlack)
        {
            isBlack = false;
            //1. 找出帶Cidr的
            List<WlContent> content = wlContents.Where(x => x.Content.Contains("/")).ToList();

            //2. 建立分析器 進行分析

            foreach (var i in content)
            {
                try
                {
                    var temp = i.Content.Split("/");
                    IPAddress test = IPAddress.Parse(temp[0]);
                    byte c = byte.Parse(temp[1]);
                    
                    IPNetwork network = IPNetwork.Parse(i.Content);
                    
                    if (test.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && address.IsIPv4MappedToIPv6)
                    {
                        if (network.Contains(address.MapToIPv4()))
                        {
                            if(i.policy == Policy.Deny)
                            {
                                isBlack = true;
                                return false;
                            }
                            else
                                //只要一個規則驗證通過就回傳成功
                                return true;
                        }
                            
                    }
                    else if(test.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        if (network.Contains(address.MapToIPv6()))
                        {
                            if (i.policy == Policy.Deny)
                            {
                                isBlack = true;
                                return false;
                            }
                            else
                                //只要一個規則驗證通過就回傳成功
                                return true;
                        }
                            
                    }
                    else if(test.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        if (network.Contains(address.MapToIPv4()))
                        {
                            if (i.policy == Policy.Deny)
                            {
                                isBlack = true;
                                return false;
                            }
                            else
                                //只要一個規則驗證通過就回傳成功
                                return true;
                        }
                            
                    }
                    
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError("{0} error, args = route: {1}, content: {2}, ex: {3}", nameof(CheckForCidr), i.Source.Route, i.Content, ex);
                    Console.Error.WriteLine("{0} error, args = route: {1}, content: {2}, ex: {3}", nameof(CheckForCidr), i.Source.Route, i.Content, ex);
                }
            }

            //都不通過 回傳驗證失敗
            return false;
        }

        public bool CheckForSingleAddr(IEnumerable<WlContent> wlContents, IPAddress address, out bool isBlack)
        {
            isBlack = false;

            //1. 找出single address的
            List<WlContent> content = wlContents.Where(x => !x.Content.Contains("/") && x.Content.ToUpper() != "ANY").ToList();

            foreach(var i in content)
            {
                try
                {
                    IPAddress test = IPAddress.Parse(i.Content);

                    if (test.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && address.IsIPv4MappedToIPv6)
                    {
                        if (address.MapToIPv4().Equals(test))
                        {
                            if (i.policy == Policy.Deny)
                            {
                                isBlack = true;
                                return false;
                            }
                            else
                                return true;
                        }
                    }
                    else if(test.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        if (address.MapToIPv6().Equals(test.MapToIPv6()))
                        {
                            if (i.policy == Policy.Deny)
                            {
                                isBlack = true;
                                return false;
                            }
                            else
                                return true;
                        }
                    }
                    else if(test.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        if (address.MapToIPv4().Equals(test.MapToIPv4()))
                        {
                            if (i.policy == Policy.Deny)
                            {
                                isBlack = true;
                                return false;
                            }
                            else
                                return true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError("{0} error, args = route: {1}, content: {2}, ex: {3}", nameof(CheckForSingleAddr), i.Source.Route, i.Content, ex);
                    Console.Error.WriteLine("{0} error, args = route: {1}, content: {2}, ex: {3}", nameof(CheckForSingleAddr), i.Source.Route, i.Content, ex);
                }
            }



            return false;
        }
    }
}
