using BLL.Persistence.Service.Abstraction;
using DAL.Filter.ActionFilter;
using DTO.AccountDto_s;
using DTO.JWTDto_s;
using DTO.LocationDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;
using Validation.UserValidator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    public class AccountController : ControllerBase
    {
        public IAccountService _accountService { get; }
        public IWebHostEnvironment _webHostEnvironment { get; }
        public IHttpContextAccessor _httpContextAccessor { get; }

        public AccountController(IAccountService accountService, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterModelDto model)
        {
            var jsonModel = JsonSerializer.Serialize(model);
            Log.Information($"{nameof(AccountController)}.{nameof(Register)}({jsonModel}");

            RegisterValidation validator = new RegisterValidation();
            var result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                return BadRequest(new { Errors = errorMessages });
            }

            try
            {
                var user = await _accountService.RegisterUserAsync(model, _webHostEnvironment.WebRootPath);
                if (user.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new { Errors = user.Errors });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(AccountController)}.{nameof(Register)}({jsonModel}");
                return BadRequest();
            }










        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(string userId)
        {

            Log.Information($"{nameof(AccountController)}.{nameof(GetUserById)}({userId}");
            try
            {
                User user = await _accountService.FindUserById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(AccountController)}.{nameof(GetUserById)}({userId}");
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            Log.Information($"{nameof(AccountController)}.{nameof(GetUserByEmail)}({email}");
            try
            {
                User user = await _accountService.FindUserByEmail(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(AccountController)}.{nameof(GetUserByEmail)}({email}");
                return BadRequest(ex.Message);

            }




        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var jsonModel = JsonSerializer.Serialize(loginDto);
            Log.Information($"{nameof(AccountController)}.{nameof(Login)}({jsonModel}");
            LoginValidator validator = new LoginValidator();
            var result = validator.Validate(loginDto);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    var response = await _accountService.Login(loginDto);
                    Response.Cookies.Append("X-Access-Token", response.AccessToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                    Response.Cookies.Append("X-Refresh-Token", response.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });





                    return Ok(response);
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(AccountController)}.{nameof(Login)}({jsonModel}");
                    return BadRequest(ex.Message);
                }

            }
            Log.Error($"{nameof(AccountController)}.{nameof(Login)}({jsonModel}");
            return BadRequest(error);



        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto entity)
        {
            var model = JsonSerializer.Serialize(entity);
            Log.Information($"{nameof(AccountController)}.{nameof(UpdateUser)}({model})");
            try
            {
                if (entity == null)
                {
                    return BadRequest();           //vaxt olsa buralarada validator yazarsan
                }
                await _accountService.UpdateAsync(entity, _webHostEnvironment.WebRootPath);
                return Ok(entity);
               
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(AccountController)}.{nameof(UpdateUser)}({model})");
                return BadRequest(ex.Message);
            }

        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(DeleteUserDto dto)
        {
            var model = JsonSerializer.Serialize(dto);
            Log.Information($"{nameof(AccountController)}.{nameof(DeleteUser)}({model})");
            try
            {
                var result = await _accountService.DeleteAsync(dto);
                if (result)
                {
                    return Ok(result);

                }
                return BadRequest(result);


            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(AccountController)}.{nameof(DeleteUser)}({model})");
                return BadRequest(ex.Message);
            }
        }
    }
}
