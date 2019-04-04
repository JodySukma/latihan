using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Latihan.Data;
using Latihan.Models;

namespace Latihan.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _enviroment;

        public BooksController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _enviroment = environment;
        }

        // GET: Books
        public IActionResult Index()
        {
            var bookList = _context.Books
                .Include(c => c.Category)
                .Include(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author)
                .ToList();

            IList<BookViewModel> items = new List<BookViewModel>();
            foreach (Book book in bookList)
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
                foreach (BookAuthor bookAuthors in booksAuthorList)
                {
                    var author = bookAuthors.Author;
                    authorNameList += author.Name + ", ";
                }
                item.AuthorNames = authorNameList.Substring(0, authorNameList.Length - 2);
                items.Add(item);
            }
            return View(items);
        }
        // sebelum dimodifikasi yang bawah
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Books.Include(b => b.Category);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: Books/Details/5
        public IActionResult Details(int? id)
        {
            var book = _context.Books
                .Include(c => c.Category)
                .Include(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author).SingleOrDefault(b => b.BookId.Equals(id));

            BookViewModel item = new BookViewModel();
            item.ISBN = book.BookId;
            item.Title = book.Title;
            item.PublishDate = book.PublishDate;
            item.Price = book.Price;
            item.Quantity = book.Quantity;
            item.CategoryName = book.Category.Name;

            string authorNameList = string.Empty;
            var booksAuthorList = book.BookAuthors;
            foreach (BookAuthor bookAuthors in booksAuthorList)
            {
                var author = bookAuthors.Author;
                authorNameList += author.Name + ", ";
            }
            item.AuthorNames = authorNameList.Substring(0, authorNameList.Length - 2);
            return View(item);
        }

        // sebelum dimodifikasi yang bawah
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var book = await _context.Books
        //        .Include(b => b.Category)
        //        .SingleOrDefaultAsync(m => m.BookId == id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(book);
        //}

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryID", "Name");
            ViewBag.Authors = new MultiSelectList(_context.Authors.ToList(), "AuthorID", "Name");

            //ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookFormViewModel item)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book();
                book.BookId = item.ISBN;
                book.CategoryID = item.CategoryID;
                book.Title = item.Title;
                book.PublishDate = item.PublishDate;
                book.Price = item.Price;
                book.Quantity = item.Quantity;
                _context.Add(book);

                foreach (int authorID in item.AuthorIDds)
                {
                    BookAuthor bookAuthor = new BookAuthor();
                    bookAuthor.BookID = item.ISBN;
                    bookAuthor.AuthorID = authorID;
                    _context.Add(bookAuthor);
                }
                _context.SaveChanges();

                if (item.Photo != null)
                {
                    var file = item.Photo;
                    var upload = Path.Combine(_enviroment.WebRootPath, "upload");

                    if(file.Length > 0)
                    {
                        using (var fileStream = new FileStream(Path.Combine(upload, item.ISBN + ".jpg"), FileMode.Create))
                        {
                            file.CopyToAsync(fileStream);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
        }


        //sebelum dirubah yang bawah
        //public async Task<IActionResult> Create([Bind("BookId,CategoryID,Title,Photo,PublishDate,Price,Quantity")] Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(book);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", book.CategoryID);
        //    return View(book);
        //}

        // GET: Books/Edit/5
        [HttpGet]
        public IActionResult Edit (int? id)
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryID", "Name");
            ViewBag.Authors = new MultiSelectList(_context.Authors.ToList(), "AuthorID", "Name");

            var book = _context.Books.SingleOrDefault(p => p.BookId.Equals(id));

            BookFormViewModel item = new BookFormViewModel();
            item.ISBN = book.BookId;
            item.Title = book.Title;
            item.PublishDate = book.PublishDate;
            item.Price = book.Price;
            item.Quantity = book.Quantity;
            item.CategoryID = book.CategoryID;

            var authorList = _context.BooksAuthors.Where(p => p.BookID.Equals(book.BookId)).ToList();
            List<int> authors = new List<int>();
            foreach (BookAuthor bookAuthor in authorList)
            {
                authors.Add(bookAuthor.AuthorID);
            }
            item.AuthorIDds = authors.ToArray();

            return View(item);
        }


        //Sebelum dirubah yang bawah
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var book = await _context.Books.SingleOrDefaultAsync(id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", book.CategoryID);
        //    return View(book);
        //}

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("ISBN, CategoryID, Title, Photo, PublishDate, Price, Quantity, AuthorIDs")] BookFormViewModel item)
        {
            if (ModelState.IsValid)
            {
                _context.BooksAuthors.RemoveRange(_context.BooksAuthors.Where(p => p.BookID.Equals(item.ISBN)));
                _context.SaveChanges();

                Book book = _context.Books.SingleOrDefault(p => p.BookId.Equals(item.ISBN));
                book.CategoryID = item.CategoryID;
                book.Title = item.Title;
                book.PublishDate = item.PublishDate;
                book.Price = item.Price;
                book.Quantity = item.Quantity;
                _context.Update(book);

                foreach (int authorid in item.AuthorIDds)
                {
                    BookAuthor bookAuthor = new BookAuthor();
                    bookAuthor.BookID = item.ISBN;
                    bookAuthor.AuthorID = authorid;
                    _context.Add(bookAuthor);
                }
                _context.SaveChanges();

                if (item.Photo != null)
                {
                    var file = item.Photo;
                    var upload = Path.Combine(_enviroment.WebRootPath, "upload");

                    if (file.Length > 0)
                    {
                        using (var fileStream = new FileStream(Path.Combine(upload, item.ISBN + ".jpg"), FileMode.Create))
                        {
                            file.CopyToAsync(fileStream);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
        }


        //sebelum dirubah yang dibawah
        //public async Task<IActionResult> Edit(int id, [Bind("BookId,CategoryID,Title,Photo,PublishDate,Price,Quantity")] Book book)
        //{
        //    if (id != book.BookId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(book);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BookExists(book.BookId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", book.CategoryID);
        //    return View(book);
        //}

        // GET: Books/Delete/5
        public IActionResult Delete(int? id)
        {
            var book = _context.Books
                .Include(c => c.Category)
                .Include(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author).SingleOrDefault(b => b.BookId.Equals(id));

            BookViewModel item = new BookViewModel();
            item.ISBN = book.BookId;
            item.Title = book.Title;
            item.PublishDate = book.PublishDate;
            item.Price = book.Price;
            item.Quantity = book.Quantity;
            item.CategoryName = book.Category.Name;

            string authorNameList = string.Empty;
            var bookAuthorList = book.BookAuthors;
            foreach (BookAuthor bookAuthor in bookAuthorList)
            {
                var author = bookAuthor.Author;
                authorNameList += author.Name + ", "; 
            }
            item.AuthorNames = authorNameList.Substring(0, authorNameList.Length - 2);

            return View(item);
        }

        //sebelum dirubah yang dibawah
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var book = await _context.Books
        //        .Include(b => b.Category)
        //        .SingleOrDefaultAsync(m => m.BookId == id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(book);
        //}

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                _context.BooksAuthors.RemoveRange(_context.BooksAuthors.Where(p => p.BookID.Equals(id)));
                _context.SaveChanges();

                var book = _context.Books.SingleOrDefault(m => m.BookId == id);

                _context.Books.Remove(book);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        // seblum dirubah yang bawah
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var book = await _context.Books.FindAsync(id);
        //    _context.Books.Remove(book);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool BookExists(int id)
        //{
        //    return _context.Books.Any(e => e.BookId == id);
        //}

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

    }
}
