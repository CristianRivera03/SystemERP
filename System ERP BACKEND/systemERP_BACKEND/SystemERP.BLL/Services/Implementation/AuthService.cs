using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DTO.Security;
using SystemERP.Model;
using SystemERP.Utility;

namespace SystemERP.BLL.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtUtility _jwtUtility;
        private readonly IActionLogService _actionLogService;

        public AuthService(
            IGenericRepository<User> userRepository,
            IMapper mapper,
            ILogger<AuthService> logger,
            IJwtUtility jwtUtility,
            IActionLogService actionLogService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _jwtUtility = jwtUtility;
            _actionLogService = actionLogService;
        }

        public async Task<SessionDTO> Login(LoginDTO loginDto)
        {
            try
            {
                var queryUser = _userRepository.Query(u => u.Email == loginDto.Email && (u.IsActive == null || u.IsActive == true) && u.DeletedAt == null);
                var userFound = await queryUser
                    .Include(u => u.IdRoleNavigation)
                        .ThenInclude(r => r.IdModules)
                    .Include(u => u.IdCountryNavigation)
                    .FirstOrDefaultAsync();

                if (userFound == null || !SecurityHelper.VerifyPassword(loginDto.Password, userFound.PasswordHash))
                {
                    throw new UnauthorizedAccessException("El usuario no existe o la contraseña es incorrecta.");
                }

                var session = _mapper.Map<SessionDTO>(userFound);
                session.Token = _jwtUtility.GenerarJWT(session);

                await _actionLogService.LogActionAsync(userFound.IdUser, "INICIO_SESION", "users", userFound.IdUser.ToString(), $"Inicio de sesión exitoso de: {userFound.FirstName} {userFound.LastName} ({userFound.Email})");

                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el inicio de sesión para {Email}", loginDto.Email);
                throw;
            }
        }

        public async Task<SessionDTO> Register(RegisterDTO registerDto)
        {
            try
            {
                var existingUser = await _userRepository.Exists(u => u.Email == registerDto.Email && u.DeletedAt == null);
                if (existingUser)
                {
                    throw new InvalidOperationException("Ya existe un usuario registrado con este correo electrónico.");
                }

                var newUser = new User
                {
                    IdUser = Guid.NewGuid(),
                    IdRole = registerDto.IdRole,
                    IdCountry = registerDto.IdCountry,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    PasswordHash = SecurityHelper.HashPassword(registerDto.Password),
                    Phone = registerDto.Phone,
                    DocumentId = registerDto.DocumentId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.Create(newUser);

                // Cargar relaciones para el DTO de sesión
                var userWithNav = await _userRepository.Query(u => u.IdUser == createdUser.IdUser)
                    .Include(u => u.IdRoleNavigation)
                        .ThenInclude(r => r.IdModules)
                    .Include(u => u.IdCountryNavigation)
                    .FirstOrDefaultAsync() ?? createdUser;

                var session = _mapper.Map<SessionDTO>(userWithNav);
                session.Token = _jwtUtility.GenerarJWT(session);

                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro de usuario para {Email}", registerDto.Email);
                throw;
            }
        }
    }
}
