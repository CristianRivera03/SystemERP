using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using SystemERP.DTO.Security;

namespace SystemERP.Utility
{
    public class JwtUtility : IJwtUtility
    {
        private readonly JwtSettings _jwtSettings;

        public JwtUtility(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Genera un token JWT para la sesión de usuario
        /// </summary>
        public string GenerarJWT(SessionDTO session)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, session.IdUser.ToString()),
                new Claim(ClaimTypes.Email, session.Email),
                new Claim(ClaimTypes.Role, session.RoleName ?? "User"),
                new Claim(ClaimTypes.Name, $"{session.FirstName} {session.LastName}".Trim())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Genera un token de recuperación para cambio de contraseña
        /// </summary>
        public string GenerarTokenRecuperacion(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var claims = new List<Claim>
            {
                new Claim("userId", userId.ToString()),
                new Claim("Purpose", "PasswordReset")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Valida el token de recuperación y extrae el ID de usuario
        /// </summary>
        public Guid ValidarTokenRecuperacion(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var purposeClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "Purpose")?.Value;
                if (purposeClaim != "PasswordReset")
                {
                    throw new UnauthorizedAccessException("Token no es válido para recuperación de contraseña.");
                }

                var userIdString = jwtToken.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
                if (userIdString == null || !Guid.TryParse(userIdString, out Guid userId))
                {
                    throw new UnauthorizedAccessException("El token no contiene un ID de usuario válido.");
                }

                return userId;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException($"Token inválido o expirado. Detalles: {ex.Message}");
            }
        }
    }
}
