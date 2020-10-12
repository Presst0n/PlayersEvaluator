using PE.DataManager.Dto;
using PE.DataManager.Repository;
using PE.WPF.Service.Interfaces;
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
        private readonly IUserRepository _userRepo;
        private ILoggedInUserModel _loggedInUser;

        public UserService(IAPIHelper apiHelper, IUserRepository authRepository, ILoggedInUserModel loggedInUserModel)
        {
            _client = apiHelper.ApiClient;
            _userRepo = authRepository;
            _loggedInUser = loggedInUserModel;
        }

        public async Task GetLoggedInUserAsync(string token, string refreshToken)
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
                    _loggedInUser.LastLogIn = DateTime.Now;

                    if (!await UpdateLocalUserAsync())
                    {
                        await CreateLocalUserAsync();
                    }
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


        // TODO - Extract or remove all these methods. Propably better idea is to just inject user repository into ViewModel who needs it. 
        //        This is kinda abomination... And I really don't like it.
        public User GetLastLoggedInLocalUser()
        {
            return _userRepo.GetLastLoggedInUser();
        }

        public async Task<User> GetLocalUserByEmailAsync(string email)
        {
            return await _userRepo.GetUserByEmailAsync(email);
        }

        public async Task<User> GetLocalUserByAuthId(string authId)
        {
            return await _userRepo.GetUserByAuthId(authId);
        }

        public async Task CreateLocalUserAsync()
        {
            await _userRepo.AddUserAsync(new User
            {
                AuthUserId = _loggedInUser.UserId,
                Name = _loggedInUser.UserName,
                EmailAddress = _loggedInUser.Email,
                CreatedAt = _loggedInUser.CreationDate,
                RefreshToken = _loggedInUser.RefreshToken,
                Token = _loggedInUser.Token,
                LastLoginDate = DateTime.Now
            });
        }

        public async Task<bool> UpdateLocalUserAsync()
        {
            return await _userRepo.UpdateUserAsync(new User
            {
                AuthUserId = _loggedInUser.UserId,
                Name = _loggedInUser.UserName,
                EmailAddress = _loggedInUser.Email,
                CreatedAt = _loggedInUser.CreationDate,
                RefreshToken = _loggedInUser.RefreshToken,
                Token = _loggedInUser.Token,
                LastLoginDate = _loggedInUser.LastLogIn
            });
        }

        public async Task DeleteUserAsync()
        {

        }
    }
}
