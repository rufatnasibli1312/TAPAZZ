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
    [Authorize(Roles = "Admin")]
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
        [HttpPost]
        public async Task<IActionResult> CreateLocation(LocationtoAddDTO locationToAddDto)
        {


            try
            {
                await _locationService.AddAsync(locationToAddDto);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }




        }

        [HttpPut]
        public async Task<IActionResult> UpdateLocation(LocationToUpdateDTO locationToUpdate)
        {


            try
            {

                await _locationService.UpdateAsync(locationToUpdate);
                return Ok(locationToUpdate);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLocation(DeleteLocationDto entity)
        {

            try
            {
                await _locationService.Delete(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }

}

