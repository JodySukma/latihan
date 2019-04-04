using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Latihan.Models
{
    public partial class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        [Display(Name = "ISBN")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} harus angka")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "{0} tidak boleh lebih dari {1} dan tidak boleh kurang {2} karakter.")]
        public int BookId { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} harus angka")]
        public int CategoryID { get; set; }
        
        public Category Category { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        public String Title { get; set; }

        [Display(Name = "Photo")]
        public String Photo { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Price")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} harus diisi angka")]
        public double Price { get; set; }

        [Display(Name = "Quantity")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} harus diisi angka")]
        public int Quantity { get; set; }

        public ICollection <BookAuthor> BookAuthors { get; set; }
    }
}
