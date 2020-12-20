using System;
using System.Collections.Generic;

namespace intro.Models.WhiteLists
{
    public partial class WlContent : ITimeLogger
    {
        public long Id { get; set; }
        public long Wid { get; set; }
        public string Content { get; set; }
        public string Policy { get; set; }
        public string Remarks { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual Whitelists W { get; set; }
    }
}
