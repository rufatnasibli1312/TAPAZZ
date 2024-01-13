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

        Task AddAsync(LocationtoAddDTO locationDto);
        Task<LocationFindIdDTO> GetAsync(int id);
        Task<List<LocationToListDto>> GetAllAsync();
        Task Delete(DeleteLocationDto entity);
        Task UpdateAsync(LocationToUpdateDTO locationToUpdateDTO);

        
    }
}
