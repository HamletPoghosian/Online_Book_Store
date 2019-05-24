using Online_Book_Store.BookStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Service.CategoryService
{
    public interface ICategoryService
    {
        Task<Data.Category> AddItemsAsync(Data.Category category);
        Task EditAsync(Data.Category category);
        Task DeleteAsync(Data.Category category);
        Task<Data.Category> GetCategoryAsync(Guid categoryId);        
        List<string> GetCategoryName();
        List<string> GetCategoryDescription();
        Task<IEnumerable<Data.Category>> GetCategorysAsync();
        IEnumerable<Data.Category> GetCategory();
    }
}
