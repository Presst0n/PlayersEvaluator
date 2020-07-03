using PE.DomainModels;
using PE.RepositoryLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PE.Repository
{
    public interface IRosterRepository
    {
        Task<bool> CreateRosterAsync(Roster roster);
        Task<List<string>> GetAllRostersIdsAsync();
        Task<Roster> GetRosterByIdAsync(string rosterId);
        IEnumerable<Roster> GetRosters(int pageToSkip, int pageSize, List<string> rosterIds);
        IEnumerable<Roster> GetRosters(List<string> rosterIds);
        Task<bool> UpdateRosterAsync(Roster rosterToUpdate);
        Task<bool> DeleteRosterAsync(Roster rosterToDelete);
    }
}