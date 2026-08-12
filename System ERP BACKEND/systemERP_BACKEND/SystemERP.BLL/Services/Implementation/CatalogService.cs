using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DTO.Catalog;
using SystemERP.DTO.Products;
using SystemERP.Model;

namespace SystemERP.BLL.Services.Implementation
{
    public class CatalogService : ICatalogService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Country> _countryRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<SubCategory> _subCategoryRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<UnitMeasure> _unitMeasureRepository;
        private readonly IGenericRepository<Presentation> _presentationRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Municipality> _municipalityRepository;
        private readonly IGenericRepository<District> _districtRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogService> _logger;
        private readonly IActionLogService _actionLogService;

        public CatalogService(
            IGenericRepository<Role> roleRepository,
            IGenericRepository<Country> countryRepository,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<SubCategory> subCategoryRepository,
            IGenericRepository<ProductType> productTypeRepository,
            IGenericRepository<UnitMeasure> unitMeasureRepository,
            IGenericRepository<Presentation> presentationRepository,
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<Municipality> municipalityRepository,
            IGenericRepository<District> districtRepository,
            IMapper mapper,
            ILogger<CatalogService> logger,
            IActionLogService actionLogService)
        {
            _roleRepository = roleRepository;
            _countryRepository = countryRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _productTypeRepository = productTypeRepository;
            _unitMeasureRepository = unitMeasureRepository;
            _presentationRepository = presentationRepository;
            _departmentRepository = departmentRepository;
            _municipalityRepository = municipalityRepository;
            _districtRepository = districtRepository;
            _mapper = mapper;
            _logger = logger;
            _actionLogService = actionLogService;
        }

        #region Consultas de Catálogos (Getters)

        public async Task<List<CatalogDTO>> GetRolesAsync()
        {
            try
            {
                var list = await _roleRepository.Query().ToListAsync();
                return _mapper.Map<List<CatalogDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de roles");
                throw;
            }
        }

        public async Task<List<CatalogDTO>> GetCountriesAsync()
        {
            try
            {
                var list = await _countryRepository.Query().ToListAsync();
                return _mapper.Map<List<CatalogDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de países");
                throw;
            }
        }

        public async Task<List<CatalogDTO>> GetCategoriesAsync()
        {
            try
            {
                var list = await _categoryRepository.Query(c => c.IsActive == null || c.IsActive == true).ToListAsync();
                return _mapper.Map<List<CatalogDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de categorías");
                throw;
            }
        }

        public async Task<List<SubCategoryDTO>> GetSubCategoriesAsync(int? categoryId = null)
        {
            try
            {
                var query = _subCategoryRepository.Query();
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    query = query.Where(sc => sc.IdCategory == categoryId.Value);
                }

                var list = await query
                    .Include(sc => sc.IdCategoryNavigation)
                    .Where(sc => sc.IsActive == null || sc.IsActive == true)
                    .OrderBy(sc => sc.Name)
                    .ToListAsync();

                return _mapper.Map<List<SubCategoryDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo subcategorías");
                throw;
            }
        }

        public async Task<List<CatalogDTO>> GetProductTypesAsync()
        {
            try
            {
                var list = await _productTypeRepository.Query().ToListAsync();
                return _mapper.Map<List<CatalogDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de tipos de producto");
                throw;
            }
        }

        public async Task<List<UnitMeasureDTO>> GetUnitMeasuresAsync()
        {
            try
            {
                var list = await _unitMeasureRepository.Query(u => u.IsActive == null || u.IsActive == true).ToListAsync();
                return _mapper.Map<List<UnitMeasureDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de unidades de medida");
                throw;
            }
        }

        public async Task<List<CatalogDTO>> GetPresentationsAsync()
        {
            try
            {
                var list = await _presentationRepository.Query().ToListAsync();
                return _mapper.Map<List<CatalogDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de presentaciones");
                throw;
            }
        }

        public async Task<List<CatalogDTO<string>>> GetDepartmentsAsync()
        {
            try
            {
                var list = await _departmentRepository.Query().ToListAsync();
                return _mapper.Map<List<CatalogDTO<string>>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de departamentos");
                throw;
            }
        }

        public async Task<List<CatalogDTO<string>>> GetMunicipalitiesAsync(string departmentId)
        {
            try
            {
                var list = await _municipalityRepository.Query(m => m.DepartmentId == departmentId).ToListAsync();
                return _mapper.Map<List<CatalogDTO<string>>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de municipios para departamento {DepartmentId}", departmentId);
                throw;
            }
        }

        public async Task<List<CatalogDTO<string>>> GetDistrictsAsync(string municipalityId)
        {
            try
            {
                var list = await _districtRepository.Query(d => d.MunicipalityId == municipalityId).ToListAsync();
                return _mapper.Map<List<CatalogDTO<string>>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de distritos para municipio {MunicipalityId}", municipalityId);
                throw;
            }
        }

        #endregion

        #region CRUD Admin (Category & SubCategory)

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Category>(dto);
                entity.IsActive = true;
                var created = await _categoryRepository.Create(entity);

                await _actionLogService.LogActionAsync(null, "CREAR_CATEGORIA", "categories", created.IdCategory.ToString(), $"Categoría creada: {created.Name}");

                return _mapper.Map<CategoryDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando categoría");
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var entity = await _categoryRepository.GetById(id);
                if (entity == null) return false;

                var deleted = await _categoryRepository.HardDelete(entity);
                if (deleted)
                {
                    await _actionLogService.LogActionAsync(null, "ELIMINAR_CATEGORIA", "categories", id.ToString(), $"Categoría eliminada: {entity.Name}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando categoría con ID {Id}", id);
                throw;
            }
        }

        public async Task<SubCategoryDTO> CreateSubCategoryAsync(SubCategoryDTO dto)
        {
            try
            {
                var entity = _mapper.Map<SubCategory>(dto);
                entity.IsActive = true;
                var created = await _subCategoryRepository.Create(entity);
                var loaded = await _subCategoryRepository.Query(sc => sc.IdSubCategory == created.IdSubCategory)
                    .Include(sc => sc.IdCategoryNavigation)
                    .FirstOrDefaultAsync() ?? created;

                await _actionLogService.LogActionAsync(null, "CREAR_SUBCATEGORIA", "sub_categories", created.IdSubCategory.ToString(), $"Subcategoría creada: {created.Name} (Categoría ID: {created.IdCategory})");

                return _mapper.Map<SubCategoryDTO>(loaded);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando subcategoría");
                throw;
            }
        }

        public async Task<bool> DeleteSubCategoryAsync(int id)
        {
            try
            {
                var entity = await _subCategoryRepository.GetById(id);
                if (entity == null) return false;

                var deleted = await _subCategoryRepository.HardDelete(entity);
                if (deleted)
                {
                    await _actionLogService.LogActionAsync(null, "ELIMINAR_SUBCATEGORIA", "sub_categories", id.ToString(), $"Subcategoría eliminada: {entity.Name}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando subcategoría con ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region CRUD Admin (ProductType)

        public async Task<CatalogDTO> CreateProductTypeAsync(CatalogDTO dto)
        {
            try
            {
                var entity = new ProductType { Description = dto.Name };
                var created = await _productTypeRepository.Create(entity);

                await _actionLogService.LogActionAsync(null, "CREAR_TIPO_PRODUCTO", "product_types", created.IdProductType.ToString(), $"Tipo de producto creado: {created.Description}");

                return _mapper.Map<CatalogDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando tipo de producto");
                throw;
            }
        }

        public async Task<bool> DeleteProductTypeAsync(int id)
        {
            try
            {
                var entity = await _productTypeRepository.GetById(id);
                if (entity == null) return false;

                var deleted = await _productTypeRepository.HardDelete(entity);
                if (deleted)
                {
                    await _actionLogService.LogActionAsync(null, "ELIMINAR_TIPO_PRODUCTO", "product_types", id.ToString(), $"Tipo de producto eliminado: {entity.Description}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando tipo de producto con ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region CRUD Admin (UnitMeasure)

        public async Task<UnitMeasureDTO> CreateUnitMeasureAsync(UnitMeasureDTO dto)
        {
            try
            {
                var entity = _mapper.Map<UnitMeasure>(dto);
                entity.IsActive = true;
                var created = await _unitMeasureRepository.Create(entity);

                await _actionLogService.LogActionAsync(null, "CREAR_UNIDAD_MEDIDA", "unit_measures", created.IdUnitMeasure.ToString(), $"Unidad de medida creada: {created.Description} ({created.Type})");

                return _mapper.Map<UnitMeasureDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando unidad de medida");
                throw;
            }
        }

        public async Task<bool> DeleteUnitMeasureAsync(int id)
        {
            try
            {
                var entity = await _unitMeasureRepository.GetById(id);
                if (entity == null) return false;

                var deleted = await _unitMeasureRepository.HardDelete(entity);
                if (deleted)
                {
                    await _actionLogService.LogActionAsync(null, "ELIMINAR_UNIDAD_MEDIDA", "unit_measures", id.ToString(), $"Unidad de medida eliminada: {entity.Description}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando unidad de medida con ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region CRUD Admin (Presentation)

        public async Task<CatalogDTO> CreatePresentationAsync(CatalogDTO dto)
        {
            try
            {
                var entity = new Presentation { Name = dto.Name };
                var created = await _presentationRepository.Create(entity);

                await _actionLogService.LogActionAsync(null, "CREAR_PRESENTACION", "presentations", created.IdPresentation.ToString(), $"Presentación creada: {created.Name}");

                return _mapper.Map<CatalogDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando presentación");
                throw;
            }
        }

        public async Task<bool> DeletePresentationAsync(int id)
        {
            try
            {
                var entity = await _presentationRepository.GetById(id);
                if (entity == null) return false;

                var deleted = await _presentationRepository.HardDelete(entity);
                if (deleted)
                {
                    await _actionLogService.LogActionAsync(null, "ELIMINAR_PRESENTACION", "presentations", id.ToString(), $"Presentación eliminada: {entity.Name}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando presentación con ID {Id}", id);
                throw;
            }
        }

        #endregion
    }
}
