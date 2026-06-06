using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoooCart.id.Authentication.Base;
using MoooCart.id.Identity;
using System.Security.Claims;

namespace MoooCart.id.Authentication.Repositories
{
    public class UserManagement(IRoleManagement role, UserManager<AppUser> userManager) : IUserManagement
    {
        public async Task<bool> CreateUser(AppUser user)
        {
            var _user = await GetUserByEmail(user.Email!);
            if (_user == null)
            {
                var result = await userManager.CreateAsync(user, user.PasswordHash!);
                if (!result.Succeeded)
                {
                    var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                    throw new Exception($"UserManager Error: {errors}");
                }

                return true;
            }
            return false;
        }

        public async Task<int> DeleteUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return 0;

            var result = await userManager.DeleteAsync(user);
            return result.Succeeded ? 1 : 0;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<AppUser?> GetUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser?> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<List<Claim>> GetUserClaims(string email)
        {
            var user = await GetUserByEmail(email);
            string? roleName = await role.GetUserRole(user!.Email!);

            List<Claim> claims =
            [
                new Claim("fullName", user.FullName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, roleName ?? "User"),
            ];
            return claims;
        }

        public async Task<bool> LoginUser(AppUser user)
        {
            var _user = await GetUserByEmail(user.Email!);
            if (_user == null) return false;

            var roleName = await role.GetUserRole(user.Email!);
            if (string.IsNullOrEmpty(roleName)) return false;

            return await userManager.CheckPasswordAsync(_user, user.PasswordHash!);
        }
    }
}