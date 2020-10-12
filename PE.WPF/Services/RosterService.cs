using PE.WPF.Models;
using PE.WPF.Models.Requests;
using PE.WPF.Models.Responses;
using PE.WPF.Service.Interfaces;
using PE.WPF.UILibrary.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PE.WPF.Services
{
    public class RosterService : IRosterService
    {
        private readonly HttpClient _client;

        public RosterService(IAPIHelper apiHelper)
        {
            _client = apiHelper.ApiClient;
        }

        public async Task<PagedResponse<Roster>> GetRostersAsync()
        {
            using (HttpResponseMessage response = await _client.GetAsync("api/v1/rosters"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<PagedResponse<Roster>>();
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
