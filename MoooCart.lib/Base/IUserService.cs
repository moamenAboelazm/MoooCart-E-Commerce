using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.Base
{
    public interface IUserService
    {
        Task<IEnumerable<DtoUserProfile>> GetAllUsersAsync();
        Task<DtoUserProfile> GetProfileAsync(string email);
        Task<DtoResponse> UpdateProfileAsync(string email, DtoUpdateProfile model);
        Task<DtoResponse> ChangePasswordAsync(string email, DtoChangePassword model);
    }
}
