using PE.DomainModels;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task<AuthenticationResult> RegisterAsync(string email, string password, string userName);
    }
}