using Microsoft.AspNetCore.WebUtilities;
using PE.Contracts.V1;
using PE.Contracts.V1.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllRostersUri(PaginationQuery paginationQuery = null)
        {
            var uri = new Uri(_baseUri);

            if (paginationQuery == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", paginationQuery.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetRosterUri(string rosterId)
        {
            return new Uri(_baseUri + ApiRoutes.Rosters.Get.Replace("{rosterId}", rosterId));
        }

        public Uri GetRaiderUri(string raiderId)
        {
            return new Uri(_baseUri + ApiRoutes.Raiders.Get.Replace("{raiderId}", raiderId));
        }

        public Uri GetRaiderNoteUri(string raiderNoteId)
        {
            return new Uri(_baseUri + ApiRoutes.RaiderNotes.Get.Replace("{raiderNoteId}", raiderNoteId));
        }

        public Uri GetRosterAccessUri(string rosterAccessId)
        {
            return new Uri(_baseUri + ApiRoutes.RosterAccesses.Get);
        }
    }
}
