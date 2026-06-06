using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoooCart.id.Identity;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;

namespace MoooCart.Services
{
    public class UserService(UserManager<AppUser> _userManager, IMapper _mapper) : IUserService
    {
        public async Task<IEnumerable<DtoUserProfile>> GetAllUsersAsync()
        {
            return await _userManager.Users.Select(u => new DtoUserProfile
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                ProfileImgUrl = u.ProfileImgUrl,
                IsActive = u.IsActive
            }).ToListAsync();
        }

        public async Task<DtoUserProfile> GetProfileAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            return new DtoUserProfile
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ProfileImgUrl = user.ProfileImgUrl,
                IsActive = user.IsActive
            };
        }

        public async Task<DtoResponse> UpdateProfileAsync(string email, DtoUpdateProfile model)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new DtoResponse { Success = false, msg = "User not found!" };

            user.FullName = model.FullName ?? user.FullName;
            user.ProfileImgUrl = model.ProfileImgUrl ?? user.ProfileImgUrl;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return new DtoResponse { Success = true, msg = "Profile updated successfully!" };

            return new DtoResponse { Success = false, msg = string.Join(", ", result.Errors.Select(e => e.Description)) };
        }

        public async Task<DtoResponse> ChangePasswordAsync(string email, DtoChangePassword model)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new DtoResponse { Success = false, msg = "User not found!" };

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
                return new DtoResponse { Success = true, msg = "Password changed successfully!" };

            return new DtoResponse { Success = false, msg = string.Join(", ", result.Errors.Select(e => e.Description)) };
        }
    }
}