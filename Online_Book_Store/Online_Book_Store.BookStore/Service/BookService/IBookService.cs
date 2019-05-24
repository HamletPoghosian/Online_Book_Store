
using Online_Book_Store.BookStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreOnline.Service
{
    public interface IBookService
    {
        Task<Book> AddItemsAsync(Book book);
        Task EditAsync(Book book);
        Task DeleteAsync(Book bookItems);
        Task<Book> GetBookAsync(Guid bookId);
        Task<IEnumerable<Book>> GetBooksAsync();
    }
}
