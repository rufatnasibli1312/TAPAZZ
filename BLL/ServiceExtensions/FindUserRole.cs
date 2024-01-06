using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceExtensions
{
    public class FindUserRole
    {

        public UserManager<User> _userManager { get; }

        public FindUserRole(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public async Task<string> GetUserRole(User user)
        {

            var userRole = await _userManager.GetRolesAsync(user);


            if (userRole != null && userRole.Any())
            {
                return userRole.First();
            }
            return "User";

        }


    }
}
