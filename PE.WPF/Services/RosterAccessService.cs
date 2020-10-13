using PE.WPF.Models;
using PE.WPF.Models.Responses;
using PE.WPF.Services.Interfaces;
using PE.WPF.UILibrary.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PE.WPF.Services
{
    public class RosterAccessService : IRosterAccessService
    {
        private readonly HttpClient _client;

        public RosterAccessService(IAPIHelper apiHelper)
        {
            _client = apiHelper.ApiClient;
        }

        public async Task<PagedResponse<RosterAccess>> GetRosterAccessAsync(string rosterId)
        {
            using (var response = await _client.GetAsync($"api/v1/rosterAccesses?rosterId={rosterId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<PagedResponse<RosterAccess>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
