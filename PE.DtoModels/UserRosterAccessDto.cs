using PE.API.ExtendedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PE.DtoModels
{
    public class UserRosterAccessDto
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string RosterId { get; set; }
        public bool IsModerator { get; set; }
        public bool IsOwner { get; set; }
        public DateTime CreatedOn { get; set; }

        public string CreatorId { get; set; }
        [ForeignKey(nameof(CreatorId))]
        public ExtendedIdentityUser CreatedBy { get; set; }
    }
}
