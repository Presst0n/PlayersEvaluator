using PE.WPF.Models;
using System.Threading.Tasks;

namespace PE.WPF.Services
{
    public interface IAuthService
    {
        Task<AuthenticatedUser> AuthenticateAsync(string email, string password);
        Task<AuthenticatedUser> RegisterAsync(string email, string userName, string password);
    }
}