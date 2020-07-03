using PE.API.ExtendedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PE.DtoModels
{
    public class RaiderNoteDto
    {
        [Key]
        public string RaiderId { get; set; }
        public string Message { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey(nameof(CreatorId))]
        public ExtendedIdentityUser CreatedBy { get; set; }

        public virtual RaiderDto Raider { get; set; }
    }
}
