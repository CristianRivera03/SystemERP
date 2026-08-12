using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Products;

namespace SystemERP.BLL.Services.Contract
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProductsAsync(string? search = null, int? categoryId = null, int? subCategoryId = null);
        Task<ProductDTO?> GetProductByIdAsync(Guid id);
        Task<ProductDTO> CreateProductAsync(ProductDTO dto);
        Task<ProductDTO> UpdateProductAsync(ProductDTO dto);
        Task<bool> ToggleStatusAsync(Guid id);
    }
}
