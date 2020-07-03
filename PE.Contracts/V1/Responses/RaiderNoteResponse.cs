using System;
using System.Collections.Generic;
using System.Text;

namespace PE.Contracts.V1.Responses
{
    public class RaiderNoteResponse
    {
        public string RaiderId { get; set; }
        public string RaiderNoteId { get; set; }
        public string Message { get; set; }
        public string CreatorId { get; set; }
    }
}
