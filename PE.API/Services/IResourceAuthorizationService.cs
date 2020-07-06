using PE.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public interface IResourceAuthorizationService
    {
        Task<ResourceAuthorizationResult> AuthorizeAsync(string userId, Raider raider);
        Task<ResourceAuthorizationResult> AuthorizeAsync(string userId, RaiderNote note);
        Task<ResourceAuthorizationResult> AuthorizeAsync(string userId, Roster roster);
        Task<ResourceAuthorizationResult> AuthorizeAsync(string userId, string resourceId);
        Task<List<string>> GetAuthorizedRosterIds(string userId);
    }
}