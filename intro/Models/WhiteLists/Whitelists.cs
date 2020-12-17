using System;
using System.Collections.Generic;

namespace intro.Models.WhiteLists
{
    public partial class Whitelists
    {
        public Whitelists()
        {
            Source = new List<WlContent>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string Remarks { get; set; }

        public virtual List<WlContent> Source { get; set; }
    }
}
