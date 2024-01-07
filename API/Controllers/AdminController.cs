using BLL.Persistence.Service.Abstract;
using DAL.Filter.ActionFilter;
using DTO.CategoryDto_s;
using DTO.LocationDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;
using Validation.CategoryValidator;
using Validation.LocationValidator;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    public class AdminController : ControllerBase
    {

        public ICategoryService _categoryservice { get; }
        public ILocationService _locationService { get; }

        public AdminController(ICategoryService Categoryservice, ILocationService locationService)
        {
            _categoryservice = Categoryservice;
            _locationService = locationService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryToAddDto categoryToAdd)
        {
            try
            {
                await _categoryservice.AddAsync(categoryToAdd);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto categoryToUpdate)
        {

            try
            {

                await _categoryservice.UpdateAsync(categoryToUpdate);

                return Ok(categoryToUpdate);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryDTO entity)
        {

            {
                try
                {
                    await _categoryservice.Delete(entity);
                    return NoContent();
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }




            }


        }
        [HttpPost]

        public async Task<IActionResult> CreateLocation(LocationtoAddDTO locationToAddDto)
        {
            var model = JsonSerializer.Serialize(locationToAddDto);
            Log.Information($"{nameof(LocationController)}.{nameof(CreateLocation)}({model})");
            LocationtoAddValidator validator = new LocationtoAddValidator();
            var result = validator.Validate(locationToAddDto);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {

                    await _locationService.AddAsync(locationToAddDto);
                    return Ok();
                }
                catch (Exception ex)
                {

                    Log.Error($"{nameof(LocationController)}.{nameof(CreateLocation)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(LocationController)}.{nameof(CreateLocation)}({model})");
            return BadRequest(error);


        }
        [HttpPut]
        public async Task<IActionResult> UpdateLocation(LocationToUpdateDTO locationToUpdate)
        {
            var model = JsonSerializer.Serialize(locationToUpdate);
            Log.Information($"{nameof(LocationController)}.{nameof(UpdateLocation)}({model})");
            LocationToUpdateValidator validator = new LocationToUpdateValidator();
            var result = validator.Validate(locationToUpdate);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    if (locationToUpdate == null)
                    {
                        return BadRequest();
                    }

                    await _locationService.UpdateAsync(locationToUpdate);
                    return Ok(locationToUpdate);
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(LocationController)}.{nameof(UpdateLocation)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(LocationController)}.{nameof(UpdateLocation)}({model})");
            return BadRequest(error);




        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLocation(DeleteLocationDto entity)
        {
            var model = JsonSerializer.Serialize(entity);
            Log.Information($"{nameof(LocationController)}.{nameof(DeleteLocation)}({model})");
            DeleteLocationValidation validator = new DeleteLocationValidation();
            var result = validator.Validate(entity);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    var existingLocation = await _locationService.GetAsync(entity.Id);
                    if (existingLocation == null)
                    {
                        return NotFound();
                    }
                    await _locationService.Delete(entity);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(LocationController)}.{nameof(DeleteLocation)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(LocationController)}.{nameof(DeleteLocation)}({model})");
            return BadRequest(error);


        }

    }
}
