using DAL.Data;
using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Abstraction;
using DTO.ProductDto_s;
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

        public async Task<List<Product>> GetByFilterAsync(FindProductByFilter filter)
        {

            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                // Use EF.Functions.Like for case-insensitive comparison
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{filter.Name}%"));
            }

            if (!string.IsNullOrEmpty(filter.Description))
            {
                // Use EF.Functions.Like for case-insensitive comparison
                query = query.Where(p => EF.Functions.Like(p.Description, $"%{filter.Description}%"));
            }

            if (filter.CityId != 0)
            {
                query = query.Where(p => p.CityId == filter.CityId);
            }

            // Add other filtering conditions as needed

            return await query.ToListAsync();
        }
    }
}
