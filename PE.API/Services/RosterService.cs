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
    public class RosterService : IRosterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public RosterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> CreateRosterAsync(Roster roster)
        {
            await _context.RostersDto.AddAsync(_mapper.Map<RosterDto>(roster));
            
            return await _context.SaveChangesAsync() > 0;
        }

        public List<Roster> GetRosters(List<string> rosterIds, PaginationFilter paginationFilter = null)
        {
            var queryable = _context.RostersDto.AsQueryable();

            if (paginationFilter is null)
            {
                return _mapper.Map<List<Roster>>(rosterIds.Select(x => queryable.Include(r => r.Raiders).ThenInclude(c => c.Notes).SingleOrDefault(c => c.Id == x)));
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            var rosters = rosterIds.Select(x => queryable.Include(r => r.Raiders).ThenInclude(c => c.Notes).SingleOrDefault(c => c.Id == x)).Skip(skip).Take(paginationFilter.PageSize);

            return _mapper.Map<List<Roster>>(rosters);
        }

        public async Task<Roster> GetRosterByIdAsync(string rosterId)
        {
            return _mapper.Map<Roster>(await _context.RostersDto.AsNoTracking().Include(x => x.Raiders).AsNoTracking().SingleOrDefaultAsync(r => r.Id == rosterId));
        }

        public async Task<bool> UpdateRosterAsync(Roster rosterToUpdate)
        {
            if (rosterToUpdate == null)
                return false;

            _context.RostersDto.Update(_mapper.Map<RosterDto>(rosterToUpdate));

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRosterAsync(string rosterId)
        {
            var roster = await GetRosterByIdAsync(rosterId);
            if (roster is null)
                return false;

            _context.RostersDto.Remove(_mapper.Map<RosterDto>(roster));

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRosterAsync(Roster roster)
        {
            var exists = _context.RostersDto.Any(r => r.Id == roster.Id);
            if (!exists)
                return false;

            _context.RostersDto.Remove(_mapper.Map<RosterDto>(roster));

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
