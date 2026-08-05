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
using SystemERP.Model;

namespace SystemERP.BLL.Services.Implementation
{
    public class CatalogService : ICatalogService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Country> _countryRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<UnitMeasure> _unitMeasureRepository;
        private readonly IGenericRepository<Presentation> _presentationRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Municipality> _municipalityRepository;
        private readonly IGenericRepository<District> _districtRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(
            IGenericRepository<Role> roleRepository,
            IGenericRepository<Country> countryRepository,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<ProductType> productTypeRepository,
            IGenericRepository<UnitMeasure> unitMeasureRepository,
            IGenericRepository<Presentation> presentationRepository,
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<Municipality> municipalityRepository,
            IGenericRepository<District> districtRepository,
            IMapper mapper,
            ILogger<CatalogService> logger)
        {
            _roleRepository = roleRepository;
            _countryRepository = countryRepository;
            _categoryRepository = categoryRepository;
            _productTypeRepository = productTypeRepository;
            _unitMeasureRepository = unitMeasureRepository;
            _presentationRepository = presentationRepository;
            _departmentRepository = departmentRepository;
            _municipalityRepository = municipalityRepository;
            _districtRepository = districtRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Consultas de Catálogos (Getters)

        public async Task<List<CatalogDTO>> GetRolesAsync()
        {
            try
            {
                var query = _roleRepository.Query();
                var list = await query.ToListAsync();
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
                var query = _countryRepository.Query();
                var list = await query.ToListAsync();
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
                var query = _categoryRepository.Query();
                var list = await query.ToListAsync();
                return _mapper.Map<List<CatalogDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de categorías");
                throw;
            }
        }

        public async Task<List<CatalogDTO>> GetProductTypesAsync()
        {
            try
            {
                var query = _productTypeRepository.Query();
                var list = await query.ToListAsync();
                return _mapper.Map<List<CatalogDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de tipos de producto");
                throw;
            }
        }

        public async Task<List<CatalogDTO>> GetUnitMeasuresAsync()
        {
            try
            {
                var query = _unitMeasureRepository.Query();
                var list = await query.ToListAsync();
                return _mapper.Map<List<CatalogDTO>>(list);
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
                var query = _presentationRepository.Query();
                var list = await query.ToListAsync();
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
                var query = _departmentRepository.Query();
                var list = await query.ToListAsync();
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
                var query = _municipalityRepository.Query(m => m.DepartmentId == departmentId);
                var list = await query.ToListAsync();
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
                var query = _districtRepository.Query(d => d.MunicipalityId == municipalityId);
                var list = await query.ToListAsync();
                return _mapper.Map<List<CatalogDTO<string>>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de distritos para municipio {MunicipalityId}", municipalityId);
                throw;
            }
        }

        #endregion

        #region CRUD Admin (Category)

        public async Task<CatalogDTO> CreateCategoryAsync(CatalogDTO dto)
        {
            try
            {
                var entity = new Category { Name = dto.Name };
                var created = await _categoryRepository.Create(entity);
                return _mapper.Map<CatalogDTO>(created);
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
                return await _categoryRepository.HardDelete(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando categoría con ID {Id}", id);
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
                return await _productTypeRepository.HardDelete(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando tipo de producto con ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region CRUD Admin (UnitMeasure)

        public async Task<CatalogDTO> CreateUnitMeasureAsync(CatalogDTO dto)
        {
            try
            {
                var entity = new UnitMeasure { Description = dto.Name };
                var created = await _unitMeasureRepository.Create(entity);
                return _mapper.Map<CatalogDTO>(created);
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
                return await _unitMeasureRepository.HardDelete(entity);
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
                return await _presentationRepository.HardDelete(entity);
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
