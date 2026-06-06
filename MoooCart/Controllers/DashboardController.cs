using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoooCart.lib.Base;
using MoooCart.Services;

namespace MoooCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController(IDashboardService _dashboardService) : ControllerBase
    {

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _dashboardService.GetStatisticsAsync();
            return Ok(statistics);
        }
    }
}