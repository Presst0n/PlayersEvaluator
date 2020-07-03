using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PE.API.Data;
using PE.API.Dtos;
using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PE.API.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public IdentityRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            return _mapper.Map<RefreshToken>(await _context.RefreshTokensDto.AsNoTracking().SingleOrDefaultAsync(x => x.Token == refreshToken));
        }

        public async Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokensDto.Update(_mapper.Map<RefreshTokenDto>(refreshToken));
            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<string> AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            var respone = await _context.RefreshTokensDto.AddAsync(_mapper.Map<RefreshTokenDto>(refreshToken));
            await _context.SaveChangesAsync();
            var token = respone.CurrentValues.GetValue<string>("Token");

            return string.IsNullOrEmpty(token) ? null : token;
        }

        public async Task<bool> DeleteRefreshTokenAsync(RefreshToken refreshToken)
        {
            var token = await _context.RefreshTokensDto.SingleOrDefaultAsync(x => x.JwtId == refreshToken.JwtId);
            _context.RefreshTokensDto.Remove(token);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
