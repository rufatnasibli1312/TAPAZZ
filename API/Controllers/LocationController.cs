using BLL.Persistence.Service.Abstract;
using DAL.Filter.ActionFilter;
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
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    public class LocationController : ControllerBase
    {
        public ILocationService _service { get; }
        public LocationController(ILocationService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {


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

                return BadRequest(ex.Message);
            }


        }
        [HttpGet("GetAllLocation")]
        public async Task<IActionResult> GetAllLocation()
        {
            
            try
            {
                var locations = await _service.GetAllAsync();
                if(locations == null)
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
            try
            {
                var products = await _service.GetProductsWithLocationId(id);
                if (products[0].Products.Count == 0)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }





    }
}
