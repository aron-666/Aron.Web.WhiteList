using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace intro.Data
{
    public class AppUser : IdentityUser
    {
        [NotMapped]
        public string Name{ get => Email.Split('@')[0]; }
    }
}
