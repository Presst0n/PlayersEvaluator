using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PE.DtoModels
{
    public class RaiderDto
    {
        [Key]
        public string Id { get; set; }
        public string RosterId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string MainSpecialization { get; set; }
        public string OffSpecialization { get; set; }
        public string Role { get; set; }
        public int Points { get; set; }
        [ForeignKey(nameof(RosterId))]
        public virtual RosterDto RaidRoster { get; set; }
        public virtual List<RaiderNoteDto> Notes { get; set; }
    }
}
