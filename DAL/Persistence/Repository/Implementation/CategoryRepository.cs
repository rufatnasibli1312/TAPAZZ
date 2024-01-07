using DAL.Data;
using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Implementation;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Concrete
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public MyDbContext _context { get; }
        public CategoryRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> FindIsItParentCategoryOrNot(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category.ParentCategoryId == null)
            {
                return true;
            }
            return false;

        }
    }
}
