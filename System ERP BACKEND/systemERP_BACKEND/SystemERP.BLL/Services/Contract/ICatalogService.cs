using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Catalog;
using SystemERP.DTO.Products;

namespace SystemERP.BLL.Services.Contract
{
    public interface ICatalogService
    {
        // Consultas de catálogos generales (Dropdowns / Selects)
        Task<List<CatalogDTO>> GetRolesAsync();
        Task<List<CatalogDTO>> GetCountriesAsync();
        Task<List<CatalogDTO>> GetCategoriesAsync();
        Task<List<SubCategoryDTO>> GetSubCategoriesAsync(int? categoryId = null);
        Task<List<CatalogDTO>> GetProductTypesAsync();
        Task<List<UnitMeasureDTO>> GetUnitMeasuresAsync();
        Task<List<CatalogDTO>> GetPresentationsAsync();

        // Ubicación geográfica
        Task<List<CatalogDTO<string>>> GetDepartmentsAsync();
        Task<List<CatalogDTO<string>>> GetMunicipalitiesAsync(string departmentId);
        Task<List<CatalogDTO<string>>> GetDistrictsAsync(string municipalityId);

        // CRUD Admin de Catálogos
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO dto);
        Task<bool> DeleteCategoryAsync(int id);

        Task<SubCategoryDTO> CreateSubCategoryAsync(SubCategoryDTO dto);
        Task<bool> DeleteSubCategoryAsync(int id);

        Task<CatalogDTO> CreateProductTypeAsync(CatalogDTO dto);
        Task<bool> DeleteProductTypeAsync(int id);

        Task<UnitMeasureDTO> CreateUnitMeasureAsync(UnitMeasureDTO dto);
        Task<bool> DeleteUnitMeasureAsync(int id);

        Task<CatalogDTO> CreatePresentationAsync(CatalogDTO dto);
        Task<bool> DeletePresentationAsync(int id);
    }
}
