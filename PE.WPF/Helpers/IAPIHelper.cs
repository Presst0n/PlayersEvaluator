using PE.WPF.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace PE.WPF.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string email, string password);
    }
}