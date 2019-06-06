using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Models
{
    public class ViewSearch
    {
        public IEnumerable<ViewBook> Books { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public double Popular { get; set; }
        public DateTime Publish { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
    }
}
