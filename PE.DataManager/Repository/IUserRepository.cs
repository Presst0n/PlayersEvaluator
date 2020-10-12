using PE.DataManager.Dto;
using System.Threading.Tasks;

namespace PE.DataManager.Repository
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        User GetLastLoggedInUser();
        Task<User> GetUserByAuthId(string authId);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(User user);
    }
}