using PE.WPF.Services.Interfaces;
using PE.WPF.UILibrary.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PE.WPF.Services
{
    public class NoteService : INoteService
    {
        private readonly HttpClient _client;

        public NoteService(IAPIHelper apiHelper)
        {
            _client = apiHelper.ApiClient;
        }

        public async Task<bool> CreateRaiderNote(string raiderId, string message)
        {
            // TODO - Connect to API and hit Create (raider-note) endpoint.  

            using (HttpResponseMessage response = await _client.PostAsJsonAsync("api/v1/raiderNotes", new { raiderId, message }))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;

                    //throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> EditRaiderNote(string raiderNoteId, string message)
        {
            using (HttpResponseMessage response = await _client.PutAsJsonAsync($"api/v1/raiderNotes/{raiderNoteId}", new { message }))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;

                    //throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
