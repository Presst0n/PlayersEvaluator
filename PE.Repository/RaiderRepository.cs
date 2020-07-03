using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE.Repository
{
    public class RaiderRepository : IRaiderRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RaiderRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Raider> GetRaiderByIdAsync(string raiderId)
        {
            return _mapper.Map<Raider>(await _context.RaidersDto.AsNoTracking().SingleOrDefaultAsync(x => x.Id == raiderId));
        }

        public async Task<List<Raider>> GetRaidersFromRosterAsync(string rosterId)
        {
            return _mapper.Map<List<Raider>>(await _context.RaidersDto.AsNoTracking().Include(x => x.Notes)
                .Where(x => x.RosterId == rosterId).ToListAsync());
        }

        public async Task<List<Raider>> GetRaidersFromRosterAsync(string rosterId, int pageToSkip, int pageSize)
        {
            var queryable = _context.RaidersDto.AsQueryable();

            return _mapper.Map<List<Raider>>(await queryable.AsNoTracking().Include(x => x.Notes)
                .Where(x => x.RosterId == rosterId).Skip(pageToSkip).Take(pageSize).ToListAsync());
        }

        public async Task<bool> CreateRaiderAsync(Raider raider)
        {
            await _context.RaidersDto.AddAsync(_mapper.Map<RaiderDto>(raider));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRaiderAsync(Raider raiderToUpdate)
        {
            _context.RaidersDto.Update(_mapper.Map<RaiderDto>(raiderToUpdate));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRaidersAsync(List<Raider> raiders)
        {
            var raidersDto = _mapper.Map<List<RaiderDto>>(raiders);
            _context.RemoveRange(raidersDto);
            return await _context.SaveChangesAsync() >= raidersDto.Count;
        }

        public async Task<bool> DeleteRaiderAsync(Raider raider)
        {
            _context.RaidersDto.Remove(_mapper.Map<RaiderDto>(raider));
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
