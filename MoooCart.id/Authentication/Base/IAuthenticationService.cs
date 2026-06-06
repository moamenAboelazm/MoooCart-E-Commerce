using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.id.Authentication.Base
{
    public interface IAuthenticationService
    {
        Task<DtoResponse> CreateUser(DtoCreateUser user);
        Task<DtoLoginResponse> LoginUser(DtoLoginUser user);
        Task<DtoLoginResponse> RetrieveToken(string refreshToken);


    }
}
