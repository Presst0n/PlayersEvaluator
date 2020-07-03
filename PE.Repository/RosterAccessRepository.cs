using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PE.DomainModels;
using PE.RepositoryLibrary.Dtos;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE.Repository
{
    public class RosterAccessRepository : IRosterAccessRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RosterAccessRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateRosterAccessAsync(UserRosterAccess userRosterAccess)
        {
            await _context.UserRosterAccessesDto.AddAsync(_mapper.Map<UserRosterAccessDto>(userRosterAccess));

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<UserRosterAccess>> GetRosterAccessesAsync(string id)
        {
            var rosterAccesses = _context.UserRosterAccessesDto.AsQueryable();

            return _mapper.Map<List<UserRosterAccess>>(await rosterAccesses.Where(x => x.UserId == id).ToListAsync());
        }

        public async Task<UserRosterAccess> GetRosterAccessAsync(string userId, string rosterId)
        {
            var rosterAccess = await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId && x.RosterId == rosterId);

            return _mapper.Map<UserRosterAccess>(rosterAccess);
        }

        public async Task<UserRosterAccess> GetRosterAccessByIdAsync(Guid id)
        {
            return _mapper.Map<UserRosterAccess>(await _context.UserRosterAccessesDto.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id));
        }

        public async Task<bool> DeleteRosterAccessAsync(UserRosterAccess rosterAccess)
        {
            _context.UserRosterAccessesDto.Remove(_mapper.Map<UserRosterAccessDto>(rosterAccess));
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
