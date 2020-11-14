using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aron.Web.WhiteList.Models
{
    public class WlContent
    {
        private string content;

        public long Id { get; set; }
        public long Wid { get; set; }
        public string Content 
        { 
            get => content; 
            set 
            { 
                if(value.ToLower() == "any")
                {
                    
                }
                else if(value.Contains("/"))
                {
                    var temp = value.Split("/");
                    if(!IPAddress.TryParse(temp[1], out var _) || !byte.TryParse(temp[1], out var _))
                        throw new ArgumentException("jast can be any, single address or address/cidr");
                }
                else if(IPAddress.TryParse(value, out var _))
                {

                }
                else
                {
                    throw new ArgumentException("jast can be any, single address or address/cidr");
                }
                content = value; 
            } 
        }
        public string Remarks { get; set; }

        public string Policy { get => policy.ToString(); set => policy = (Policy)Enum.Parse(typeof(Policy), value); }
        public Policy policy;
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }



        public WhiteLists Source;
    }

    public enum Policy
    {
        Allow,
        Deny
    }
}
