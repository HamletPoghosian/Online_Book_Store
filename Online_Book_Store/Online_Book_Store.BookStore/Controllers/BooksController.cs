using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreOnline.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Book_Store.BookStore.Data;
using Online_Book_Store.BookStore.Models;
using Online_Book_Store.BookStore.Models.CategoryViewModel;
using Online_Book_Store.BookStore.Service.CategoryService;

namespace Online_Book_Store.BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookService _addBook;
        private readonly ICategoryService _addCategory;

        public BooksController(ApplicationDbContext context, IBookService addbook, ICategoryService category)
        {
            _context = context;
            _addBook = addbook;
            _addCategory = category;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var entity = await _addBook.GetBooksAsync();
            return View(entity.Select(book => new ViewBook
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                CategoryName = book.Category.Name,
                Popular = book.Popular,
                Price = book.Price,
                Publish = book.Publish
            }));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _addBook.GetBookAsync(id.Value);

            await _addBook.EditAsync(entity);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }
        public async Task<IActionResult> GetBooks(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _addBook.GetBookAsync(id.Value);

            var viewBook = new ViewBook
            {
                Id = entity.Id,
                Author = entity.Author,
                Name = entity.Name,
                Popular = entity.Popular,
                Price = entity.Price,
                CategoryName = entity.CategoryName,
                Publish = entity.Publish
            };

            if (entity == null)
            {
                return NotFound();
            }

            return View(viewBook);
        }

        [Authorize(Roles = "Admin")]
        // GET: Books/Create
        public IActionResult Create()
        {
            GetCategoryDropDown();
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Author,Popular,Price,Publish,CategoryId")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();

                await _addBook.AddItemsAsync(book);

                return RedirectToAction(nameof(Index));
            }
            GetCategoryDropDown();

            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Id", book.CategoryId);

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Author,Popular,Price,Publish,CategoryId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Id", book.CategoryId);
            return View(book);
        }


        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books

                .Include(b => b.Category)

                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        private void GetCategoryDropDown()
        {
            var categories = _addCategory.GetCategory().Select(category => new CategoryDropDown
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Discription

            });

            ViewData["CategoryName"] = new SelectList(categories, "Id", "Name");
        }


        [HttpPost]
        public  IActionResult AllBooks(ViewSearch model)
        {

            IEnumerable<Book> book = _context.Books.Include(b => b.Category)
                .Where(b => b.Name == model.Name);

            if (!string.IsNullOrWhiteSpace(model.Author))
            {
                book = book.Where(b => b.Author == model.Author);

                if (model.Popular>0)
                {
                    book = book.Where(b => b.Popular >= model.Popular);
                    if (model.Publish != null)
                    {
                        book = book.Where(b => b.Publish == model.Publish);
                        if (model.MaxPrice >= model.MaxPrice)
                        {
                            book = book.Where(b => b.Price >= model.MinPrice && b.Price <= model.MaxPrice);
                        }
                    }
                }
            }
             
            if (model!=null)
            {
                book = book;
            }

            if (book == null)
            {
                return NotFound();
            }

            model.Books = book.Select(b => new ViewBook
            {
                Id = b.Id,
                Author = b.Author,
                Name = b.Name,
                CategoryName = b.CategoryName,
                Popular = b.Popular,
                Price = b.Price,
                Publish = b.Publish
            });

            return View(model);
        }

        public async Task<IActionResult> AllBooks(int count = 0)
        {
            ViewSearch view = new ViewSearch();
            var book = await _context.Books.Include(b => b.Category).ToListAsync();



            if (book == null)
            {
                return NotFound();
            }
            view.Books = book.Select(b => new ViewBook
            {
                Id = b.Id,
                Author = b.Author,
                Name = b.Name,
                CategoryName = b.Category.Name,
                Popular = b.Popular,
                Price = b.Price,
                Publish = b.Publish
            }).Skip(count * 10).Take(10);
            return View(view);
        }
    }
}
