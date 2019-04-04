using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Latihan.Data;
using Latihan.Models;
using Microsoft.EntityFrameworkCore;

namespace Latihan.Controllers
{
    public class BookController : Controller
    {
        private ApplicationDbContext data;

        public BookController(ApplicationDbContext db) => data = db ;

        [HttpGet]
        public IActionResult Index()
        {
            var booklist = data.Books
            .Include(c => c.Category)
            .Include(ba => ba.BookAuthors)
            .ThenInclude(a => a.Author)
            .ToList();

            IList<BookViewModel> items = new List<BookViewModel>();
            foreach (Book book in booklist)
            {
                BookViewModel item = new BookViewModel();
                item.ISBN = book.BookId;
                item.Title = book.Title;
                item.Photo = book.Photo;
                item.PublishDate = book.PublishDate;
                item.Price = book.Price;
                item.Quantity = book.Quantity;
                item.CategoryName = book.Category.Name;

                string authorNameList = string.Empty;
                var booksAuthorList = book.BookAuthors;
                foreach (BookAuthor bookAuthor in booksAuthorList)
                {
                    var author = bookAuthor.Author;
                    authorNameList = authorNameList + author.Name + ", ";
                }
                item.AuthorNames = authorNameList.Substring(0, authorNameList.Length - 2);

                items.Add(item);
            }
            return View(items);
        }
    }
}