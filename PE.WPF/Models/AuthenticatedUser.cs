using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.Models
{
    public class AuthenticatedUser
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
