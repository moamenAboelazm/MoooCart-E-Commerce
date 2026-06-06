using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;
using MoooCart.Services;
using System.Security.Claims;

namespace MoooCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUserService _userService) : ControllerBase
    {

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized();

            var profile = await _userService.GetProfileAsync(email);
            if (profile == null) return NotFound("User not found");

            return Ok(profile);
        }

        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] DtoUpdateProfile model)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _userService.UpdateProfileAsync(email, model);

            if (result.Success) return Ok(result.msg);

            return BadRequest(result.msg);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] DtoChangePassword model)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _userService.ChangePasswordAsync(email, model);

            if (result.Success) return Ok(result.msg);

            return BadRequest(result.msg);
        }
    }
}