using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using MoooCart.lib.DTOs;
using MoooCart.id.Authentication.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MoooCart.lib.Base;
using AutoMapper;
using FluentValidation;
using MoooCart.id.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog.Core;

namespace MoooCart.id.Authentication.Repositories
{
    public class AuthenticationService(ITokenManagement tokenManagement , IUserManagement userManagement , IRoleManagement roleManagement , 
        IAppLoger<AuthenticationService> applogger , IMapper mapper , IValidator<DtoCreateUser> createUserValidator , IValidator<DtoLoginUser> loginUserValidator , IValidationService Validator , IValidationService validationService) : Base.IAuthenticationService
    {
        public async Task<DtoResponse> CreateUser(DtoCreateUser user)
        {
            var _validationResult = await validationService.ValidationAsync(user, createUserValidator);
            if (!_validationResult.Success) return _validationResult;

            var mappedUser = mapper.Map<AppUser>(user);
            mappedUser.UserName = user.Email;
            mappedUser.PasswordHash = user.Password;

            var result = await userManagement.CreateUser(mappedUser);
            if (!result) return new DtoResponse { msg = "There is invalid data" };

            var _user = await userManagement.GetUserByEmail(user.Email);
            var users = await userManagement.GetAllUsers();
            bool assignedResult = await roleManagement.AddUserToRole(_user! , users.Count() > 1 ? "User" : "Admin" );

            if (!assignedResult)
            {
                int removeUser = await userManagement.DeleteUserByEmail(_user.Email);
                if (removeUser == 0) 
                {
                    applogger.LogError(new Exception($"User Error in use Email {_user.Email}"), "user error");
                    return new DtoResponse { msg = "Can't Create new account"};
                }
            }
            return new DtoResponse { msg = "Success user account has been created", Success = true };
        }

        public async Task<DtoLoginResponse> LoginUser(DtoLoginUser user)
        {
            var _validationResult = await validationService.ValidationAsync(user, loginUserValidator);
            if (!_validationResult.Success) return new DtoLoginResponse(false ,_validationResult.msg );

            var mappedUser = mapper.Map<AppUser>(user);
            mappedUser.PasswordHash = user.Password;

            bool loginResult = await userManagement.LoginUser(mappedUser);

            if (!loginResult) return new DtoLoginResponse { msg = "Invalid user Credentials" };

            var _user = await userManagement.GetUserByEmail(user.Email);
            var claims = await userManagement.GetUserClaims(user.Email);

            string jwtToken = tokenManagement.GenerateToken(claims);
            string refreshToken = tokenManagement.GetRefreshToken();

            int saveTokenResult = await tokenManagement.UpdateRefreshToken(_user.Id, refreshToken);
            if (saveTokenResult == 0) return new DtoLoginResponse { msg = "Internal server Error" };
            return new DtoLoginResponse
            {
                Success = true,
                Token = jwtToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<DtoLoginResponse> RetrieveToken(string refreshToken)
        {
            bool validateTokenResult = await tokenManagement.ValidateRefreshToken(refreshToken);
            if (!validateTokenResult) return new DtoLoginResponse { msg = "Invalid Token" };

            string userId = await tokenManagement.GetUserIdByRefreshToken(refreshToken);
            AppUser? appUser = await userManagement.GetUserById(userId);
            var claims = await userManagement.GetUserClaims(appUser!.Email!);
            string newJwtToken = tokenManagement.GenerateToken(claims);
            string newRefreshToken = tokenManagement.GetRefreshToken();
            await tokenManagement.UpdateRefreshToken(userId, newRefreshToken);

            return new DtoLoginResponse { Success = true , RefreshToken = newRefreshToken , Token = newJwtToken};
            
        }
    }
}
