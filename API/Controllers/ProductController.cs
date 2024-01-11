using BLL.Persistence.Service.Abstraction;
using DAL.Filter.ActionFilter;
using DTO.LocationDto_s;
using DTO.ProductDto_s;
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

        public ProductController(IProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductAddDto productAddDto)
        {

            try
            {
                await _productService.AddAsync(productAddDto, _webHostEnvironment.WebRootPath);
                return Ok();
            }
            catch (Exception ex)
            {


                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetALlProducts()
        {

            try
            {
                var products = await _productService.GetAllAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetProduct(int id)
        {

            try
            {
                var product = await _productService.GetAsync(id);

                return Ok(product);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [HttpGet("UserId")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyProducts()
        {
            Log.Information($"{nameof(ProductController)}.{nameof(GetMyProducts)}()");
            try
            {
                var products = await _productService.GetMyProductsAsync();
                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ProductController)}.{nameof(GetMyProducts)}()");
                return BadRequest(ex.Message);
            }


        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(DeleteProductDto entity)
        {


            try
            {
                await _productService.Delete(entity);

                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }





        }
        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto entity)
        {
            
            if (validate.IsValid)
            {
                try
                {
                    if (entity == null)
                    {
                        return BadRequest();
                    }
                    await _productService.UpdateAsync(entity, _webHostEnvironment.WebRootPath);
                    return Ok(entity);
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(ProductController)}.{nameof(UpdateProduct)}({model})");
                    return BadRequest();
                }
            }
            Log.Error($"{nameof(ProductController)}.{nameof(UpdateProduct)}({model})");
            return BadRequest(error);


        }

    }
}
