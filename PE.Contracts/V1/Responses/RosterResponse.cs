using System;
using System.Collections.Generic;
using System.Text;

namespace PE.Contracts.V1.Responses
{
    public class RosterResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Description { get; set; }
        public List<RaiderResponse> Raiders { get; set; }
    }
}
