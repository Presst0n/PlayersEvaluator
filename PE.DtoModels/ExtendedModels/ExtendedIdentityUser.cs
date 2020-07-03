using Microsoft.AspNetCore.Identity;
using System;

namespace PE.API.ExtendedModels
{
    public class ExtendedIdentityUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
    }
}
