using PE.DataManager.Dto;
using System.Threading.Tasks;

namespace PE.DataManager.Repository
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
    }
}