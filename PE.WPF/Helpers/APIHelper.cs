using PE.WPF.Models;
using PE.WPF.Models.Requests;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PE.WPF.Helpers
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _apiClient;

        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["Api"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string email, string password)
        {

            using (HttpResponseMessage response = await _apiClient.PostAsJsonAsync("api/v1/identity/login", new LoginRequest { Email = email, Password = password }))
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
    }
}
