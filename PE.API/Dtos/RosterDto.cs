using PE.API.ExtendedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PE.API.Dtos
{
    public class RosterDto
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RaiderDto> Raiders { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public ExtendedIdentityUser User { get; set; }
    }
}
