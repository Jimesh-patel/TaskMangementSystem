using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMangementSystem.Models;

namespace TaskMangementSystem.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserByIdAsync(int id);
        Task<UserModel> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task AddUserAsync(UserModel user);
        Task UpdateUserAsync(UserModel user);
        Task DeleteUserAsync(int id);
    }
}
