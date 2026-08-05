using System.Threading.Tasks;
using SystemERP.DTO.Security;

namespace SystemERP.BLL.Services.Contract
{
    public interface IAuthService
    {
        Task<SessionDTO> Login(LoginDTO loginDto);
        Task<SessionDTO> Register(RegisterDTO registerDto);
    }
}
