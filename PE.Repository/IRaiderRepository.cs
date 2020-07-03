using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PE.Repository
{
    public interface IRaiderRepository
    {
        Task<bool> CreateRaiderAsync(Raider raider);
        Task<bool> DeleteRaiderAsync(Raider raider);
        Task<bool> DeleteRaidersAsync(List<Raider> raiders);
        Task<Raider> GetRaiderByIdAsync(string raiderId);
        Task<List<string>> GetRaidersIdsAsync();
        Task<List<Raider>> GetRaidersFromRosterAsync(string rosterId);
        Task<List<Raider>> GetRaidersFromRosterAsync(string rosterId, int pageToSkip, int pageSize);
        Task<bool> UpdateRaiderAsync(Raider raiderToUpdate);
    }
}
