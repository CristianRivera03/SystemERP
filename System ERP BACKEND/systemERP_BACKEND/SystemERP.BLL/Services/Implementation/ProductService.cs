using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DTO.Products;
using SystemERP.Model;

namespace SystemERP.BLL.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IActionLogService _actionLogService;

        public ProductService(
            IGenericRepository<Product> productRepository,
            IMapper mapper,
            ILogger<ProductService> logger,
            IActionLogService actionLogService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
            _actionLogService = actionLogService;
        }

        public async Task<List<ProductDTO>> GetProductsAsync(string? search = null, int? categoryId = null, int? subCategoryId = null)
        {
            try
            {
                var query = _productRepository.Query(p => p.DeletedAt == null);

                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    query = query.Where(p => p.IdCategory == categoryId.Value);
                }

                if (subCategoryId.HasValue && subCategoryId.Value > 0)
                {
                    query = query.Where(p => p.IdSubCategory == subCategoryId.Value);
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim().ToLower();
                    query = query.Where(p =>
                        p.Name.ToLower().Contains(search) ||
                        (p.Sku != null && p.Sku.ToLower().Contains(search)) ||
                        (p.InternalCode != null && p.InternalCode.ToLower().Contains(search)) ||
                        (p.OriginalCode != null && p.OriginalCode.ToLower().Contains(search)) ||
                        (p.Barcode != null && p.Barcode.ToLower().Contains(search)));
                }

                var list = await query
                    .Include(p => p.IdCategoryNavigation)
                    .Include(p => p.IdSubCategoryNavigation)
                    .Include(p => p.IdProductTypeNavigation)
                    .Include(p => p.IdUnitMeasureNavigation)
                    .Include(p => p.PurchaseUnit)
                    .Include(p => p.SaleUnit)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                return _mapper.Map<List<ProductDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo productos");
                throw;
            }
        }

        public async Task<ProductDTO?> GetProductByIdAsync(Guid id)
        {
            try
            {
                var product = await _productRepository.Query(p => p.IdProduct == id && p.DeletedAt == null)
                    .Include(p => p.IdCategoryNavigation)
                    .Include(p => p.IdSubCategoryNavigation)
                    .Include(p => p.IdProductTypeNavigation)
                    .Include(p => p.IdUnitMeasureNavigation)
                    .Include(p => p.PurchaseUnit)
                    .Include(p => p.SaleUnit)
                    .FirstOrDefaultAsync();

                return product == null ? null : _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo producto con ID {Id}", id);
                throw;
            }
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Product>(dto);
                entity.IdProduct = Guid.NewGuid();
                entity.IsActive = true;
                entity.CreatedAt = DateTime.UtcNow;

                var created = await _productRepository.Create(entity);
                var loaded = await GetProductByIdAsync(created.IdProduct);

                await _actionLogService.LogActionAsync(null, "CREAR_PRODUCTO", "products", created.IdProduct.ToString(), $"Producto creado: {created.Name} (SKU: {created.Sku ?? "N/A"})");

                return loaded ?? _mapper.Map<ProductDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando producto");
                throw;
            }
        }

        public async Task<ProductDTO> UpdateProductAsync(ProductDTO dto)
        {
            try
            {
                var entity = await _productRepository.GetById(dto.IdProduct);
                if (entity == null || entity.DeletedAt != null)
                {
                    throw new Exception("Producto no encontrado");
                }

                entity.Name = dto.Name;
                entity.IdCategory = dto.IdCategory;
                entity.IdSubCategory = dto.IdSubCategory;
                entity.IdProductType = dto.IdProductType;
                entity.IdUnitMeasure = dto.IdUnitMeasure;
                entity.PurchaseUnitId = dto.PurchaseUnitId;
                entity.SaleUnitId = dto.SaleUnitId;
                entity.Sku = dto.Sku;
                entity.OriginalCode = dto.OriginalCode;
                entity.InternalCode = dto.InternalCode;
                entity.Barcode = dto.Barcode;
                entity.Size = dto.Size;
                entity.Dimensions = dto.Dimensions;
                entity.Presentation = dto.Presentation;
                entity.Description = dto.Description;
                entity.ImageUrl = dto.ImageUrl;
                entity.IsTaxable = dto.IsTaxable;
                entity.MinStock = dto.MinStock;
                entity.UpdatedAt = DateTime.UtcNow;

                await _productRepository.Update(entity);
                var updated = await GetProductByIdAsync(entity.IdProduct);

                await _actionLogService.LogActionAsync(null, "EDITAR_PRODUCTO", "products", entity.IdProduct.ToString(), $"Producto actualizado: {entity.Name}");

                return updated ?? _mapper.Map<ProductDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando producto {Id}", dto.IdProduct);
                throw;
            }
        }

        public async Task<bool> ToggleStatusAsync(Guid id)
        {
            try
            {
                var entity = await _productRepository.GetById(id);
                if (entity == null || entity.DeletedAt != null) return false;

                entity.IsActive = !(entity.IsActive ?? true);
                entity.UpdatedAt = DateTime.UtcNow;

                var result = await _productRepository.Update(entity);
                if (result)
                {
                    await _actionLogService.LogActionAsync(null, "CAMBIAR_ESTADO_PRODUCTO", "products", id.ToString(), $"Estado de producto {entity.Name} cambiado a {(entity.IsActive == true ? "Activo" : "Inactivo")}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cambiando estado de producto {Id}", id);
                throw;
            }
        }
    }
}
