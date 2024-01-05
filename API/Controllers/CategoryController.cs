using BLL.Persistence.Service.Abstract;
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

    public class CategoryController : ControllerBase
    {
        public ICategoryService _service { get; }
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryToAddDto categoryToAdd)
        {

            var model = JsonSerializer.Serialize(categoryToAdd);
            Log.Information($"{nameof(CategoryController)}.{nameof(CreateCategory)}({model})");
            CategoryToAddValidator validator = new CategoryToAddValidator();
            var result = validator.Validate(categoryToAdd);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    await _service.AddAsync(categoryToAdd);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(CategoryController)}.{nameof(CreateCategory)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(CategoryController)}.{nameof(CreateCategory)}({model})");
            return BadRequest(error);





        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            Log.Information($"{nameof(CategoryController)}.{nameof(GetCategory)}({id})");
            try
            {
                var category = await _service.GetAsync(id);
                if (category == null)
                {
                    return NotFound();

                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(CategoryController)}.{nameof(GetCategory)}({id})");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCategory()
        {
            Log.Information($"{nameof(CategoryController)}.{nameof(GetAllCategory)}()");
            try
            {
                var isAdmin = User.IsInRole("Admin");
                //var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value);

                var category = await _service.GetAllAsync();
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);

            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(CategoryController)}.{nameof(GetAllCategory)}()");
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto categoryToUpdate)
        {
            var model = JsonSerializer.Serialize(categoryToUpdate);
            Log.Information($"{nameof(CategoryController)}.{nameof(UpdateCategory)}({model})");
            UpdateCategoryValidator validator = new UpdateCategoryValidator();
            var result = validator.Validate(categoryToUpdate);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    if (categoryToUpdate == null)
                    {
                        return BadRequest();
                    }

                    await _service.UpdateAsync(categoryToUpdate);
                    return Ok(categoryToUpdate);
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(CategoryController)}.{nameof(UpdateCategory)}({model})");
                    return BadRequest(ex.Message);
                }

            }
            Log.Error($"{nameof(CategoryController)}.{nameof(UpdateCategory)}({model})");
            return BadRequest(error);







        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryDTO entity)
        {
            var model = JsonSerializer.Serialize(entity);
            Log.Information($"{nameof(CategoryController)}.{nameof(DeleteCategory)}({model})");
            DeleteCategoryValidator validator = new DeleteCategoryValidator();
            var result = validator.Validate(entity);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    var existingCategory = await _service.GetAsync(entity.Id);
                    if (existingCategory == null)
                    {
                        return NotFound();
                    }
                    await _service.Delete(entity);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(CategoryController)}.{nameof(DeleteCategory)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(CategoryController)}.{nameof(DeleteCategory)}({model})");
            return BadRequest(error);





        }

    }
}
