using BLL.Persistence.Service.Abstract;
using BLL.Persistence.Service.Concrete;
using DAL.Filter.ActionFilter;
using DTO.CategoryDto_s;
using DTO.LocationDto_s;

using Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Validation.LocationValidator;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    public class CityController : ControllerBase
    {
        public ICityService _service { get; }
        public CityController(ICityService service)
        {
            _service = service;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task CreateCity(LocationtoAddDTO locationToAddDto)
        {

            await _service.AddAsync(locationToAddDto);

        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task UpdateCity(LocationToUpdateDTO locationToUpdate)
        {

            await _service.UpdateAsync(locationToUpdate);

        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task DeleteCity(DeleteLocationDto entity)
        {

            await _service.Delete(entity);
        }
        [HttpGet]
        public async Task<LocationFindIdDTO> GetCity(int id)
        {


            return await _service.GetAsync(id);


        }
        [HttpGet("GetAllCities")]
        public async Task<List<LocationToListDto>> GetAllCities()
        {

            return await _service.GetAllAsync();
        }






    }
}
