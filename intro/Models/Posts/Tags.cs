using System;
using System.Collections.Generic;

namespace intro.Models.Posts
{
    public partial class Tags : ITimeLogger
    {
        public Tags()
        {
            PostTagList = new HashSet<PostTagList>();
        }

        public ulong Id { get; set; }
        public string TName { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual ICollection<PostTagList> PostTagList { get; set; }
    }
}
