using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.API.Utility;
using SystemERP.BLL.Services.Contract;
using SystemERP.DTO.Products;

namespace SystemERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] string? search = null, [FromQuery] int? categoryId = null, [FromQuery] int? subCategoryId = null)
        {
            try
            {
                var list = await _productService.GetProductsAsync(search, categoryId, subCategoryId);
                return Ok(new Response<List<ProductDTO>> { status = true, value = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<ProductDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var item = await _productService.GetProductByIdAsync(id);
                if (item == null)
                {
                    return NotFound(new Response<ProductDTO> { status = false, msg = "Producto no encontrado" });
                }
                return Ok(new Response<ProductDTO> { status = true, value = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<ProductDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ProductDTO dto)
        {
            try
            {
                var created = await _productService.CreateProductAsync(dto);
                return Ok(new Response<ProductDTO> { status = true, value = created });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<ProductDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDTO dto)
        {
            try
            {
                dto.IdProduct = id;
                var updated = await _productService.UpdateProductAsync(dto);
                return Ok(new Response<ProductDTO> { status = true, value = updated });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<ProductDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPatch("ToggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var result = await _productService.ToggleStatusAsync(id);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }
    }
}
