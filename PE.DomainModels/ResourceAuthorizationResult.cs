using System;
using System.Collections.Generic;
using System.Text;

namespace PE.DomainModels
{
    public class ResourceAuthorizationResult
    {
        public bool ReadOnlyAccess { get; set; }
        public bool IsOwner { get; set; }
        public bool IsModerator { get; set; }
    }
}
