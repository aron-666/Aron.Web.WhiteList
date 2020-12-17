using System;
using System.Net;


namespace Aron.Web.WhiteList.Models
{
    internal class Task
    {
        
        public long ID{ get; set; }

        public DateTime Created{ get; set; }

        public IPAddress Address{ get; set; }

        public string Path{ get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            return obj is Task t && t.ID == this.ID;
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }
    }
}