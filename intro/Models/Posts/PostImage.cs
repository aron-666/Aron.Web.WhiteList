using System;
using System.Collections.Generic;

namespace intro.Models.Posts
{
    public partial class PostImage : ITimeLogger
    {
        public ulong Id { get; set; }
        public string UId { get; set; }
        public byte[] IImage { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
