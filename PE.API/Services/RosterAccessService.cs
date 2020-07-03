using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PE.API.Data;
using PE.API.Dtos;
using PE.DomainModels;
using PE.DomainModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public class RosterAccessService : IRosterAccessService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RosterAccessService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> CreateRosterAccessAsync(UserRosterAccess userRosterAccess)
        {
            var exists = _context.UserRosterAccessesDto.Any(r => r.RosterId == userRosterAccess.RosterId && r.UserId == userRosterAccess.UserId);

            if (exists)
                return false;

            await _context.UserRosterAccessesDto.AddAsync(_mapper.Map<UserRosterAccessDto>(userRosterAccess));

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<UserRosterAccess>> GetRosterAccessesByUserIdAsync(string userId)
        {
            var rosterAccesses = _context.UserRosterAccessesDto.AsQueryable();

            return _mapper.Map<List<UserRosterAccess>>(await rosterAccesses.Where(x => x.UserId == userId).ToListAsync());
        }

        public async Task<UserRosterAccess> GetRosterAccessAsync(GetBy getBy, string userId, string id)
        {
            return getBy switch
            {
                GetBy.RosterId => await GetByRosterIdAsync(userId, id),
                GetBy.RaiderId => await GetByRaiderIdAsync(userId, id),
                _ => null,
            };
        }

        //public async Task<UserRosterAccess> GetRosterAccessAsync(string userId, string rosterId)
        //{
        //    if (string.IsNullOrEmpty(rosterId))
        //        return null;

        //    var rosterAccess = await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId);

        //    return _mapper.Map<UserRosterAccess>(rosterAccess);
        //}

        //public async Task<UserRosterAccess> GetRosterAccessByRaiderIdAsync(string userId, string raiderId)
        //{
        //    if (string.IsNullOrEmpty(raiderId))
        //        return null;

        //    var rosterId = _context.RaidersDto.AsQueryable().Where(r => r.RaiderId == raiderId).Select(x => x.RosterId).FirstOrDefault();

        //    if (string.IsNullOrEmpty(rosterId))
        //        return null;

        //    var rosterAccess = await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId);

        //    return _mapper.Map<UserRosterAccess>(rosterAccess);
        //}

        //public async Task<UserRosterAccess> GetRosterAccessAsync(string userId, RaiderNote raiderNote)
        //{
        //    if (raiderNote is null)
        //        return null;

        //    var rosterId = _context.RaidersDto.AsQueryable().Where(x => x.RaiderId == raiderNote.RaiderId).Select(r => r.RosterId).FirstOrDefault();

        //    if (string.IsNullOrEmpty(rosterId))
        //        return null;

        //    var rosterAccess = await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId);

        //    return _mapper.Map<UserRosterAccess>(rosterAccess);
        //}

        public async Task<List<UserRosterAccess>> GetRosterAccessesByRosterIdAsync(string rosterId, PaginationFilter pagination = null)
        {
            if (string.IsNullOrEmpty(rosterId))
                return null;

            var accesses = _context.UserRosterAccessesDto.AsQueryable();

            if (pagination is null)
            {
                return _mapper.Map<List<UserRosterAccess>>(await accesses.AsNoTracking()
                    .Where(x => x.RosterId == rosterId).ToListAsync());
            }

            var skip = (pagination.PageNumber - 1) * pagination.PageSize;

            return _mapper.Map<List<UserRosterAccess>>(await accesses.AsNoTracking()
                .Where(x => x.RosterId == rosterId).Skip(skip).Take(pagination.PageSize).ToListAsync());
        }

        public async Task<UserRosterAccess> GetRosterAccessByIdAsync(Guid id)
        {
            return _mapper.Map<UserRosterAccess>(await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id));
        }

        public async Task<bool> DeleteRosterAccessAsync(Guid rosterAccessId)
        {
            var rosterAccess = await GetRosterAccessByIdAsync(rosterAccessId);

            if (rosterAccess is null)
                return false;

            _context.UserRosterAccessesDto.Remove(_mapper.Map<UserRosterAccessDto>(rosterAccess));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRosterAccessAsync(UserRosterAccess rosterToDelete)
        {
            var accessToDelete = _context.UserRosterAccessesDto.Any(x => x.Id == rosterToDelete.Id);

            if (!accessToDelete)
                return false;

            _context.Remove(_mapper.Map<UserRosterAccessDto>(rosterToDelete));

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRosterAccessesAsync(string rosterId)
        {
            var accessesToDelete = await _context.UserRosterAccessesDto.Where(x => x.RosterId == rosterId).ToListAsync();

            _context.RemoveRange(accessesToDelete);
            var deleted = await _context.SaveChangesAsync();

            return deleted == accessesToDelete.Count;
        }

        public async Task<bool> UpdateRosterAccessAsync(UserRosterAccess rosterAccessToUpdate)
        {
            _context.UserRosterAccessesDto.Update(_mapper.Map<UserRosterAccessDto>(rosterAccessToUpdate));
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<UserRosterAccess> GetByRosterIdAsync(string userId, string rosterId)
        {
            if (string.IsNullOrEmpty(rosterId))
                return null;

            var rosterAccess = await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId);

            return _mapper.Map<UserRosterAccess>(rosterAccess);
        }


        private async Task<UserRosterAccess> GetByRaiderIdAsync(string userId, string raiderId)
        {
            if (string.IsNullOrEmpty(raiderId))
                return null;

            var rosterId = _context.RaidersDto.AsQueryable().Where(r => r.RaiderId == raiderId).Select(x => x.RosterId).FirstOrDefault();

            if (string.IsNullOrEmpty(rosterId))
                return null;

            var rosterAccess = await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId);

            return _mapper.Map<UserRosterAccess>(rosterAccess);
        }
    }
}
