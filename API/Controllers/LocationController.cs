using BLL.Persistence.Service.Abstract;
using DTO.CategoryDto_s;
using DTO.LocationDto_s;

using Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Validation.LocationValidator;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        public ILocationService _service { get; }
        public LocationController(ILocationService service)
        {
            _service = service;
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

                    await _service.AddAsync(locationToAddDto);
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {

            Log.Information($"{nameof(LocationController)}.{nameof(GetLocation)}({id})");
            try
            {
                var location = await _service.GetAsync(id);
                if (location == null)
                {
                    return NotFound();
                }
                return Ok(location);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(LocationController)}.{nameof(GetLocation)}({id})");
                return BadRequest(ex.Message);
            }


        }
        [HttpGet("GetAllLocation")]
        public async Task<IActionResult> GetAllLocation()
        {
            Log.Information($"{nameof(LocationController)}.{nameof(GetAllLocation)}()");
            try
            {
                var locations = await _service.GetAllAsync();
                if (locations == null)
                {
                    return NotFound();
                }
                return Ok(locations);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(LocationController)}.{nameof(GetAllLocation)}()");
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetProductsWithLocationId")]
        public async Task<IActionResult> GetProductsWithLocationId(int id)
        {
           var products =  await _service.GetProductsWithLocationId(id);
            if(products == null)
            {
                return BadRequest();
            }
            return Ok(products);
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

                    await _service.UpdateAsync(locationToUpdate);
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
            if(result.IsValid)
            {
                try
                {
                    var existingLocation = await _service.GetAsync(entity.Id);
                    if (existingLocation == null)
                    {
                        return NotFound();
                    }
                    await _service.Delete(entity);
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
