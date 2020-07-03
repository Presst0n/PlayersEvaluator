using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PE.API.Data;
using PE.API.Dtos;
using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public class RaiderService : IRaiderService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RaiderService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Raider> GetRaiderByIdAsync(string raiderId)
        {
            return _mapper.Map<Raider>(await _context.RaidersDto.AsNoTracking().Include(n => n.Notes).SingleOrDefaultAsync(x => x.RaiderId == raiderId));
        }

        public async Task<List<Raider>> GetRaidersFromRosterAsync(string rosterId, PaginationFilter pagination = null)
        {
            var queryable = _context.RaidersDto.AsQueryable();

            if (pagination is null)
                return _mapper.Map<List<Raider>>(await queryable.AsNoTracking().Include(x => x.Notes)
                    .Where(x => x.RosterId == rosterId).ToListAsync());

            var skip = (pagination.PageNumber - 1) * pagination.PageSize;

            return _mapper.Map<List<Raider>>(await queryable.AsNoTracking().Include(x => x.Notes)
                .Where(x => x.RosterId == rosterId).Skip(skip).Take(pagination.PageSize).ToListAsync());
        }

        public async Task<bool> CreateRaiderAsync(Raider raider)
        {
            await _context.RaidersDto.AddAsync(_mapper.Map<RaiderDto>(raider));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRaiderAsync(Raider raiderToUpdate)
        {
            if (raiderToUpdate is null)
                return false;

            _context.RaidersDto.Update(_mapper.Map<RaiderDto>(raiderToUpdate));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRaidersInRosterAsync(string rosterId)
        {
            var raiders = await GetRaidersFromRosterAsync(rosterId);
            if (raiders is null)
                return false;

            var raidersDto = _mapper.Map<List<RaiderDto>>(raiders);
            _context.RemoveRange(raidersDto);

            return await _context.SaveChangesAsync() >= raidersDto.Count;
        }

        public async Task<bool> DeleteRaiderAsync(Raider raider)
        {
            if (raider is null)
                return false;

            _context.RaidersDto.Remove(_mapper.Map<RaiderDto>(raider));
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
