using System;
using System.Collections.Generic;
using System.Text;

namespace PE.DomainModels
{
    public class UserRosterAccess
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string RosterId { get; set; }
        public bool IsModerator { get; set; }
        public bool IsOwner { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatorId { get; set; }
    }
}
