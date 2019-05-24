using Online_Book_Store.BookStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Service.Category
{
    public  class CategoryServic
    {
        private readonly ApplicationDbContext _context;
        public CategoryServic(ApplicationDbContext context)
        {
            _context = context;
        }
        public static List<string> GetCategoryName()
        {
            var categoryList = _context.Categorys.Select(c=>c.Name).ToList();
            return (categoryList);
        }

    }
}
