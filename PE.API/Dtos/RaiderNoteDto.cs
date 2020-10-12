using PE.API.ExtendedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PE.API.Dtos
{
    public class RaiderNoteDto
    {
        [Key]
        public string RaiderNoteId { get; set; }

        [ForeignKey(nameof(Raider))]
        public string RaiderId { get; set; }
        public string Message { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public virtual RaiderDto Raider { get; set; }
    }
}
