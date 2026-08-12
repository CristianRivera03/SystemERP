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
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;
        private readonly IActionLogService _actionLogService;

        public CustomerService(
            IGenericRepository<Customer> customerRepository,
            IMapper mapper,
            ILogger<CustomerService> logger,
            IActionLogService actionLogService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
            _actionLogService = actionLogService;
        }

        public async Task<List<CustomerDTO>> GetCustomersAsync(string? search = null)
        {
            try
            {
                var query = _customerRepository.Query(c => c.DeletedAt == null);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim().ToLower();
                    query = query.Where(c =>
                        c.Name.ToLower().Contains(search) ||
                        c.TaxId.ToLower().Contains(search) ||
                        (c.Email != null && c.Email.ToLower().Contains(search)) ||
                        (c.Phone != null && c.Phone.ToLower().Contains(search)));
                }

                var list = await query
                    .Include(c => c.District).ThenInclude(d => d.Municipality).ThenInclude(m => m.Department)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                return _mapper.Map<List<CustomerDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo clientes");
                throw;
            }
        }

        public async Task<CustomerDTO?> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.Query(c => c.IdCustomer == id && c.DeletedAt == null)
                    .Include(c => c.District).ThenInclude(d => d.Municipality).ThenInclude(m => m.Department)
                    .FirstOrDefaultAsync();

                return customer == null ? null : _mapper.Map<CustomerDTO>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cliente {Id}", id);
                throw;
            }
        }

        public async Task<CustomerDTO> CreateCustomerAsync(CustomerDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Customer>(dto);
                entity.IdCustomer = Guid.NewGuid();
                entity.IsActive = true;
                entity.CreatedAt = DateTime.UtcNow;

                var created = await _customerRepository.Create(entity);
                var loaded = await GetCustomerByIdAsync(created.IdCustomer);

                await _actionLogService.LogActionAsync(null, "CREAR_CLIENTE", "customers", created.IdCustomer.ToString(), $"Cliente creado: {created.Name} (NIT/Doc: {created.TaxId})");

                return loaded ?? _mapper.Map<CustomerDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando cliente");
                throw;
            }
        }

        public async Task<CustomerDTO> UpdateCustomerAsync(CustomerDTO dto)
        {
            try
            {
                var entity = await _customerRepository.GetById(dto.IdCustomer);
                if (entity == null || entity.DeletedAt != null)
                {
                    throw new Exception("Cliente no encontrado");
                }

                entity.Name = dto.Name;
                entity.TaxId = dto.TaxId;
                entity.Email = dto.Email;
                entity.Phone = dto.Phone;
                entity.DistrictId = dto.DistrictId;
                entity.AddressComplement = dto.AddressComplement;
                entity.UpdatedAt = DateTime.UtcNow;

                await _customerRepository.Update(entity);
                var updated = await GetCustomerByIdAsync(entity.IdCustomer);

                await _actionLogService.LogActionAsync(null, "EDITAR_CLIENTE", "customers", entity.IdCustomer.ToString(), $"Cliente actualizado: {entity.Name}");

                return updated ?? _mapper.Map<CustomerDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando cliente {Id}", dto.IdCustomer);
                throw;
            }
        }

        public async Task<bool> ToggleStatusAsync(Guid id)
        {
            try
            {
                var entity = await _customerRepository.GetById(id);
                if (entity == null || entity.DeletedAt != null) return false;

                entity.IsActive = !(entity.IsActive ?? true);
                entity.UpdatedAt = DateTime.UtcNow;

                var result = await _customerRepository.Update(entity);
                if (result)
                {
                    await _actionLogService.LogActionAsync(null, "CAMBIAR_ESTADO_CLIENTE", "customers", id.ToString(), $"Estado de cliente {entity.Name} cambiado a {(entity.IsActive == true ? "Activo" : "Inactivo")}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cambiando estado de cliente {Id}", id);
                throw;
            }
        }
    }
}
