using PE.WPF.UILibrary.Models;
using System.Threading.Tasks;

namespace PE.WPF.Services
{
    public interface IUserService
    {
        Task GetLoggedInUserInfo(string token, string refreshToken);
    }
}