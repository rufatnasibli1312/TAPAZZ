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




            try
            {
                var user = await _accountService.RegisterUserAsync(model, _webHostEnvironment.WebRootPath);
                return Ok();
            }
            catch (Exception ex)
            {

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

            try
            {
                var response = await _accountService.Login(loginDto);
                Response.Cookies.Append("X-Access-Token", response.AccessToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                Response.Cookies.Append("X-Refresh-Token", response.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });


                return Ok(response);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }






        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto entity)
        {

            try
            {

                await _accountService.UpdateAsync(entity, _webHostEnvironment.WebRootPath);
                return Ok(entity);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(DeleteUserDto dto)
        {

            try
            {
                await _accountService.DeleteAsync(dto);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
