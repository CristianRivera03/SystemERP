using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.API.Utility;
using SystemERP.BLL.Services.Contract;
using SystemERP.DTO.Catalog;

namespace SystemERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        #region Consultas de Catálogos (Getters)

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var result = await _catalogService.GetRolesAsync();
                return Ok(new Response<List<CatalogDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var result = await _catalogService.GetCountriesAsync();
                return Ok(new Response<List<CatalogDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var result = await _catalogService.GetCategoriesAsync();
                return Ok(new Response<List<CatalogDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("ProductTypes")]
        public async Task<IActionResult> GetProductTypes()
        {
            try
            {
                var result = await _catalogService.GetProductTypesAsync();
                return Ok(new Response<List<CatalogDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("UnitMeasures")]
        public async Task<IActionResult> GetUnitMeasures()
        {
            try
            {
                var result = await _catalogService.GetUnitMeasuresAsync();
                return Ok(new Response<List<CatalogDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("Presentations")]
        public async Task<IActionResult> GetPresentations()
        {
            try
            {
                var result = await _catalogService.GetPresentationsAsync();
                return Ok(new Response<List<CatalogDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("Departments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var result = await _catalogService.GetDepartmentsAsync();
                return Ok(new Response<List<CatalogDTO<string>>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO<string>>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("Municipalities/{departmentId}")]
        public async Task<IActionResult> GetMunicipalities(string departmentId)
        {
            try
            {
                var result = await _catalogService.GetMunicipalitiesAsync(departmentId);
                return Ok(new Response<List<CatalogDTO<string>>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO<string>>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("Districts/{municipalityId}")]
        public async Task<IActionResult> GetDistricts(string municipalityId)
        {
            try
            {
                var result = await _catalogService.GetDistrictsAsync(municipalityId);
                return Ok(new Response<List<CatalogDTO<string>>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CatalogDTO<string>>> { status = false, msg = ex.Message });
            }
        }

        #endregion

        #region Admin CRUD Endpoints

        [HttpPost("Category")]
        public async Task<IActionResult> CreateCategory([FromBody] CatalogDTO dto)
        {
            try
            {
                var result = await _catalogService.CreateCategoryAsync(dto);
                return Ok(new Response<CatalogDTO> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<CatalogDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpDelete("Category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _catalogService.DeleteCategoryAsync(id);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("ProductType")]
        public async Task<IActionResult> CreateProductType([FromBody] CatalogDTO dto)
        {
            try
            {
                var result = await _catalogService.CreateProductTypeAsync(dto);
                return Ok(new Response<CatalogDTO> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<CatalogDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpDelete("ProductType/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            try
            {
                var result = await _catalogService.DeleteProductTypeAsync(id);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("UnitMeasure")]
        public async Task<IActionResult> CreateUnitMeasure([FromBody] CatalogDTO dto)
        {
            try
            {
                var result = await _catalogService.CreateUnitMeasureAsync(dto);
                return Ok(new Response<CatalogDTO> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<CatalogDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpDelete("UnitMeasure/{id}")]
        public async Task<IActionResult> DeleteUnitMeasure(int id)
        {
            try
            {
                var result = await _catalogService.DeleteUnitMeasureAsync(id);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("Presentation")]
        public async Task<IActionResult> CreatePresentation([FromBody] CatalogDTO dto)
        {
            try
            {
                var result = await _catalogService.CreatePresentationAsync(dto);
                return Ok(new Response<CatalogDTO> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<CatalogDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpDelete("Presentation/{id}")]
        public async Task<IActionResult> DeletePresentation(int id)
        {
            try
            {
                var result = await _catalogService.DeletePresentationAsync(id);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }

        #endregion
    }
}
