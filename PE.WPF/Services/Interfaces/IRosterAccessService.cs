using PE.WPF.Models;
using PE.WPF.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PE.WPF.Services.Interfaces
{
    public interface IRosterAccessService
    {
        Task<PagedResponse<RosterAccess>> GetRosterAccessAsync(string rosterId);
    }
}
