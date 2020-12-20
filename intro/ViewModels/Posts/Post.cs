using System;
using System.Collections.Generic;

namespace intro.ViewModels.Posts
{
    public class Post
    {
        public ulong id{get;set;}
        public ulong count{get;set;}
        public DateTime created_time{get;set;}
        public DateTime update_time{get;set;}
        public string name{get;set;}
        public string content{get;set;}
        public string photo{get;set;}
        public IEnumerable<Tag> tags{get;set;}

    }
}