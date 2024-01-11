using BLL.Persistence.Service.Abstraction;
using DAL.Filter.ActionFilter;
using DTO;
using DTO.ComplaintDto_s;
using DTO.FavouriteDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;
using Validation.FavouriteValidator;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    [Authorize(Roles = "User")]
    public class FavouriteController : ControllerBase
    {
        public IFavouriteService _favouriteService { get; }

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavourites(FavouriteAddDto favouriteAddDto)
        {
            try
            {

                await _favouriteService.AddAsync(favouriteAddDto);

                return Ok(favouriteAddDto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [HttpGet("GetAllFavourite")]
        public async Task<IActionResult> GetAllFavourite()
        {

            try
            {
                var favourites = await _favouriteService.GetAllAsync();

                return Ok(favourites);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetFavourite(int id)
        {

            try
            {
                var favourite = await _favouriteService.GetAsync(id);

                return Ok(favourite);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteFavouriteDto entity)
        {


            try
            {

                await _favouriteService.Delete(entity);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }



        }


    }
}
