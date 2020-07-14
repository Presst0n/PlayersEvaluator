using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PE.API.Data;
using PE.API.Dtos;
using PE.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public class ResourceAuthorizationService : IResourceAuthorizationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ResourceAuthorizationService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResourceAuthorizationResult> AuthorizeAsync(string userId, Roster roster)
        {
            if (roster is null)
                return null;

            var access = _mapper.Map<UserRosterAccess>(await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == roster.Id));

            return GenerateAuthorizationResult(access);
        }

        public async Task<ResourceAuthorizationResult> AuthorizeAsync(string userId, Raider raider)
        {
            if (raider is null)
                return null;

            var rosterId = await _context.RaidersDto.Where(r => r.RaiderId == raider.RaiderId).Select(x => x.RosterId).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(rosterId))
                return null;

            var access = _mapper.Map<UserRosterAccess>(await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId));

            return GenerateAuthorizationResult(access);
        }

        public async Task<ResourceAuthorizationResult> AuthorizeAsync(string userId, RaiderNote note)
        {
            if (note is null)
                return null;

            var rosterId = _context.RaidersDto.AsQueryable().Where(x => x.RaiderId == note.RaiderId).Select(r => r.RosterId).FirstOrDefault();

            if (string.IsNullOrEmpty(rosterId))
                return null;

            var access = _mapper.Map<UserRosterAccess>(await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId));

            return GenerateAuthorizationResult(access);
        }

        public async Task<ResourceAuthorizationResult> AuthorizeAsync(string userId, string resourceId)
        {
            //if (string.IsNullOrEmpty(resourceId))
            //    return null;

            if (_context.RostersDto.Any(x => x.Id == resourceId))
            {
                var access_ = _mapper.Map<UserRosterAccess>(await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == resourceId));

                return GenerateAuthorizationResult(access_);
            }

            var rosterId = _context.RaidersDto.AsQueryable().Where(r => r.RaiderId == resourceId).Select(x => x.RosterId).FirstOrDefault();
            var access = _mapper.Map<UserRosterAccess>(await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId));

            return GenerateAuthorizationResult(access);
        }

        public async Task<List<string>> GetAuthorizedRosterIds(string userId)
        {
            return await _context.UserRosterAccessesDto.AsQueryable().Where(x => x.UserId == userId).Select(r => r.RosterId).ToListAsync();
        }

        private ResourceAuthorizationResult GenerateAuthorizationResult(UserRosterAccess access)
        {
            if (access is null || string.IsNullOrEmpty(access.UserId))
            {
                return new ResourceAuthorizationResult
                {
                    ReadOnlyAccess = false,
                    IsModerator = false,
                    IsOwner = false
                };
            }

            return new ResourceAuthorizationResult
            {
                ReadOnlyAccess = true,
                IsModerator = access.IsModerator,
                IsOwner = access.IsOwner
            };
        }

        private ResourceAuthorizationResult GenerateAuthorizationResult(UserRosterAccessDto access)
        {
            if (access is null || string.IsNullOrEmpty(access.UserId))
            {
                return new ResourceAuthorizationResult
                {
                    ReadOnlyAccess = false,
                    IsModerator = false,
                    IsOwner = false
                };
            }

            return new ResourceAuthorizationResult
            {
                ReadOnlyAccess = true,
                IsModerator = access.IsModerator,
                IsOwner = access.IsOwner
            };
        }

        // TODO - Is this service really necessery? Take a moment to reconsider this approach, cuz I have some doubts.
        // Maybe I should rebuild/refactor RosterAccessService or maybe I should create this service where every method 
        // will return simplified version of model with only required data to evaluate if user has access to requested resouce... Ight Imma head out.
    }
}
