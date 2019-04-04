using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Latihan.Models
{
    public partial class UserViewModel
    {
        [Display(Name = "Username")]
        public String UserName { set; get; }

        [Display(Name = "Role")]
        public String RoleName { set; get; }

        [Display(Name = "Email")]
        public String Email { get; set; }

        [Display(Name = "Fullname")]
        public String Fullname { get; set; }
    }
}
