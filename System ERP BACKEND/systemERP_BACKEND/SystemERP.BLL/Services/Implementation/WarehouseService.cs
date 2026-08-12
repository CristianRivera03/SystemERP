using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DTO.Inventory;
using SystemERP.Model;

namespace SystemERP.BLL.Services.Implementation
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IGenericRepository<WarehouseCategory> _catRepository;
        private readonly IGenericRepository<Warehouse> _warehouseRepository;
        private readonly IGenericRepository<Location> _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WarehouseService> _logger;
        private readonly IActionLogService _actionLogService;

        public WarehouseService(
            IGenericRepository<WarehouseCategory> catRepository,
            IGenericRepository<Warehouse> warehouseRepository,
            IGenericRepository<Location> locationRepository,
            IMapper mapper,
            ILogger<WarehouseService> logger,
            IActionLogService actionLogService)
        {
            _catRepository = catRepository;
            _warehouseRepository = warehouseRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
            _logger = logger;
            _actionLogService = actionLogService;
        }

        public async Task<List<WarehouseCategoryDTO>> GetWarehouseCategoriesAsync()
        {
            try
            {
                var cats = await _catRepository.Query().OrderBy(c => c.IdWarehouseCategory).ToListAsync();
                return _mapper.Map<List<WarehouseCategoryDTO>>(cats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar categorías de almacén.");
                throw;
            }
        }

        public async Task<List<WarehouseDTO>> GetAllWarehousesAsync()
        {
            try
            {
                var query = _warehouseRepository.Query();
                var list = await query
                    .Include(w => w.IdBranchNavigation)
                    .Include(w => w.IdWarehouseCategoryNavigation)
                    .OrderBy(w => w.Name)
                    .ToListAsync();

                return _mapper.Map<List<WarehouseDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar los almacenes.");
                throw;
            }
        }

        public async Task<WarehouseDTO?> GetWarehouseByIdAsync(Guid id)
        {
            try
            {
                var item = await _warehouseRepository.Query(w => w.IdWarehouse == id)
                    .Include(w => w.IdBranchNavigation)
                    .Include(w => w.IdWarehouseCategoryNavigation)
                    .FirstOrDefaultAsync();

                return item != null ? _mapper.Map<WarehouseDTO>(item) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el almacén {Id}", id);
                throw;
            }
        }

        public async Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO dto)
        {
            try
            {
                var model = _mapper.Map<Warehouse>(dto);
                model.IdWarehouse = Guid.NewGuid();
                model.IsActive = true;

                var created = await _warehouseRepository.Create(model);

                var queryWithNav = await _warehouseRepository.Query(w => w.IdWarehouse == created.IdWarehouse)
                    .Include(w => w.IdBranchNavigation)
                    .Include(w => w.IdWarehouseCategoryNavigation)
                    .FirstOrDefaultAsync() ?? created;

                await _actionLogService.LogActionAsync(null, "CREAR_ALMACEN", "warehouses", created.IdWarehouse.ToString(), $"Almacén/Bodega creada: {created.Name} (Sucursal ID: {created.IdBranch})");

                return _mapper.Map<WarehouseDTO>(queryWithNav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear almacén.");
                throw;
            }
        }

        public async Task<bool> UpdateWarehouseAsync(Guid id, WarehouseDTO dto)
        {
            try
            {
                var item = await _warehouseRepository.Query(w => w.IdWarehouse == id).FirstOrDefaultAsync();
                if (item == null) throw new KeyNotFoundException("El almacén no fue encontrado.");

                item.Name = dto.Name;
                item.Description = dto.Description;
                item.IdBranch = dto.IdBranch;
                item.IdWarehouseCategory = dto.IdWarehouseCategory;

                var updated = await _warehouseRepository.Update(item);
                if (updated)
                {
                    await _actionLogService.LogActionAsync(null, "MODIFICAR_ALMACEN", "warehouses", id.ToString(), $"Almacén modificado: {dto.Name}");
                }
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar almacén {Id}", id);
                throw;
            }
        }

        public async Task<bool> ToggleWarehouseStatusAsync(Guid id)
        {
            try
            {
                var item = await _warehouseRepository.Query(w => w.IdWarehouse == id).FirstOrDefaultAsync();
                if (item == null) throw new KeyNotFoundException("El almacén no fue encontrado.");

                item.IsActive = !(item.IsActive ?? true);

                var updated = await _warehouseRepository.Update(item);
                if (updated)
                {
                    await _actionLogService.LogActionAsync(null, "CAMBIAR_ESTADO_ALMACEN", "warehouses", id.ToString(), $"Estado de almacén '{item.Name}' cambiado a: {(item.IsActive == true ? "Activo" : "Inactivo")}");
                }
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado del almacén {Id}", id);
                throw;
            }
        }

        public async Task<List<LocationDTO>> GetLocationsByWarehouseAsync(Guid warehouseId)
        {
            try
            {
                var list = await _locationRepository.Query(l => l.IdWarehouse == warehouseId)
                    .Include(l => l.IdWarehouseNavigation)
                    .OrderBy(l => l.Code)
                    .ToListAsync();

                return _mapper.Map<List<LocationDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar ubicaciones del almacén {WarehouseId}", warehouseId);
                throw;
            }
        }

        public async Task<LocationDTO> CreateLocationAsync(LocationDTO dto)
        {
            try
            {
                var model = _mapper.Map<Location>(dto);
                model.IdLocation = Guid.NewGuid();
                model.IsActive = true;
                if (string.IsNullOrEmpty(model.Code))
                {
                    var parts = new[] { dto.Aisle, dto.Rack, dto.Level, dto.Position }.Where(p => !string.IsNullOrEmpty(p));
                    model.Code = string.Join("-", parts);
                }

                var created = await _locationRepository.Create(model);

                var queryWithNav = await _locationRepository.Query(l => l.IdLocation == created.IdLocation)
                    .Include(l => l.IdWarehouseNavigation)
                    .FirstOrDefaultAsync() ?? created;

                await _actionLogService.LogActionAsync(null, "CREAR_UBICACION", "locations", created.IdLocation.ToString(), $"Ubicación creada: '{created.Code}' en almacén ID: {created.IdWarehouse}");

                return _mapper.Map<LocationDTO>(queryWithNav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear ubicación.");
                throw;
            }
        }
    }
}
