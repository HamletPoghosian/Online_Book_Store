using Book_Store.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreOnline.Service
{
    public static class BookServiceextensions
    {
        public static IServiceCollection AddBook(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IBookService, BookService>();
        }
    }
}
