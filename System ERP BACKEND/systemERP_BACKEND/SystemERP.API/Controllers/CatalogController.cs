using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.API.Utility;
using SystemERP.BLL.Services.Contract;
using SystemERP.DTO.Catalog;
using SystemERP.DTO.Products;

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

        [HttpGet("SubCategories")]
        public async Task<IActionResult> GetSubCategories([FromQuery] int? categoryId = null)
        {
            try
            {
                var result = await _catalogService.GetSubCategoriesAsync(categoryId);
                return Ok(new Response<List<SubCategoryDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<SubCategoryDTO>> { status = false, msg = ex.Message });
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
                return Ok(new Response<List<UnitMeasureDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<UnitMeasureDTO>> { status = false, msg = ex.Message });
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
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO dto)
        {
            try
            {
                var result = await _catalogService.CreateCategoryAsync(dto);
                return Ok(new Response<CategoryDTO> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<CategoryDTO> { status = false, msg = ex.Message });
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

        [HttpPost("SubCategory")]
        public async Task<IActionResult> CreateSubCategory([FromBody] SubCategoryDTO dto)
        {
            try
            {
                var result = await _catalogService.CreateSubCategoryAsync(dto);
                return Ok(new Response<SubCategoryDTO> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<SubCategoryDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpDelete("SubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            try
            {
                var result = await _catalogService.DeleteSubCategoryAsync(id);
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
        public async Task<IActionResult> CreateUnitMeasure([FromBody] UnitMeasureDTO dto)
        {
            try
            {
                var result = await _catalogService.CreateUnitMeasureAsync(dto);
                return Ok(new Response<UnitMeasureDTO> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<UnitMeasureDTO> { status = false, msg = ex.Message });
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
