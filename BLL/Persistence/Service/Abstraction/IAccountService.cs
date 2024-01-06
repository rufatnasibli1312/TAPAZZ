using DTO.AccountDto_s;
using DTO.JWTDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Abstraction
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterModelDto model, string webRootPath);
        Task<User> FindUserById(string userId);
        Task<ResponseDto> Login(LoginDto loginDto);
        Task<User> FindUserByEmail(string email);
        Task UpdateAsync(UpdateUserDto updateUserDto, string webRootPath);

        

        // delete sohbetleri qalib burda



    }
}
