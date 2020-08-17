using PE.DataManager.Dto;
using PE.DataManager.Repository;
using PE.WPF.UILibrary.Api;
using PE.WPF.UILibrary.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PE.WPF.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly IUserRepository _authRepository;
        private ILoggedInUserModel _loggedInUser;

        public UserService(IAPIHelper apiHelper, IUserRepository authRepository, ILoggedInUserModel loggedInUserModel)
        {
            _client = apiHelper.ApiClient;
            _authRepository = authRepository;
            _loggedInUser = loggedInUserModel;
        }

        public async Task GetLoggedInUserInfo(string token, string refreshToken)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (HttpResponseMessage response = await _client.GetAsync("api/v1/users/user"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.CreationDate = result.CreationDate;
                    _loggedInUser.Email = result.Email;
                    _loggedInUser.UserName = result.UserName;
                    _loggedInUser.UserId = result.UserId;
                    _loggedInUser.Token = token;
                    _loggedInUser.RefreshToken = refreshToken;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
