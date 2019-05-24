using Book_Store.Service;

using BookStoreOnline.Service;
using Microsoft.EntityFrameworkCore;
using Online_Book_Store.BookStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Service
{
    public class BookService : IBookService
    {
        ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Book> AddItemsAsync(Book book)
        {
            
            var entity = new Book
            {
                Id = book.Id,
                Author = book.Author,
                Name = book.Name,
                Popular = book.Popular,
                Price = book.Price,
                CategoryId = book.CategoryId
            };
            await _context.Books.AddAsync(entity);
            await _context.SaveChangesAsync();
            book.Id = entity.Id;
            return book;
        }

        public async Task DeleteAsync(Book bookItems)
        {
            var entity = await _context.Books.SingleOrDefaultAsync(e => e.Id == bookItems.Id);
            _context.Books.Remove(entity);
            await _context.SaveChangesAsync();

        }

        public async Task EditAsync(Book bookItems)
        {
            var entity = await _context.Books.SingleOrDefaultAsync(e => e.Id == bookItems.Id);
            entity.Id = bookItems.Id;
            entity.Name = bookItems.Name;
            entity.Popular = bookItems.Popular;
            entity.Price = bookItems.Price;
            entity.Author = bookItems.Author;

            _context.Books.Update(entity);
            await _context.SaveChangesAsync();

        }

        public async Task<Book> GetBookAsync(Guid bookId)
        {
            var entity = await _context.Books.Include(b => b.Category).SingleOrDefaultAsync(e => e.Id == bookId);
            return new Book
            {
                Id = entity.Id,
                Author = entity.Author,
                Name = entity.Name,
                Popular = entity.Popular,
                Price = entity.Price,
                CategoryName = entity.Category.Name
            };

        }

        public async Task<IEnumerable<Book>> GetBooksAsync()

        {
            var query = _context.Books.Include(b => b.Category).AsQueryable();
            var entity = await query.ToListAsync();
            return entity.Select(p => new Book
            {
                Id = p.Id,
                Author = p.Author,
                Name = p.Name,
                Popular = p.Popular,
                Price = p.Price,
                Category= p.Category
            });

        }

    }
}
