using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Catalog;

namespace SystemERP.BLL.Services.Contract
{
    public interface ICatalogService
    {
        // Consultas de catálogos generales (Dropdowns / Selects)
        Task<List<CatalogDTO>> GetRolesAsync();
        Task<List<CatalogDTO>> GetCountriesAsync();
        Task<List<CatalogDTO>> GetCategoriesAsync();
        Task<List<CatalogDTO>> GetProductTypesAsync();
        Task<List<CatalogDTO>> GetUnitMeasuresAsync();
        Task<List<CatalogDTO>> GetPresentationsAsync();

        // Ubicación geográfica
        Task<List<CatalogDTO<string>>> GetDepartmentsAsync();
        Task<List<CatalogDTO<string>>> GetMunicipalitiesAsync(string departmentId);
        Task<List<CatalogDTO<string>>> GetDistrictsAsync(string municipalityId);

        // CRUD Admin de Catálogos
        Task<CatalogDTO> CreateCategoryAsync(CatalogDTO dto);
        Task<bool> DeleteCategoryAsync(int id);

        Task<CatalogDTO> CreateProductTypeAsync(CatalogDTO dto);
        Task<bool> DeleteProductTypeAsync(int id);

        Task<CatalogDTO> CreateUnitMeasureAsync(CatalogDTO dto);
        Task<bool> DeleteUnitMeasureAsync(int id);

        Task<CatalogDTO> CreatePresentationAsync(CatalogDTO dto);
        Task<bool> DeletePresentationAsync(int id);
    }
}
