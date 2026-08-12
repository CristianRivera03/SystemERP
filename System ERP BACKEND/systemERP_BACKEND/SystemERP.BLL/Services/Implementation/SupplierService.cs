using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DTO.Entities;
using SystemERP.Model;

namespace SystemERP.BLL.Services.Implementation
{
    public class SupplierService : ISupplierService
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;
        private readonly IGenericRepository<SupplierContact> _supplierContactRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupplierService> _logger;
        private readonly IActionLogService _actionLogService;

        public SupplierService(
            IGenericRepository<Supplier> supplierRepository,
            IGenericRepository<SupplierContact> supplierContactRepository,
            IMapper mapper,
            ILogger<SupplierService> logger,
            IActionLogService actionLogService)
        {
            _supplierRepository = supplierRepository;
            _supplierContactRepository = supplierContactRepository;
            _mapper = mapper;
            _logger = logger;
            _actionLogService = actionLogService;
        }

        public async Task<List<SupplierDTO>> GetSuppliersAsync(string? search = null)
        {
            try
            {
                var query = _supplierRepository.Query(s => s.DeletedAt == null);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim().ToLower();
                    query = query.Where(s =>
                        s.Name.ToLower().Contains(search) ||
                        s.TaxId.ToLower().Contains(search) ||
                        (s.Code != null && s.Code.ToLower().Contains(search)) ||
                        (s.Email != null && s.Email.ToLower().Contains(search)) ||
                        (s.Phone != null && s.Phone.ToLower().Contains(search)));
                }

                var list = await query
                    .Include(s => s.District).ThenInclude(d => d.Municipality).ThenInclude(m => m.Department)
                    .Include(s => s.SupplierContacts)
                    .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync();

                return _mapper.Map<List<SupplierDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proveedores");
                throw;
            }
        }

        public async Task<SupplierDTO?> GetSupplierByIdAsync(Guid id)
        {
            try
            {
                var supplier = await _supplierRepository.Query(s => s.IdSupplier == id && s.DeletedAt == null)
                    .Include(s => s.District).ThenInclude(d => d.Municipality).ThenInclude(m => m.Department)
                    .Include(s => s.SupplierContacts)
                    .FirstOrDefaultAsync();

                return supplier == null ? null : _mapper.Map<SupplierDTO>(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proveedor {Id}", id);
                throw;
            }
        }

        public async Task<SupplierDTO> CreateSupplierAsync(SupplierDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Supplier>(dto);
                entity.IdSupplier = Guid.NewGuid();
                entity.IsActive = true;
                entity.CreatedAt = DateTime.UtcNow;

                var created = await _supplierRepository.Create(entity);

                if (dto.Contacts != null && dto.Contacts.Any())
                {
                    foreach (var cDto in dto.Contacts)
                    {
                        var contact = _mapper.Map<SupplierContact>(cDto);
                        contact.IdSupplier = created.IdSupplier;
                        contact.IsActive = true;
                        await _supplierContactRepository.Create(contact);
                    }
                }

                var loaded = await GetSupplierByIdAsync(created.IdSupplier);

                await _actionLogService.LogActionAsync(null, "CREAR_PROVEEDOR", "suppliers", created.IdSupplier.ToString(), $"Proveedor creado: {created.Name} (NIT/TaxId: {created.TaxId})");

                return loaded ?? _mapper.Map<SupplierDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando proveedor");
                throw;
            }
        }

        public async Task<SupplierDTO> UpdateSupplierAsync(SupplierDTO dto)
        {
            try
            {
                var entity = await _supplierRepository.GetById(dto.IdSupplier);
                if (entity == null || entity.DeletedAt != null)
                {
                    throw new Exception("Proveedor no encontrado");
                }

                entity.Name = dto.Name;
                entity.TaxId = dto.TaxId;
                entity.Code = dto.Code;
                entity.Website = dto.Website;
                entity.Email = dto.Email;
                entity.Phone = dto.Phone;
                entity.DistrictId = dto.DistrictId;
                entity.AddressComplement = dto.AddressComplement;
                entity.UpdatedAt = DateTime.UtcNow;

                await _supplierRepository.Update(entity);
                var updated = await GetSupplierByIdAsync(entity.IdSupplier);

                await _actionLogService.LogActionAsync(null, "EDITAR_PROVEEDOR", "suppliers", entity.IdSupplier.ToString(), $"Proveedor actualizado: {entity.Name}");

                return updated ?? _mapper.Map<SupplierDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando proveedor {Id}", dto.IdSupplier);
                throw;
            }
        }

        public async Task<bool> ToggleStatusAsync(Guid id)
        {
            try
            {
                var entity = await _supplierRepository.GetById(id);
                if (entity == null || entity.DeletedAt != null) return false;

                entity.IsActive = !(entity.IsActive ?? true);
                entity.UpdatedAt = DateTime.UtcNow;

                var result = await _supplierRepository.Update(entity);
                if (result)
                {
                    await _actionLogService.LogActionAsync(null, "CAMBIAR_ESTADO_PROVEEDOR", "suppliers", id.ToString(), $"Estado de proveedor {entity.Name} cambiado a {(entity.IsActive == true ? "Activo" : "Inactivo")}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cambiando estado de proveedor {Id}", id);
                throw;
            }
        }

        public async Task<SupplierContactDTO> AddContactAsync(SupplierContactDTO dto)
        {
            try
            {
                var entity = _mapper.Map<SupplierContact>(dto);
                entity.IsActive = true;
                var created = await _supplierContactRepository.Create(entity);

                await _actionLogService.LogActionAsync(null, "CREAR_CONTACTO_PROVEEDOR", "supplier_contacts", created.IdSupplierContact.ToString(), $"Contacto creado: {created.FullName} para proveedor {created.IdSupplier}");

                return _mapper.Map<SupplierContactDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando contacto de proveedor");
                throw;
            }
        }

        public async Task<bool> DeleteContactAsync(int contactId)
        {
            try
            {
                var contact = await _supplierContactRepository.GetById(contactId);
                if (contact == null) return false;

                var deleted = await _supplierContactRepository.HardDelete(contact);
                if (deleted)
                {
                    await _actionLogService.LogActionAsync(null, "ELIMINAR_CONTACTO_PROVEEDOR", "supplier_contacts", contactId.ToString(), $"Contacto eliminado: {contact.FullName}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando contacto {Id}", contactId);
                throw;
            }
        }
    }
}
