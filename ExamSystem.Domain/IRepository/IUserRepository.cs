using ExamSystem.Domain.Models;

namespace ExamSystem.Domain.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> IsUsernameExist(string username);
        Task<bool> IsEmailExist(string email);
        Task<bool> IsEmailExistForAnotherUser(string email, Guid excludeUserId);
        Task<bool> IsUsernameExistForAnotherUser(string username, Guid excludeUserId);
        Task<User?> GetByEmailAsync(string email);

 
    }
}
