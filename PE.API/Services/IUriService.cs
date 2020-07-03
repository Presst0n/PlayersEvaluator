using PE.Contracts.V1.Requests;
using System;

namespace PE.API.Services
{
    public interface IUriService
    {
        Uri GetRosterUri(string postId);
        Uri GetAllRostersUri(PaginationQuery paginationQuery = null);
        Uri GetRaiderUri(string raiderId);
        Uri GetRosterAccessUri(string rosterAccessId);
        Uri GetRaiderNoteUri(string raiderNoteId);
    }
}