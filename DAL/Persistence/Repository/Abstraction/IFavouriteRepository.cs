using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Abstraction
{
    public interface IFavouriteRepository : IGenericRepository<Favorite>
    {
        Task<Favorite> GetByProductIdAndUserIdAsync(int productId, string userId);
        

    }
}
