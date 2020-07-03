using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public interface IRaiderNoteService
    {
        Task<RaiderNote> GetRaiderNoteByIdAsync(string noteId);
        Task<List<RaiderNote>> GetRaiderNotesByRaiderIdAsync(string raiderId, PaginationFilter pagination);
        Task<bool> CreateRaiderNoteAsync(RaiderNote note);
        Task<bool> UpdateRaiderNoteAsync(RaiderNote note);
        Task<bool> DeleteRaiderNoteAsync(RaiderNote note);
    }
}
