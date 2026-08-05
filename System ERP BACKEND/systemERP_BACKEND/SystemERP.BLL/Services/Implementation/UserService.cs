using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DTO.Security;
using SystemERP.Model;
using SystemERP.Utility;

namespace SystemERP.BLL.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IActionLogService _actionLogService;

        public UserService(
            IGenericRepository<User> userRepository,
            IMapper mapper,
            ILogger<UserService> logger,
            IActionLogService actionLogService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _actionLogService = actionLogService;
        }

        public async Task<List<UserDTO>> GetAll()
        {
            try
            {
                var query = _userRepository.Query(u => u.DeletedAt == null);
                var users = await query
                    .Include(u => u.IdRoleNavigation)
                    .Include(u => u.IdCountryNavigation)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();

                return _mapper.Map<List<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios.");
                throw;
            }
        }

        public async Task<UserDTO?> GetById(Guid id)
        {
            try
            {
                var user = await _userRepository.Query(u => u.IdUser == id && u.DeletedAt == null)
                    .Include(u => u.IdRoleNavigation)
                    .Include(u => u.IdCountryNavigation)
                    .FirstOrDefaultAsync();

                return user != null ? _mapper.Map<UserDTO>(user) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario por ID {Id}", id);
                throw;
            }
        }

        public async Task<UserDTO> Register(RegisterDTO registerDto)
        {
            try
            {
                var existingUser = await _userRepository.Query(u => u.Email == registerDto.Email && u.DeletedAt == null).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    throw new InvalidOperationException("El correo electrónico ya se encuentra registrado.");
                }

                var userModel = new User
                {
                    IdUser = Guid.NewGuid(),
                    IdRole = registerDto.IdRole,
                    IdCountry = registerDto.IdCountry,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    Phone = registerDto.Phone,
                    DocumentId = registerDto.DocumentId,
                    PasswordHash = SecurityHelper.HashPassword(registerDto.Password),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.Create(userModel);

                var queryCreated = _userRepository.Query(u => u.IdUser == createdUser.IdUser)
                    .Include(u => u.IdRoleNavigation)
                    .Include(u => u.IdCountryNavigation);

                var userWithNav = await queryCreated.FirstOrDefaultAsync() ?? createdUser;

                await _actionLogService.LogActionAsync(createdUser.IdUser, "REGISTRAR_USUARIO", "users", createdUser.IdUser.ToString(), $"Usuario registrado: {registerDto.FirstName} {registerDto.LastName} ({registerDto.Email})");

                return _mapper.Map<UserDTO>(userWithNav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario.");
                throw;
            }
        }

        public async Task<bool> UpdateName(Guid id, UpdateUserNameDTO dto)
        {
            try
            {
                var user = await _userRepository.Query(u => u.IdUser == id && u.DeletedAt == null).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new KeyNotFoundException("El usuario no fue encontrado.");
                }

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userRepository.Update(user);
                if (result)
                {
                    await _actionLogService.LogActionAsync(null, "MODIFICAR_NOMBRE", "users", id.ToString(), $"Se modificó el nombre del usuario ({user.Email}) a: {dto.FirstName} {dto.LastName}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el nombre del usuario {Id}", id);
                throw;
            }
        }

        public async Task<bool> UpdateInfo(Guid id, UpdateUserInfoDTO dto)
        {
            try
            {
                var user = await _userRepository.Query(u => u.IdUser == id && u.DeletedAt == null).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new KeyNotFoundException("El usuario no fue encontrado.");
                }

                if (user.Email != dto.Email)
                {
                    var existingEmail = await _userRepository.Query(u => u.Email == dto.Email && u.IdUser != id && u.DeletedAt == null).FirstOrDefaultAsync();
                    if (existingEmail != null)
                    {
                        throw new InvalidOperationException("El correo electrónico ya está registrado por otro usuario.");
                    }
                }

                user.Email = dto.Email;
                user.Phone = dto.Phone;
                user.DocumentId = dto.DocumentId;
                user.IdCountry = dto.IdCountry;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userRepository.Update(user);
                if (result)
                {
                    await _actionLogService.LogActionAsync(null, "MODIFICAR_INFORMACION", "users", id.ToString(), $"Se modificó la información del usuario ({dto.Email}): Tel={dto.Phone}, Doc={dto.DocumentId}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la información del usuario {Id}", id);
                throw;
            }
        }

        public async Task<bool> UpdateRole(Guid id, UpdateUserRoleDTO dto)
        {
            try
            {
                var user = await _userRepository.Query(u => u.IdUser == id && u.DeletedAt == null).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new KeyNotFoundException("El usuario no fue encontrado.");
                }

                user.IdRole = dto.IdRole;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userRepository.Update(user);
                if (result)
                {
                    await _actionLogService.LogActionAsync(null, "MODIFICAR_ROL", "users", id.ToString(), $"Se modificó el rol del usuario ({user.Email}) al rol ID: {dto.IdRole}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol del usuario {Id}", id);
                throw;
            }
        }

        public async Task<bool> ToggleStatus(Guid id)
        {
            try
            {
                var user = await _userRepository.Query(u => u.IdUser == id && u.DeletedAt == null).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new KeyNotFoundException("El usuario no fue encontrado.");
                }

                user.IsActive = !(user.IsActive ?? true);
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userRepository.Update(user);
                if (result)
                {
                    await _actionLogService.LogActionAsync(null, "CAMBIAR_ESTADO", "users", id.ToString(), $"Se cambió el estado del usuario ({user.Email}) a: {(user.IsActive == true ? "Activo" : "Inactivo")}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado del usuario {Id}", id);
                throw;
            }
        }
    }
}
