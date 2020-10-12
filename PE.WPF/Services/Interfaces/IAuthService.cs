using PE.WPF.Models;
using PE.WPF.Models.Responses;
using System.Threading.Tasks;

namespace PE.WPF.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(string email, string password);
        Task<AuthResponse> RefreshUserLoginAsync(string token, string refreshToken);
        Task<AuthResponse> RegisterAsync(string email, string userName, string password);
    }
}