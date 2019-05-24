using Online_Book_Store.BookStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Service.CategoryService
{
    interface ICategoryService
    {
        Task<Data.Category> AddItemsAsync(Data.Category category);
        Task EditAsync(Data.Category category);
        Task DeleteAsync(Data.Category category);
        Task<Book> GetCategoryAsync(Guid categoryId);
        Task<IEnumerable<Data.Category>> GetCategorysAsync();
        Task<List<string>> GetCategoryName();
        Task<List<string>> GetCategoryDescription();
    }
}
