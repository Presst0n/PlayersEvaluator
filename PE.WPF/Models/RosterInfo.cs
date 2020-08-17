using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.Models
{
    public class RosterInfo
    {
        public int RosterId { get; set; }
        public string RosterName { get; set; }
        public string CreationDate { get; set; }
        public List<string> Raiders { get; set; } = new List<string>();
        public string Description { get; set; }
        public string GuildLogo { get; set; }
    }
}
