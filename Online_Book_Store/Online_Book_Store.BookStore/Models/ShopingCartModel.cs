using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Models
{
    public class ShopingCartModel
    {
        public Guid Id { get; set; }
        public ViewBook Book { get; set; }
        public int Amount { get; set; }
        public double TotalPrice { get; set; }


    }
}
