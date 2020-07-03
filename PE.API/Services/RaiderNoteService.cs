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
    public class RaiderNoteService : IRaiderNoteService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RaiderNoteService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateRaiderNoteAsync(RaiderNote note)
        {
            await _context.RaiderNotesDto.AddAsync(_mapper.Map<RaiderNoteDto>(note));
            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> DeleteRaiderNoteAsync(RaiderNote note)
        {
            _context.RaiderNotesDto.Remove(_mapper.Map<RaiderNoteDto>(note));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RaiderNote> GetRaiderNoteByIdAsync(string noteId)
        {
            return _mapper.Map<RaiderNote>(await _context.RaiderNotesDto.AsNoTracking().SingleOrDefaultAsync(x => x.RaiderNoteId == noteId));
        }

        public async Task<List<RaiderNote>> GetRaiderNotesByRaiderIdAsync(string raiderId, PaginationFilter pagination)
        {
            var queryable = _context.RaiderNotesDto.AsQueryable();

            if (pagination is null)
                return _mapper.Map<List<RaiderNote>>(await queryable.AsNoTracking()
                    .Where(x => x.Raider.RaiderId == raiderId).ToListAsync());

            var skip = (pagination.PageNumber - 1) * pagination.PageSize;

            return _mapper.Map<List<RaiderNote>>(await queryable.AsNoTracking()
                .Where(x => x.Raider.RaiderId == raiderId).Skip(skip).Take(pagination.PageSize).ToListAsync());
        }

        public async Task<bool> UpdateRaiderNoteAsync(RaiderNote note)
        {
            _context.RaiderNotesDto.Update(_mapper.Map<RaiderNoteDto>(note));
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
