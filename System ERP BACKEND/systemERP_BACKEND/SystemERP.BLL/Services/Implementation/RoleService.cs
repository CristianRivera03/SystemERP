using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.DBContext;
using SystemERP.DTO.Security;
using SystemERP.Model;

namespace SystemERP.BLL.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly SystemErpDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService(SystemErpDbContext context, IMapper mapper, ILogger<RoleService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<RoleWithModulesDTO>> GetRolesWithModulesAsync()
        {
            try
            {
                var roles = await _context.Roles
                    .Where(r => r.DeletedAt == null)
                    .Include(r => r.IdModules)
                    .OrderBy(r => r.IdRole)
                    .ToListAsync();

                var result = roles.Select(r => new RoleWithModulesDTO
                {
                    IdRole = r.IdRole,
                    RoleName = r.RoleName,
                    Modules = _mapper.Map<List<ModuleDTO>>(r.IdModules)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles con módulos asignados");
                throw;
            }
        }

        public async Task<List<ModuleDTO>> GetAllModulesAsync()
        {
            try
            {
                var modules = await _context.Modules
                    .Where(m => m.IsActive == true)
                    .OrderBy(m => m.IdModule)
                    .ToListAsync();

                return _mapper.Map<List<ModuleDTO>>(modules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de módulos");
                throw;
            }
        }

        public async Task<RoleWithModulesDTO> CreateRoleAsync(string roleName, List<int>? initialModuleIds = null)
        {
            try
            {
                var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.Trim().ToLower());
                if (existingRole != null)
                {
                    throw new InvalidOperationException($"El rol '{roleName}' ya existe en el sistema.");
                }

                var newRole = new Role
                {
                    RoleName = roleName.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                if (initialModuleIds != null && initialModuleIds.Any())
                {
                    var modulesToAssign = await _context.Modules
                        .Where(m => initialModuleIds.Contains(m.IdModule))
                        .ToListAsync();

                    foreach (var module in modulesToAssign)
                    {
                        newRole.IdModules.Add(module);
                    }
                }

                _context.Roles.Add(newRole);
                await _context.SaveChangesAsync();

                return new RoleWithModulesDTO
                {
                    IdRole = newRole.IdRole,
                    RoleName = newRole.RoleName,
                    Modules = _mapper.Map<List<ModuleDTO>>(newRole.IdModules)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rol {RoleName}", roleName);
                throw;
            }
        }

        public async Task<bool> UpdateRolePermissionsAsync(UpdateRolePermissionsDTO dto)
        {
            try
            {
                var role = await _context.Roles
                    .Include(r => r.IdModules)
                    .FirstOrDefaultAsync(r => r.IdRole == dto.IdRole && r.DeletedAt == null);

                if (role == null)
                {
                    throw new KeyNotFoundException($"El rol con ID {dto.IdRole} no existe.");
                }

                // Clear current assigned modules
                role.IdModules.Clear();

                // Assign selected modules
                if (dto.ModuleIds != null && dto.ModuleIds.Any())
                {
                    var selectedModules = await _context.Modules
                        .Where(m => dto.ModuleIds.Contains(m.IdModule))
                        .ToListAsync();

                    foreach (var module in selectedModules)
                    {
                        role.IdModules.Add(module);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar los permisos del rol {IdRole}", dto.IdRole);
                throw;
            }
        }

        public async Task<bool> DeleteRoleAsync(int idRole)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.IdRole == idRole);
                if (role == null) return false;

                role.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol {IdRole}", idRole);
                throw;
            }
        }
    }
}
