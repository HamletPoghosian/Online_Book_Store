using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreOnline.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Book_Store.BookStore.Data;
using Online_Book_Store.BookStore.Models;

namespace Online_Book_Store.BookStore.Controllers
{
    public class ShopingCartsController : Controller
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ApplicationDbContext _context;
        IBookService _addbook;

        public ShopingCartsController(ApplicationDbContext context, UserManager<ApplicationUser> manager, IBookService addbook)
        {
            _manager = manager;
            _context = context;
            _addbook = addbook;
        }
        [Authorize]
        // GET: ShopingCarts
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUser();

            var shopincCartitems = await _context.ShopingCarts.Include(e=>e.Book).ThenInclude(book => book.Category).Where(u=>u.ApplicationUserId.ToString()==user.Id).ToListAsync();

             
            return View(shopincCartitems.Select(e=>new ShopingCartModel {
                Id=e.Id,               
                Amount=e.Amount,   
                TotalPrice=e.Amount*e.Book.Price,
                Book=new ViewBook
                {
                    Id=e.Book.Id,
                    Name=e.Book.Name,
                    Author=e.Book.Author,
                    Popular=e.Book.Popular,
                    Price=e.Book.Price,
                    Publish=e.Book.Publish,
                    CategoryName=e.Book.Category.Name
                }
                
            }));
        }
        [Authorize(Roles = "Admin")]
        // GET: ShopingCarts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCart = await _context.ShopingCarts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopingCart == null)
            {
                return NotFound();
            }

            return View(shopingCart);
        }
        [Authorize(Roles = "Admin")]
        // GET: ShopingCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShopingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Admin")]
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,Amount")] ShopingCart shopingCart)
        {
            if (ModelState.IsValid)
            {
                shopingCart.Id = Guid.NewGuid();
                _context.Add(shopingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shopingCart);
        }

        // GET: ShopingCarts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCart = await _context.ShopingCarts.FindAsync(id);
            if (shopingCart == null)
            {
                return NotFound();
            }
            return View(shopingCart);
        }

        // POST: ShopingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ApplicationUserId,Amount")] ShopingCart shopingCart)
        {
            if (id != shopingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopingCartExists(shopingCart.Id))
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
            return View(shopingCart);
        }
        [Authorize(Roles = "Admin")]
        // GET: ShopingCarts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCart = await _context.ShopingCarts
                .FirstOrDefaultAsync(m => m.Id == id);
            var book = await _addbook.GetBookAsync(shopingCart.BookId);

            if (shopingCart == null)
            {
                return NotFound();
            }
            shopingCart.Book.Name = book.Name;
            shopingCart.Book.Author = book.Author;
            shopingCart.Book.Price = book.Price;
            shopingCart.Book.Author = book.Author;
            return View(shopingCart);
        }

        // POST: ShopingCarts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shopingCart = await _context.ShopingCarts.FindAsync(id);
            _context.ShopingCarts.Remove(shopingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopingCartExists(Guid id)
        {
            return _context.ShopingCarts.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(ViewBook book)
        {
          
        ShopingCart shopingCart = new ShopingCart();
            if (book.Id == null)
            {
                return RedirectToAction("GetBooks","Books");
            }
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUser();              
                var a =await _context.ShopingCarts.Where(e => e.BookId == book.Id && e.ApplicationUserId.ToString()== user.Id).SingleOrDefaultAsync();
                if (a == null)
                {
                    shopingCart.Id = Guid.NewGuid();
                    shopingCart.ApplicationUserId = Guid.Parse(user.Id);
                    shopingCart.BookId = book.Id;
                    shopingCart.Amount = book.Amount;
                    _context.Add(shopingCart);                  
                }
                else
                {
                    a.Amount += book.Amount;
                    _context.ShopingCarts.Update(a);
                }
                  await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View();
        }
        [HttpGet]
        public  IActionResult AddToCart()
        {
                return RedirectToAction("Index", "Books");         

        }
        public  IActionResult Buy(string  Id)
        {
            var entyty = new ShopingCart
            {
                Id=Guid.Parse(Id),                
            };
            _context.ShopingCarts.Remove(entyty);
             _context.SaveChanges();
            return  View();
        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _manager.GetUserAsync(HttpContext.User);
        }
       

    }
}
