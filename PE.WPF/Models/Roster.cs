using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.Models
{
    public class Roster
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Description { get; set; }
        public List<Raider> Raiders { get; set; }
    }
}
