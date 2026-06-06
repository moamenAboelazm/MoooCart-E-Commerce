using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;

namespace MoooCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService _service) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetCategorytById(Guid id)
        {
            var category = await _service.GetByIDAsync(id);
            return category != null ? Ok(category) : NotFound(id);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(DtoCategory dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = await _service.AddAsync(dto);
            return category != null ? Ok(category) : BadRequest(dto);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(DtoUpdateCategory dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = await _service.UpdateAsync(dto);
            return category != null ? Ok(category) : BadRequest(dto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result != null ? Ok(result) : BadRequest(result);
        }
    }
}
