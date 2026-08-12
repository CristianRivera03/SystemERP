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
    public class BranchService : IBranchService
    {
        private readonly IGenericRepository<Branch> _branchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchService> _logger;
        private readonly IActionLogService _actionLogService;

        public BranchService(
            IGenericRepository<Branch> branchRepository,
            IMapper mapper,
            ILogger<BranchService> logger,
            IActionLogService actionLogService)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
            _logger = logger;
            _actionLogService = actionLogService;
        }

        public async Task<List<BranchDTO>> GetAllBranchesAsync()
        {
            try
            {
                var query = _branchRepository.Query();
                var branches = await query
                    .Include(b => b.IdCompanyNavigation)
                    .Include(b => b.District)
                        .ThenInclude(d => d.Municipality)
                            .ThenInclude(m => m.Department)
                    .OrderBy(b => b.Name)
                    .ToListAsync();

                return _mapper.Map<List<BranchDTO>>(branches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las sucursales.");
                throw;
            }
        }

        public async Task<BranchDTO?> GetBranchByIdAsync(Guid id)
        {
            try
            {
                var branch = await _branchRepository.Query(b => b.IdBranch == id)
                    .Include(b => b.IdCompanyNavigation)
                    .Include(b => b.District)
                        .ThenInclude(d => d.Municipality)
                            .ThenInclude(m => m.Department)
                    .FirstOrDefaultAsync();

                return branch != null ? _mapper.Map<BranchDTO>(branch) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la sucursal {Id}", id);
                throw;
            }
        }

        public async Task<BranchDTO> CreateBranchAsync(BranchDTO dto)
        {
            try
            {
                var model = _mapper.Map<Branch>(dto);
                model.IdBranch = Guid.NewGuid();
                model.IsActive = true;
                model.CreatedAt = DateTime.UtcNow;

                var created = await _branchRepository.Create(model);

                var queryWithNav = await _branchRepository.Query(b => b.IdBranch == created.IdBranch)
                    .Include(b => b.IdCompanyNavigation)
                    .Include(b => b.District)
                    .FirstOrDefaultAsync() ?? created;

                await _actionLogService.LogActionAsync(null, "CREAR_SUCURSAL", "branches", created.IdBranch.ToString(), $"Sucursal registrada: {created.Name} (Empresa ID: {created.IdCompany})");

                return _mapper.Map<BranchDTO>(queryWithNav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la sucursal.");
                throw;
            }
        }

        public async Task<bool> UpdateBranchAsync(Guid id, BranchDTO dto)
        {
            try
            {
                var branch = await _branchRepository.Query(b => b.IdBranch == id).FirstOrDefaultAsync();
                if (branch == null) throw new KeyNotFoundException("La sucursal no existe.");

                branch.Name = dto.Name;
                branch.DistrictId = dto.DistrictId;
                branch.AddressComplement = dto.AddressComplement;
                branch.Phone = dto.Phone;
                branch.Email = dto.Email;

                var updated = await _branchRepository.Update(branch);
                if (updated)
                {
                    await _actionLogService.LogActionAsync(null, "MODIFICAR_SUCURSAL", "branches", id.ToString(), $"Sucursal modificada: {dto.Name}");
                }
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la sucursal {Id}", id);
                throw;
            }
        }

        public async Task<bool> ToggleBranchStatusAsync(Guid id)
        {
            try
            {
                var branch = await _branchRepository.Query(b => b.IdBranch == id).FirstOrDefaultAsync();
                if (branch == null) throw new KeyNotFoundException("La sucursal no existe.");

                branch.IsActive = !(branch.IsActive ?? true);

                var updated = await _branchRepository.Update(branch);
                if (updated)
                {
                    await _actionLogService.LogActionAsync(null, "CAMBIAR_ESTADO_SUCURSAL", "branches", id.ToString(), $"Estado de sucursal '{branch.Name}' cambiado a: {(branch.IsActive == true ? "Activo" : "Inactivo")}");
                }
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado de la sucursal {Id}", id);
                throw;
            }
        }
    }
}
