using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Data
{
    public class ShopingCart
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public int Amount { get; set; }
    }
}
