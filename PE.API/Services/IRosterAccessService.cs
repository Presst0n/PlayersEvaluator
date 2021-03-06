﻿using PE.DomainModels;
using PE.DomainModels.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public interface IRosterAccessService
    {
        Task<bool> CreateRosterAccessAsync(UserRosterAccess userRosterAccess);
        Task<bool> DeleteRosterAccessAsync(Guid rosterAccessId);
        Task<bool> DeleteRosterAccessAsync(UserRosterAccess rosterToDelete);

        /// <summary>
        /// Removes all roster accesses for given roster from Database.
        /// </summary>
        /// <param name="rosterId"></param>
        /// <returns>The <see cref="System.Threading.Tasks.Task"/> that represents the asynchronous operation, containing
        ///     true if the operation succeeded, otherwise false.</returns>
        Task<bool> DeleteRosterAccessesAsync(string rosterId);
        //Task<UserRosterAccess> GetRosterAccessAsync(string userId, string rosterId);
        //Task<UserRosterAccess> GetRosterAccessAsync(string userId, RaiderNote raiderNote);
        Task<UserRosterAccess> GetRosterAccessAsync(GetBy getBy, string userId, string id);
        Task<UserRosterAccess> GetRosterAccessByIdAsync(Guid id);
        //Task<UserRosterAccess> GetRosterAccessByRaiderIdAsync(string userId, string raiderId);
        Task<List<UserRosterAccess>> GetRosterAccessesByRosterIdAsync(string rosterId, PaginationFilter pagination = null);
        Task<List<UserRosterAccess>> GetRosterAccessesByUserIdAsync(string userId);
        Task<bool> UpdateRosterAccessAsync(UserRosterAccess rosterAccessToUpdate);
    }
}