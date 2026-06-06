using MoooCart.id.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.id.Authentication.Base
{
    public interface IUserManagement
    {
        Task<bool> CreateUser(AppUser user);
        Task<bool> LoginUser(AppUser user);
        Task<AppUser?> GetUserByEmail(string email);
        Task<AppUser?> GetUserById(string id);
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<int> DeleteUserByEmail(string id);
        Task<List<Claim>> GetUserClaims(string email);
    }
}
