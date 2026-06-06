using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoooCart.DB.Contexts;
using MoooCart.id.Authentication.Base;
using MoooCart.id.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MoooCart.DB.Repositories
{
    public class TokenManagement(AppDbContext _context, IConfiguration _config) : ITokenManagement
    {
        public async Task<int> AddRefreshToken(string userId, string refreshToken)
        {
            var token = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken
            };

            _context.RefreshTokens.Add(token);
            return await _context.SaveChangesAsync();
        }

        public string GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(5);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public List<Claim> GetUserClaimsFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,   
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!)),
                ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal.Claims.ToList();
        }

        public async Task<string> GetUserIdByRefreshToken(string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
            return token?.UserId ?? string.Empty;
        }

        public async Task<int> UpdateRefreshToken(string userId, string refreshToken)
        {
            var userToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);

            if (userToken != null)
            {
                userToken.Token = refreshToken;
                _context.RefreshTokens.Update(userToken);
                return await _context.SaveChangesAsync();
            }

            return await AddRefreshToken(userId, refreshToken);
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            return await _context.RefreshTokens.AnyAsync(t => t.Token == refreshToken);
        }
    }
}