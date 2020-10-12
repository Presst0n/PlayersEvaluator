using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PE.WPF.Helpers;
using PE.WPF.Models.Requests;
using PE.WPF.Models.Responses;
using PE.WPF.Service.Interfaces;
using PE.WPF.UILibrary.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PE.WPF.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;

        public AuthService(IAPIHelper apiHelper)
        {
            _client = apiHelper.ApiClient;
        }

        public async Task<AuthResponse> AuthenticateAsync(string email, string password)
        {
            using (HttpResponseMessage response = await _client.PostAsJsonAsync("api/v1/identity/login", new LoginRequest { Email = email, Password = password }))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<AuthResponse>();
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<AuthResponse>();
                    if (result.Errors != null)
                    {
                        var error = result.Errors.FirstOrDefault();
                        throw new Exception(error.Message);
                    }
                    throw new Exception(result.CustomErrors != null ? result.CustomErrors.FirstOrDefault() : response.ReasonPhrase);
                }
            }
        }

        public async Task<AuthResponse> RefreshUserLoginAsync(string token, string refreshToken)
        {
            using (HttpResponseMessage response = await _client.PostAsJsonAsync("api/v1/identity/refresh", new RefreshTokenRequest { Token = token, RefreshToken = refreshToken }))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<AuthResponse>();
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<AuthResponse>();
                    throw new Exception(result.CustomErrors != null ? result.CustomErrors.FirstOrDefault() : response.ReasonPhrase);
                }
            }
        }

        public async Task<AuthResponse> RegisterAsync(string email, string userName, string password)
        {
            using (HttpResponseMessage response = await _client.PostAsJsonAsync("api/v1/identity/register", new RegisterRequest { Email = email, UserName = userName, Password = password }))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<AuthResponse>();
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<AuthResponse>();
                    throw new Exception(result.CustomErrors != null ? result.CustomErrors.FirstOrDefault() : response.ReasonPhrase);
                }
            }
        }
    }
}
