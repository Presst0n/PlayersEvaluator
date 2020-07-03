using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.ExtendedModels
{
    public class CustomRole : IdentityRole
    {
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }

        public CustomRole(string roleName) : base(roleName)
        {
        }
    }
}
