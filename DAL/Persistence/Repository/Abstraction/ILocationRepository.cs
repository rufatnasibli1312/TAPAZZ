using DAL.Persistence.Repository.Abstraction;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Abstract
{
    public interface ILocationRepository :IGenericRepository<Location>
    {
        Task<List<Location>> GetProductsWithLocationId(int id);

    }
}
