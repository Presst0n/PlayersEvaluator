using System;
using System.Collections.Generic;
using System.Text;

namespace PE.DomainModels
{
    public class Raider
    {
        public string RaiderId { get; set; }
        public string RosterId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string MainSpecialization { get; set; }
        public string OffSpecialization { get; set; }
        public string Role { get; set; }
        public int Points { get; set; } = 0;
        public List<RaiderNote> Notes { get; set; }
    }
}
