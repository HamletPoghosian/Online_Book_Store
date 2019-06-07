using Microsoft.EntityFrameworkCore;
using Online_Book_Store.BookStore.Data;
using Online_Book_Store.BookStore.Service.CategoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Book_Store.BookStore.Service.Category
{
    public class CategoryServic : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryServic(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Data.Category> AddItemsAsync(Data.Category category)
        {
            var entity = new Data.Category
            {
                Id = category.Id,
                Name = category.Name,
                Discription = category.Discription
            };
            await _context.Categorys.AddAsync(entity);
            await _context.SaveChangesAsync();
            category.Id = entity.Id;
            return (category);
        }

        public async Task DeleteAsync(Data.Category category)
        {
           var catDb = _context.Categorys.SingleOrDefaultAsync(c => c.Id == category.Id);
            _context.Remove(catDb);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(Data.Category category)
        {
            var entity = await _context.Categorys.SingleOrDefaultAsync(e => e.Id == category.Id);
            entity.Id = category.Id;
            entity.Name = category.Name;
            entity.Discription = category.Discription;
            _context.Categorys.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Data.Category>> GetCategorysAsync()
        {
            var query = _context.Categorys.AsQueryable();
            var entity = await query.ToListAsync();
            return entity.Select(p => new Data.Category
            {
                Id = p.Id,               
                Name = p.Name,
                Discription=p.Discription    
            });
        }

        public async Task<Data.Category> GetCategoryAsync(Guid categoryId)
        {
            var entity = await _context.Categorys.SingleOrDefaultAsync(e => e.Id == categoryId);
            return new Data.Category
            {
                Id = entity.Id,                
                Name = entity.Name,
                Discription=entity.Discription
            };
        }

        public  List<string> GetCategoryDescription()
        {
            var query = _context.Categorys.AsQueryable();
            var listDesc =  query.Select(e => e.Discription).ToList();
            return (listDesc);
        }

        public List<string> GetCategoryName()
        {
            var query = _context.Categorys.AsQueryable();
            var listName = query.Select(e => e.Name).ToList();
            return (listName);
        }
        public IEnumerable<Data.Category> GetCategory()
        {
            var query = _context.Categorys;
            return query;
        }


    }
}
