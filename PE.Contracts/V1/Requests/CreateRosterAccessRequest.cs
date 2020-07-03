using System;
using System.Collections.Generic;
using System.Text;

namespace PE.Contracts.V1.Requests
{
    public class CreateRosterAccessRequest
    {
        public string UserId { get; set; }
        public string RosterId { get; set; }
        public bool IsModerator { get; set; }
    }
}
