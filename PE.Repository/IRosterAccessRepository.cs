using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PE.Repository
{
    public interface IRosterAccessRepository
    {
        Task<bool> CreateRosterAccessAsync(UserRosterAccess userRosterAccess);
        Task<bool> DeleteRosterAccessAsync(UserRosterAccess rosterAccess);
        Task<UserRosterAccess> GetRosterAccessAsync(string userId, string rosterId);
        Task<UserRosterAccess> GetRosterAccessByIdAsync(Guid id);
        Task<List<UserRosterAccess>> GetRosterAccessesAsync(string id);
    }
}