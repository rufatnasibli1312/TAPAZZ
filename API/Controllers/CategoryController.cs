using BLL.Persistence.Service.Abstract;
using BLL.Persistence.Service.Concrete;
using DAL.Filter.ActionFilter;
using DTO.AccountDto_s;
using DTO.CategoryDto_s;
using DTO.ComplaintDto_s;
using DTO.LocationDto_s;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using Validation.CategoryValidator;
using Validation.UserValidator;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    public class CategoryController : ControllerBase
    {
        public ICategoryService _service { get; }
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task CreateCategory(CategoryToAddDto categoryToAdd)
        {
            await _service.AddAsync(categoryToAdd);


        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task UpdateCategory(UpdateCategoryDto categoryToUpdate)
        {
            await _service.UpdateAsync(categoryToUpdate);


        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task DeleteCategory(DeleteCategoryDTO entity)
        {

            await _service.Delete(entity);

        }

        [HttpGet]
        public async Task<CategoryFindIdDTO> GetCategory(int id)
        {


            return await _service.GetAsync(id);


        }


        [HttpGet("GetAllCategory")]
        public async Task<List<CategoryToListDto>> GetAllCategory()
        {

            return await _service.GetAllAsync();

        }
        [HttpGet("FindAllParentCategory")]
        public async Task<List<FindParentsCategoryDto>> FindAllParentCategory()
        {

            return await _service.FindParentCategory();
        }

        [HttpGet("GetChildCategory")]
        public async Task<List<GetChildCategoryWithParentCategoryId>> GetChildCategory(int id)
        {


            return await _service.GetChildCategoryWithParentCategoryId(id);

        }






    }
}
