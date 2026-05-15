using ExamSystem.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.IService
{
    public interface IUserService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<ShowCurrentUserDto> GetCurrentUserAsync(Guid userId);
        Task UpdateCurrentUserAsync(Guid userId, UpdateProfileDto dto);
        Task DeleteCurrentUserAsync(Guid userId);
        Task<bool> IsAdminExistsAsync();
        Task InitializeAdminAsync(InitAdminDto dto);

    }
}
