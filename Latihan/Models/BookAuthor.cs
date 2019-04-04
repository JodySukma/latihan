using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Latihan.Models
{
    public partial class BookAuthor
    {
        public int BookAuthorID { get; set; }

        [ForeignKey("Book")]
        [Display(Name = "ISBN")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} harus diisi angka")]
        [StringLength (13, MinimumLength = 10, ErrorMessage = "{0} tidak boleh lebih {1} dan tidak boleh kurang {2} karakter.")]
        public int BookID { get; set; }
        public Book Book { get; set; }

        [ForeignKey("Author")]
        [Display(Name = "AuthorID")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        public int AuthorID { get; set; }
        public Author Author { get; set; }

    }
}
