using BLL.Persistence.Service.Abstraction;
using DAL.Filter.ActionFilter;
using DTO;
using DTO.ComplaintDto_s;
using DTO.FavouriteDto_s;
using DTO.ProductDto_s;
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
        public async Task AddFavourites(FavouriteAddDto favouriteAddDto)
        {

            await _favouriteService.AddAsync(favouriteAddDto);


        }

        [HttpGet("GetAllFavourite")]
        public async Task<List<FavouriteGetProductDto>> GetAllFavourite()
        {

            return await _favouriteService.GetAllAsync();
        }
        [HttpGet]
        public async Task<FavouriteGetProductDto> GetFavourite(int id)
        {


            return await _favouriteService.GetAsync(id);


        }
        [HttpDelete]
        public async Task Delete(DeleteFavouriteDto entity)
        {

            await _favouriteService.Delete(entity);

        }


    }
}
