using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.id.Authentication.Base
{
    public interface ITokenManagement
    {
        string GetRefreshToken();
        List<Claim> GetUserClaimsFromToken(string email);
        Task<bool> ValidateRefreshToken(string refreshToken);
        Task<string> GetUserIdByRefreshToken(string refreshToken);
        Task<int> AddRefreshToken(string userId , string refreshToken);
        Task<int> UpdateRefreshToken(string userId , string refreshToken);
        string GenerateToken(List<Claim> claims);
    }
}
