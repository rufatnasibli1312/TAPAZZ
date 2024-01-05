using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Abstraction;
using DTO.CategoryDto_s;
using DTO.LocationDto_s;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Abstract
{
    public interface ICategoryService

    {
        Task AddAsync(CategoryToAddDto categoryDto);
        Task<CategoryFindIdDTO> GetAsync(int id);
        Task<List<CategoryToListDto>> GetAllAsync();
        Task Delete(DeleteCategoryDTO entity);
        Task UpdateAsync(UpdateCategoryDto categoryDto);





    }
}
