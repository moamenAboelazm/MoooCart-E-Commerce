using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoooCart.lib.DTOs;
using System.Web;
using IAuthenticationService = MoooCart.id.Authentication.Base.IAuthenticationService;


namespace MoooCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService service) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(DtoCreateUser user)
        {
            var result = await service.CreateUser(user);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(DtoLoginUser user)
        {
            var result = await service.LoginUser(user);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken( [FromQuery] string refreshToken)
        {
            var result = await service.RetrieveToken(HttpUtility.UrlDecode(refreshToken));
            return result.Success ? Ok(result) : BadRequest(result);

        }

    }
}
