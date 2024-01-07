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
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public MyDbContext _context { get; }
        public LocationRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetProductsWithLocationId(int id)
        {
            var products = await _context.Locations.Include(m => m.Products).Where(p => p.Id == id).ToListAsync();
            
            return products;
        }
    }
}
