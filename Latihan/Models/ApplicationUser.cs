using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Latihan.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
        public string Photo { get; set; }
    }
}
