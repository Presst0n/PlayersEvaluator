using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.Models
{
    public class RaiderNote
    {
        public string RaiderNoteId { get; set; }
        public string RaiderId { get; set; }
        public string Message { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
    }
}
