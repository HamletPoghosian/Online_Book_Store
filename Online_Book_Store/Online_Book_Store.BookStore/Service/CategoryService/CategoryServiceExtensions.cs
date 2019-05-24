using Microsoft.Extensions.DependencyInjection;
using Online_Book_Store.BookStore.Service.CategoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Service.Category
{
    public static class CategoryServiceExtensions
    {
        public static IServiceCollection AddCategory(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<ICategoryService, CategoryServic>();
        }
    }
}
