using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PE.API.Dtos
{
    public class RaiderDto
    {
        [Key]
        public string RaiderId { get; set; }

        [ForeignKey(nameof(Roster))]
        public string RosterId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string MainSpecialization { get; set; }
        public string OffSpecialization { get; set; }
        public string Role { get; set; }
        public int Points { get; set; }

        public virtual RosterDto Roster { get; set; }
        public virtual ICollection<RaiderNoteDto> Notes { get; set; }
    }
}
