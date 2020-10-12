using PE.DataManager.Dto;
using PE.WPF.UILibrary.Models;
using System.Threading.Tasks;

namespace PE.WPF.Service.Interfaces
{
    public interface IUserService
    {
        Task CreateLocalUserAsync();
        User GetLastLoggedInLocalUser();
        Task<User> GetLocalUserByAuthId(string authId);
        Task<User> GetLocalUserByEmailAsync(string email);
        Task GetLoggedInUserAsync(string token, string refreshToken);
        Task<bool> UpdateLocalUserAsync();
    }
}