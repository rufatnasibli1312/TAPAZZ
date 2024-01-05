using DTO;
using DTO.ComplaintDto_s;
using DTO.FavouriteDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Abstraction
{
    public interface IFavouriteService
    {
        Task AddAsync(FavouriteAddDto favouriteAddDto);
        Task<List<FavouriteGetProductDto>> GetAllAsync();
        Task<FavouriteGetProductDto> GetAsync(int id);
        Task Delete(DeleteFavouriteDto entity);


    }
}
