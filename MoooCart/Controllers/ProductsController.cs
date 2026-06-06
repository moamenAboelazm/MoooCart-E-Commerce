using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;

namespace MoooCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DtoProductParams productParams)
        {
            var result = await _service.GetAllAsync(productParams);
            return Ok(result);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _service.GetByIDAsync(id);
            return product != null ? Ok(product) : NotFound(id);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(DtoProduct dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = await _service.AddAsync(dto);
            return product != null ? Ok(product) : BadRequest(dto);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(DtoUpdateProduct dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = await _service.UpdateAsync(dto);
            return product != null ? Ok(product) : BadRequest(dto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result != null ? Ok(result) : BadRequest(result);
        }


    }
}
