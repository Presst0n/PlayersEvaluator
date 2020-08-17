using PE.DataManager.Repository;
using PE.WPF.Helpers;
using PE.WPF.Models;
using PE.WPF.Models.Requests;
using PE.WPF.UILibrary.Api;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PE.WPF.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private readonly IUserRepository _authRepository;

        public AuthService(IAPIHelper apiHelper, IUserRepository authRepository)
        {
            _client = apiHelper.ApiClient;
            _authRepository = authRepository;
        }

        public async Task<AuthenticatedUser> AuthenticateAsync(string email, string password)
        {
            using (HttpResponseMessage response = await _client.PostAsJsonAsync("api/v1/identity/login", new LoginRequest { Email = email, Password = password }))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<AuthenticatedUser> RegisterAsync(string email, string userName, string password)
        {
            using (HttpResponseMessage response = await _client.PostAsJsonAsync("api/v1/identity/register", new RegisterRequest { Email = email, UserName = userName, Password = password }))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    await _authRepository.AddUserAsync(new DataManager.Dto.User
                    {
                        RefreshToken = result.RefreshToken,
                        Token = result.Token
                     });

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
