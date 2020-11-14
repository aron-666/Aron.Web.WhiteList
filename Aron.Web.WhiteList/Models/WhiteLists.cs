using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aron.Web.WhiteList.Models
{
    public class WhiteLists
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string Remarks { get; set; }

        public IEnumerable<WlContent> WlContent;
    }
}
