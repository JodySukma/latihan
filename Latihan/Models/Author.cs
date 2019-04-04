using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Latihan.Models
{
    public partial class Author
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        public int AuthorID { get; set; }

        [Display(Name = "Authour's Name")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(256, ErrorMessage = "{0} tidak boleh lebih {1} karakter")]
        public String Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(256, ErrorMessage = "{0} tidak boleh lebih {1} karakter")]
        [RegularExpression(@"^([a-aZ-Z0-9_\.\-])+\@(({a-Az-Z0-9\-})+\.)+(a-zA-Z0-9]{2,4})+$",
            ErrorMessage = "{0} tidak valid.")]
        public String Email { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
       
    }
}
