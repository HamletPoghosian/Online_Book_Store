﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Models
{
    public class ViewBook
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public double Popular { get; set; }
        public double Price { get; set; }
        public DateTime Publish { get; set; }
        public Guid CategoryId { get; set; }      
        public string CategoryName { get; set; }
    }
}
