using System;

namespace PE.DomainModels
{
    public class RefreshToken
    {
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public string UserId { get; set; }
    }
}
