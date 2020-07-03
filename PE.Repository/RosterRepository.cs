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
    public class RosterRepository : IRosterRepository, IDisposable
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private bool disposedValue;

        public RosterRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateRosterAsync(Roster roster)
        {
            await _context.RostersDto.AddAsync(_mapper.Map<RosterDto>(roster));
            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> UpdateRosterAsync(Roster rosterToUpdate)
        {
            _context.RostersDto.Update(_mapper.Map<RosterDto>(rosterToUpdate));
            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> DeleteRosterAsync(Roster rosterToDelete)
        {
            _context.RostersDto.Remove(_mapper.Map<RosterDto>(rosterToDelete));
            var deleted = await _context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<Roster> GetRosterByIdAsync(string rosterId)
        {
            return _mapper.Map<Roster>(await _context.RostersDto.AsNoTracking().Include(x => x.Raiders).AsNoTracking().SingleOrDefaultAsync(r => r.Id == rosterId));
        }

        public IEnumerable<Roster> GetRosters(List<string> rosterIds)
        {
            var queryable = _context.RostersDto.AsQueryable();
            var rosters = rosterIds.Select(x => queryable.Include(r => r.Raiders).SingleOrDefault(c => c.Id == x));

            return _mapper.Map<IEnumerable<Roster>>(rosters);
        }

        public IEnumerable<Roster> GetRosters(int pageToSkip, int pageSize, List<string> rosterIds)
        {
            var queryable = _context.RostersDto.AsQueryable();
            var rosters = rosterIds.Select(x => queryable.Include(r => r.Raiders).SingleOrDefault(c => c.Id == x)).Skip(pageToSkip).Take(pageSize);

            return _mapper.Map<IEnumerable<Roster>>(rosters);
        }

        public async Task<List<string>> GetAllRostersIdsAsync()
        {
            return await _context.RostersDto.Select(r => r.Id).ToListAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
