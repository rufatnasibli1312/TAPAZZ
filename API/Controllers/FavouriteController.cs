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
    public class FavouriteController : ControllerBase
    {
        public IFavouriteService _favouriteService { get; }

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFavourites(FavouriteAddDto favouriteAddDto)
        {
            var model = JsonSerializer.Serialize(favouriteAddDto);
            Log.Information($"{nameof(FavouriteController)}.{nameof(AddFavourites)}({model})");
            FavouriteAddValidator validator = new FavouriteAddValidator();
            var result = validator.Validate(favouriteAddDto);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {

                    await _favouriteService.AddAsync(favouriteAddDto);

                    return Ok(favouriteAddDto);
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(FavouriteController)}.{nameof(AddFavourites)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(FavouriteController)}.{nameof(AddFavourites)}({model})");
            return BadRequest(error);

        }

        [HttpGet("GetAllFavourite")]
        [Authorize]
        public async Task<IActionResult> GetAllFavourite()
        {
            Log.Information($"{nameof(FavouriteController)}.{nameof(GetAllFavourite)}()");
            try
            {
                var favourites = await _favouriteService.GetAllAsync();
                if (favourites == null)
                {
                    return BadRequest();

                }
                return Ok(favourites);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(FavouriteController)}.{nameof(GetAllFavourite)}()");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFavourite(int id)
        {
            Log.Information($"{nameof(FavouriteController)}.{nameof(GetFavourite)}({id})");
            try
            {
                var favourite = await _favouriteService.GetAsync(id);
                if (favourite == null)
                {
                    return BadRequest();
                }
                return Ok(favourite);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(FavouriteController)}.{nameof(GetFavourite)}({id})");
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteFavouriteDto entity)
        {
            var model = JsonSerializer.Serialize(entity);
            Log.Information($"{nameof(FavouriteController)}.{nameof(Delete)}({model})");
            DeleteFavouriteValidator validator = new DeleteFavouriteValidator();
            var result = validator.Validate(entity);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    var favourite = await _favouriteService.GetAsync(entity.Id);

                    if (favourite == null)
                    {
                        return NotFound();
                    }
                    await _favouriteService.Delete(entity);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(FavouriteController)}.{nameof(Delete)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(FavouriteController)}.{nameof(Delete)}({model})");
            return BadRequest(error);
           

        }


    }
}
