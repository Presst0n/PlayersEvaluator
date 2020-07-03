using System.Collections;
using System.Collections.Generic;

namespace PE.Contracts.V1.Responses
{
    public class RaiderResponse
    {
        public string RaiderId { get; set; }
        public string RosterId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string MainSpecialization { get; set; }
        public string OffSpecialization { get; set; }
        public string Role { get; set; }
        public int Points { get; set; }
        public IEnumerable<RaiderNoteResponse> Notes { get; set; }
    }
}