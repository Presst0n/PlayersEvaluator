using System;
using System.Collections.Generic;
using System.Text;

namespace PE.DomainModels
{
    public class RaiderNote
    {
        public string RaiderNoteId { get; set; }
        public string RaiderId { get; set; }
        public string Message { get; set; }
        public string CreatorId { get; set; }
    }
}
