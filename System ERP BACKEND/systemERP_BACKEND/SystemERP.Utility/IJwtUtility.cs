using System;
using SystemERP.DTO.Security;

namespace SystemERP.Utility
{
    public interface IJwtUtility
    {
        string GenerarJWT(SessionDTO session);
        string GenerarTokenRecuperacion(Guid userId);
        Guid ValidarTokenRecuperacion(string token);
    }
}
