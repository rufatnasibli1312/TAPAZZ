using DAL.Data;
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
    public class FavouriteRepository : GenericRepository<Favorite>, IFavouriteRepository
    {
        public MyDbContext _context { get; }
        public FavouriteRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Favorite> GetByProductIdAndUserIdAsync(int productId, string userId)
        {
            return await _context.Favorites
            .FirstOrDefaultAsync(f => f.ProductId == productId && f.UserId == userId);
        }

       
    }
}
