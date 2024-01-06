using BLL.Persistence.Service.Abstract;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    public class CategoryController : ControllerBase
    {
        public ICategoryService _service { get; }
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
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
        public async Task<IActionResult> GetAllCategory()
        {
            Log.Information($"{nameof(CategoryController)}.{nameof(GetAllCategory)}()");
            try
            {


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
        [HttpGet]
        public async Task<IActionResult> FindParentCategory()
        {
            Log.Information($"{nameof(CategoryController)}.{nameof(FindParentCategory)}()");
            try
            {
                var category = await _service.FindParentCategory();
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(CategoryController)}.{nameof(FindParentCategory)}()");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetChildCategoryWithParentCategoryId(int id)
        {
            Log.Information($"{nameof(CategoryController)}.{nameof(GetChildCategoryWithParentCategoryId)}()");
            try
            {
                var category = await _service.GetChildCategoryWithParentCategoryId(id);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(CategoryController)}.{nameof(GetChildCategoryWithParentCategoryId)}()");
                return BadRequest(ex.Message);
            }
        }






    }
}
