﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intro.Models.WhiteLists
{
    public interface ITimeLogger
    {
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
