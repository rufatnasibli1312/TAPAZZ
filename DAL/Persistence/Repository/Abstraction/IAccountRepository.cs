using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Abstraction
{
    public interface IAccountRepository
    {
        Task<IdentityResult> RegisterUserAsync(User user, string password);
        Task<User> FindUserById(string Id);
        Task<User> FindUserByEmail(string Email);
        Task SaveChanges();
        Task<IdentityResult> AddRoleAsync(User user);




    }
}
