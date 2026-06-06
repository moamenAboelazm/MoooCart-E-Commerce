using Microsoft.AspNetCore.Identity;
using MoooCart.id.Authentication.Base;
using MoooCart.id.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.id.Authentication.Repositories
{
    public class RoleManagement(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager) : IRoleManagement
    {
        public async Task<bool> AddUserToRole(AppUser user, string rolename)
        {
            if (!await roleManager.RoleExistsAsync(rolename))
            {
                await roleManager.CreateAsync(new IdentityRole(rolename));
            }

            return (await userManager.AddToRoleAsync(user, rolename)).Succeeded;
        }

        public async Task<string?> GetUserRole(string UserEmail)
        {
            var user = await userManager.FindByEmailAsync(UserEmail);
            return (await userManager.GetRolesAsync(user!)).FirstOrDefault();
        }
    }
}
