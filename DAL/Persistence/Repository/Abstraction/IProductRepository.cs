using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Abstraction
{
    public interface IProductRepository : IGenericRepository<Product>
    {

        Task<Product> GetProductsWithPhotoPath(int id);

        Task<List<Product>> GetByFilterAsync(FindProductByFilter filter);
    }
}
