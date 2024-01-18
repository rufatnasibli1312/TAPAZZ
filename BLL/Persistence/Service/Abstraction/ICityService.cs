using DTO.LocationDto_s;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Abstract
{
    public interface ICityService
    {

        Task AddAsync(CitytoAddDTO locationDto);
        Task<CityFindIdDTO> GetAsync(int id);
        Task<List<CityToListDto>> GetAllAsync();
        Task Delete(DeleteCityDto entity);
        Task UpdateAsync(CityToUpdateDTO locationToUpdateDTO);

        
    }
}
