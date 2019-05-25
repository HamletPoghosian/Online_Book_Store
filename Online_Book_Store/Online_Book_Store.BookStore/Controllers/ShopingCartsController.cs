using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Book_Store.BookStore.Data;

namespace Online_Book_Store.BookStore.Controllers
{
    public class ShopingCartsController : Controller
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ApplicationDbContext _context;

        public ShopingCartsController(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _manager = manager;
            _context = context;
        }

        // GET: ShopingCarts
        public async Task<IActionResult> Index()
        {
            return View(await _context.ShopingCarts.ToListAsync());
        }

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

        // GET: ShopingCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShopingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: ShopingCarts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: ShopingCarts/Delete/5
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


        [Authorize]
        public async Task<IActionResult> AddToCart(Guid? id, int amount)
        {
          
        ShopingCart shopingCart = new ShopingCart();
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUser();               
                
                shopingCart.Id = Guid.NewGuid();
                shopingCart.ApplicationUserId = Guid.Parse(user.Id);
                shopingCart.BookId= id.Value;
                shopingCart.Amount = amount;
                _context.Add(shopingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View();
        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _manager.GetUserAsync(HttpContext.User);
        }
    }
}
