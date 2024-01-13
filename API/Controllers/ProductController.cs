using BLL.Persistence.Service.Abstract;
using BLL.Persistence.Service.Abstraction;
using DAL.Filter.ActionFilter;
using DTO.LocationDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;
using Validation.ProductValidator;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    public class ProductController : ControllerBase
    {
        public IProductService _productService { get; }
        public IWebHostEnvironment _webHostEnvironment { get; }
        public ICityService _locationService { get; }

        public ProductController(IProductService productService, IWebHostEnvironment webHostEnvironment, ICityService locationService)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
            _locationService = locationService;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task CreateProduct([FromForm] ProductAddDto productAddDto)
        {

            await _productService.AddAsync(productAddDto, _webHostEnvironment.WebRootPath);

        }


        [HttpGet("GetAllProducts")]
        public async Task<List<ProductToListDto>> GetALlProducts()
        {

            return await _productService.GetAllAsync();

        }
        [HttpGet]
        public async Task<ProductFindIdDto> GetProduct(int id)
        {

            return await _productService.GetAsync(id);

        }

        [HttpGet("UserId")]
        [Authorize(Roles = "User")]
        public async Task<List<ProductToListDto>> GetMyProducts()
        {

            return await _productService.GetMyProductsAsync();

        }
        [HttpDelete]
        [Authorize]
        public async Task DeleteProduct(DeleteProductDto entity)
        {

            await _productService.Delete(entity);

        }
        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task UpdateProduct([FromForm] UpdateProductDto entity)
        {

            await _productService.UpdateAsync(entity, _webHostEnvironment.WebRootPath);

        }


        [HttpGet("ByFilter")]
        public async Task<List<FindProductByFilter>> GetByFilter([FromQuery] FindProductByFilter filter)
        {

            return await _productService.GetByFilter(filter);


        }



    }

}

