using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.Models
{
    public class AuthenticatedUser
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
