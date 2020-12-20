

using System;

namespace intro.ViewModels.Posts
{
    public class PostInterface
    {
        public UInt64 p_id { get; set; }

        public string p_name { get; set; }
        public string p_photo { get; set; }
        public UInt64 p_count { get; set; }
    }
}