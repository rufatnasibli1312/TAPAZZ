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
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromForm] ProductAddDto productAddDto)
        {
            var model = JsonSerializer.Serialize(productAddDto);
            Log.Information($"{nameof(ProductController)}.{nameof(CreateProduct)}({model})");
            ProductAddValidator validator = new ProductAddValidator();
            var result = validator.Validate(productAddDto);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    await _productService.AddAsync(productAddDto, _webHostEnvironment.WebRootPath);
                    return Ok();
                }
                catch (Exception ex)
                {

                    Log.Error($"{nameof(ProductController)}.{nameof(CreateProduct)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(ProductController)}.{nameof(CreateProduct)}({model})");
            return BadRequest(error);


        }
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetALlProducts()
        {
            Log.Information($"{nameof(ProductController)}.{nameof(GetALlProducts)}()");
            try
            {
                var products = await _productService.GetAllAsync();
                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ProductController)}.{nameof(GetALlProducts)}()");
                return BadRequest(ex.Message);
            }


        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetProduct(int id)
        {
            Log.Information($"{nameof(ProductController)}.{nameof(GetProduct)}({id})");
            try
            {
                var product = await _productService.GetAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);

            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ProductController)}.{nameof(GetProduct)}({id})");
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("UserId")]
        [Authorize]
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
            var model = JsonSerializer.Serialize(entity);
            Log.Information($"{nameof(ProductController)}.{nameof(DeleteProduct)}({model})");
            DeleteProductValidator validator = new DeleteProductValidator();
            var validate = validator.Validate(entity);
            var error = validate.Errors.Select(m => m.ErrorMessage).ToList();
            if (validate.IsValid)
            {
                try
                {
                    var product = await _productService.GetAsync(entity.Id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    var result = await _productService.Delete(entity);
                    if (result)
                    {
                        return NoContent();
                    }
                    return BadRequest(result);

                }
                catch
                {
                    Log.Error($"{nameof(ProductController)}.{nameof(DeleteProduct)}({model})");
                    return BadRequest();
                }

            }
            Log.Error($"{nameof(ProductController)}.{nameof(DeleteProduct)}({model})");
            return BadRequest(error);


        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto entity)
        {
            var model = JsonSerializer.Serialize(entity);
            Log.Information($"{nameof(ProductController)}.{nameof(UpdateProduct)}({model})");
            UpdateProductValidator validator = new UpdateProductValidator();
            var validate = validator.Validate(entity);
            var error = validate.Errors.Select(m => m.ErrorMessage).ToList();
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
