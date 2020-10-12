using PE.WPF.Models;
using PE.WPF.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PE.WPF.Service.Interfaces
{
    public interface IRosterService
    {
        Task<PagedResponse<Roster>> GetRostersAsync();
    }
}