using System;
using System.Collections.Generic;

namespace intro.ViewModels.Intro
{
    public class PostList
    {
        public List<Post> Posts { get; set; } = new List<Post>();
    }

    public class Post
    {
        public DateTime Time{ get; set; }
        public string Image { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }

        public Post Clone()
        {
            return new Post()
            {
                Time = Time,
                Image = Image,
                Subject = Subject,
                Content = Content,
                Url = Url
            };

        }
    }

}