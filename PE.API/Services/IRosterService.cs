using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public interface IRosterService
    {
        Task<bool> CreateRosterAsync(Roster roster);
        Task<Roster> GetRosterByIdAsync(string rosterId);
        List<Roster> GetRosters(List<string> rosterIds, PaginationFilter paginationFilter = null);
        Task<bool> UpdateRosterAsync(Roster rosterToUpdate);
        Task<bool> DeleteRosterAsync(string rosterId);
        Task<bool> DeleteRosterAsync(Roster roster);
    }
}