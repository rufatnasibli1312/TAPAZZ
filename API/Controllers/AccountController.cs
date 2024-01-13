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
    [Route("api/[controller]")]
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
        public async Task<IdentityResult> Register([FromForm] RegisterModelDto model)
        {
            return await _accountService.RegisterUserAsync(model, _webHostEnvironment.WebRootPath);

        }

        [HttpGet]
        public async Task<User> GetUserById(string userId)
        {

            return await _accountService.FindUserById(userId);

        }

        [HttpGet("GetUserByEmail")]
        public async Task<User> GetUserByEmail(string email)
        {

            return await _accountService.FindUserByEmail(email);


        }


        [HttpPost("Login")]
        public async Task<ResponseDto> Login(LoginDto loginDto)
        {

            return await _accountService.Login(loginDto);

        }

        [HttpPut]
        [Authorize]
        public async Task UpdateUser([FromForm] UpdateUserDto entity)
        {

            await _accountService.UpdateAsync(entity, _webHostEnvironment.WebRootPath);

        }
        [Authorize]
        [HttpDelete]
        public async Task DeleteUser(DeleteUserDto dto)
        {

            await _accountService.DeleteAsync(dto);
        }
    }
}
