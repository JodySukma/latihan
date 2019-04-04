using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Latihan.Models
{
    public partial class BookViewModel
    {
        [Display(Name = "ISBN")]
        public int ISBN { get; set; }

        [Display(Name = "Category")]
        public String CategoryName { get; set; }

        [Display(Name = "Title")]
        public String Title { get; set; }

        [Display(Name = "Photo")]
        public String Photo { get; set; }

        [Display(Name = "Publish Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy}")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "List Author Names")]
        public string AuthorNames { get; set; }
    }
}
