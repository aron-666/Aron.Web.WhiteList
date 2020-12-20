using System;
using System.Collections.Generic;

namespace intro.Models.Posts
{
    public partial class PostTagList : ITimeLogger
    {
        public ulong TId { get; set; }
        public ulong PId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual Posts P { get; set; }
        public virtual Tags T { get; set; }
    }
}
