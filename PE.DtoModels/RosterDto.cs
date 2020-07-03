using PE.API.ExtendedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PE.DtoModels
{
    public class RosterDto
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public ExtendedIdentityUser User { get; set; }

        public virtual List<RaiderDto> Raiders { get; set; }
    }
}
