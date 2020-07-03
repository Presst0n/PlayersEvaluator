using System;
using System.Collections.Generic;
using System.Text;

namespace PE.Contracts.V1.Requests
{
    public class CreateRosterRequest
    {
        public string Name { get; set; }
        public string CreatorName { get; set; }
        public string Description { get; set; }
        //public List<CreateRaiderRequest> Raiders { get; set; }
    }
}
