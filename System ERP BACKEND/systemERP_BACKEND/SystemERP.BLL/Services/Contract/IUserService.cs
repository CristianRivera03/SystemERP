using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Security;

namespace SystemERP.BLL.Services.Contract
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAll();
        Task<UserDTO?> GetById(Guid id);
        Task<UserDTO> Register(RegisterDTO registerDto);
        Task<bool> UpdateName(Guid id, UpdateUserNameDTO dto);
        Task<bool> UpdateInfo(Guid id, UpdateUserInfoDTO dto);
        Task<bool> UpdateRole(Guid id, UpdateUserRoleDTO dto);
        Task<bool> ToggleStatus(Guid id);
    }
}
