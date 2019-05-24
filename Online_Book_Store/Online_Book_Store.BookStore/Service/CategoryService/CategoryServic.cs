using Online_Book_Store.BookStore.Data;
using Online_Book_Store.BookStore.Service.CategoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Service.Category
{
    public  class CategoryServic:ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryServic(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Data.Category> AddItemsAsync(Data.Category category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Data.Category category)
        {
            throw new NotImplementedException();
        }

        public Task EditAsync(Data.Category category)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetCategoryAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetCategoryDescription()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetCategoryName()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Data.Category>> GetCategorysAsync()
        {
            throw new NotImplementedException();
        }
    }
}
