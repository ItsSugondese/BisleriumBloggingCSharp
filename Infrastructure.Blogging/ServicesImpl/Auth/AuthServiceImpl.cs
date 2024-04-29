using Application.Blogging.Auth;
using Domain.Blogging;
using Domain.Blogging.view.auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.ServicesImpl.Auth
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthServiceImpl(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task registerUser(RegisterViewModel model)
        {
           

            var user = new AppUser { UserName = model.Email, Email = model.Email, CreatedAt = DateTime.UtcNow};

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                throw new Exception("Invalid role specified.");
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            else
            {

                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            
                
            
        }
    }
}
