using PE.DomainModels;
using System.Threading.Tasks;

namespace PE.API.Repository
{
    public interface IIdentityRepository
    {
        Task<string> AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<bool> DeleteRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}