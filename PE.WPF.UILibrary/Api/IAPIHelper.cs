using System.Net.Http;
using System.Threading.Tasks;

namespace PE.WPF.UILibrary.Api
{
    public interface IAPIHelper
    {
        //Task<AuthenticatedUser> AuthenticateAsync(string email, string password);
        HttpClient ApiClient { get; }
    }
}