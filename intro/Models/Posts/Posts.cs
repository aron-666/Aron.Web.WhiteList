using System;
using System.Collections.Generic;

namespace intro.Models.Posts
{
    public partial class Posts : ITimeLogger
    {
        public Posts()
        {
            PostTagList = new HashSet<PostTagList>();
        }

        public ulong Id { get; set; }
        public string PName { get; set; }
        public string PContent { get; set; }
        public string PAccountId { get; set; }
        public ulong PCount { get; set; }
        public byte[] PPhoto { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual ICollection<PostTagList> PostTagList { get; set; }
    }
}
