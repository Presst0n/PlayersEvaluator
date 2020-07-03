using PE.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public interface IRaiderService
    {
        Task<bool> CreateRaiderAsync(Raider raider);
        Task<bool> DeleteRaiderAsync(Raider raider);
        Task<bool> DeleteRaidersInRosterAsync(string rosterId);
        Task<Raider> GetRaiderByIdAsync(string raiderId);
        Task<List<Raider>> GetRaidersFromRosterAsync(string rosterId, PaginationFilter pagination = null);
        Task<bool> UpdateRaiderAsync(Raider raiderToUpdate);
    }
}