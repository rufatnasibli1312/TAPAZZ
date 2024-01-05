using DAL.Data;
using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Abstraction;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Implementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public MyDbContext _context { get; }
        public ProductRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> GetProductsWithPhotoPath(int id)
        {

            var product = await _context.Products
         .Include(p => p.PhotoPath)
         .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }
    }
}
