using DAL.Data;
using DAL.Persistence.Repository.Abstraction;
using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        public UserManager<User> _userManager { get; }
        public MyDbContext _context { get; }

        public AccountRepository(UserManager<User> userManager, MyDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<IdentityResult> RegisterUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<IdentityResult> AddRoleAsync(User user)
        {
            var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
            return addToRoleResult;
        }

        public async Task<User> FindUserById(string Id)
        {
            return await _userManager.FindByIdAsync(Id);   
        }

        public async Task<User> FindUserByEmail(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
        }
        public async Task SaveChanges()
        {
           await  _context.SaveChangesAsync();
        } 
    }
}
